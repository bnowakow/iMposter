using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using iMposter.Controller.Face;
using System.Windows.Threading;
using System.Drawing;
using iMposter.Model.PeriodicTable;
using System.Windows.Media.Animation;
using iMposter.Model.Task;
using iMposter.Model.Sensor;
using Nui.Vision;
using iMposter.Model.ExtensionMethod;
using System.Windows.Media.Media3D;
using iMposter.Controller.Gesture;


namespace iMposter.Controller.Interaction
{
    public class PeriodicTableInteractionControler : IPeriodicTableInteractionControler
    {
        protected volatile IPeriodicTableControl periodicTableControl;
        protected IFaceDetector faceDetector;
        protected DispatcherTimer collectFacesTimer;
        protected DispatcherTimer processFacesTimer;

        // TODO check if thread synchronization is needed
        protected List<BitmapSource> facesToProcess;
        protected IList<Element> elements;

        protected int columnNumber = 18;
        protected int rowNumber = 7;

        protected BodyTracker bodyTracker;
        protected GestureDetector gestureDetector;
        protected int lastGestureCode;

        #region Constructor
        public PeriodicTableInteractionControler(IPeriodicTableControl periodicTableControl)
        {
            this.periodicTableControl = periodicTableControl;
            this.faceDetector = new FaceDetector();
            this.facesToProcess = new List<BitmapSource>();

            InitializePeriodicTableElementsExistance();
            this.periodicTableControl.InitializePeriodicTableElements(elements);

            InitializeCollectThread();
            InitializeProcessThread();

            bodyTracker = BodyTracker.Instance;
            bodyTracker.Tracker.UserUpdated += new Nui.Vision.NuiUserTracker.UserUpdatedHandler(Tracker_UserUpdated);

            gestureDetector = GestureDetector.Instance;
            gestureDetector.GestureComplete += new MyEventHandler(gestureDetector_GestureComplete);
        }
        #endregion

        #region CollectFacesThread
        public void CollectFacesFromCapture(object sender, EventArgs e)
        {
            var faces = faceDetector.DetectFaces();
            if (facesToProcess.Count() + faces.Count() < ControllerSettings.Default.interactionTableFaceStorageMaximalNumber)
            {
                facesToProcess.AddRange(faces);
            }
            else
            {
                foreach (var face in faces)
                {
                    if (facesToProcess.Count() < ControllerSettings.Default.interactionTableFaceStorageMaximalNumber)
                    {
                        facesToProcess.Add(face);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        #endregion

        #region ProcessFacesThread
        public void ProcessFacesFromCapture(object sender, EventArgs e)
        {
            BitmapSource face = facesToProcess.FirstOrDefault();
            Element randomElement = GetRandomNotOverriddenElementElement();
            if (face != null && randomElement != null)
            {
                facesToProcess.Remove(face);

                randomElement.NewImageSource = face;
                randomElement.IsOverridden = true;

                TaskChain animationTaskChain = new TaskChain(randomElement);
                animationTaskChain.TaskList.Add(new TaskChainElement(ElementFadeOutTask, 1));
                animationTaskChain.TaskList.Add(new TaskChainElement(ElementOverrideByNewTask, periodicTableControl.GetFadeTimeMiliseconds()));
                animationTaskChain.TaskList.Add(new TaskChainElement(ElementFadeInTask, periodicTableControl.GetFadeTimeMiliseconds()));
                animationTaskChain.TaskList.Add(new TaskChainElement(ElementFadeOutTask, ControllerSettings.Default.interactionTableFaceRenewalAnnimationMilisecondsDuration));
                animationTaskChain.TaskList.Add(new TaskChainElement(ElementRevertTask, periodicTableControl.GetFadeTimeMiliseconds()));
                animationTaskChain.TaskList.Add(new TaskChainElement(ElementFadeInTask, periodicTableControl.GetFadeTimeMiliseconds()));
                animationTaskChain.Execute();
            }
        }

        #region ElementAnimationTasks
        protected void ElementFadeOutTask(Object targetElement)
        {
            Element element = targetElement as Element;
            element.FadeOutElementImage();
        }

        protected void ElementFadeInTask(Object targetElement)
        {
            Element element = targetElement as Element;
            element.FadeInElementImage();
        }

        protected void ElementOverrideByNewTask(Object targetElement)
        {
            Element element = targetElement as Element;
            element.SetNewImage();
        }

        protected void ElementRevertTask(Object targetElement)
        {
            Element element = targetElement as Element;
            element.RevertDefaultImage();
        }
        #endregion

        protected Element GetRandomNotOverriddenElementElement()
        {
            // elements which are not currently animated or are scheduled to be animated
            var notOverriddenElements = from e in elements
                                        where e.IsOverridden == false
                                        select e;
            if (notOverriddenElements.Count() > 0)
            {
                Random random = new Random();
                int randomElementIndex = random.Next(notOverriddenElements.Count());
                return notOverriddenElements.ToArray()[randomElementIndex];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region ProccessSensorThread
        void gestureDetector_GestureComplete(double[][] gesturePath)
        {
            int recognizedLabelIndex = gestureDetector.HiddenMarkovModelDetect(gesturePath);
            if (recognizedLabelIndex == (int)GestureDetector.GestureCodes.NAVIGATION_GESTURE)
            {
                lastGestureCode = (int)GestureDetector.GestureCodes.NAVIGATION_GESTURE;
            }
            else
            {
                lastGestureCode = (int)GestureDetector.GestureCodes.ZOOM_GESTURE;
            }
        }

        protected double prevHandDistance = 0.0;
        protected double prevZoomDirection = 0.0;

        void Tracker_UserUpdated(object sender, Nui.Vision.NuiUserEventArgs e)
        {
            foreach (var user in e.Users)
            {
                periodicTableControl.GetCamera().Dispatcher.BeginInvoke((Action)delegate
                {
                    if (lastGestureCode == (int)GestureDetector.GestureCodes.NAVIGATION_GESTURE)
                    {
                        periodicTableControl.GetCamera().LookDirection =
                            new Vector3D(
                            //periodicTableControl.GetCamera().LookDirection.X,
                            periodicTableControl.GetCamera().LookDirection.X - ((user.RightHand.normalizedX() - 0.5) / 300),
                            //periodicTableControl.GetCamera().LookDirection.Y,
                            periodicTableControl.GetCamera().LookDirection.Y + ((user.RightHand.normalizedY() - 0.5) / 300),
                            periodicTableControl.GetCamera().LookDirection.Z
                            //periodicTableControl.GetCamera().LookDirection.Z + ((user.RightHand.normalizedZ() - 0.5) / 80)
                            );

                        prevHandDistance = 0.0;
                        prevZoomDirection = 0.0;
                    }

                    if (lastGestureCode == (int)GestureDetector.GestureCodes.ZOOM_GESTURE)
                    {
                        double zoomDirection = 0.0;
                        double curentHandDistance = Math.Abs(user.RightHand.normalizedX() - user.LeftHand.normalizedX());
                        double moveDistance = Math.Abs(prevHandDistance - curentHandDistance);
                        if (prevHandDistance != 0)
                        {
                            if (moveDistance > 0.02)
                            {
                                if (curentHandDistance < prevHandDistance)
                                {
                                    zoomDirection = 100;
                                }
                                else
                                {
                                    zoomDirection = -100;
                                }
                            }
                            else
                            {
                                zoomDirection = prevZoomDirection;
                            }
                        }
                        prevHandDistance = curentHandDistance;
                        prevZoomDirection = zoomDirection;
                        periodicTableControl.GetCamera().Position =
                            new Point3D(
                            periodicTableControl.GetCamera().Position.X,
                            //periodicTableControl.GetCamera().Position.X - ((user.RightHand.normalizedX() - 0.5) / 300),
                            periodicTableControl.GetCamera().Position.Y,
                            //periodicTableControl.GetCamera().Position.Y + ((user.RightHand.normalizedY() - 0.5) / 300),
                            //periodicTableControl.GetCamera().Position.Z
                            periodicTableControl.GetCamera().Position.Z + (zoomDirection * moveDistance)
                            );
                    }
                });
            }
        }
        #endregion

        #region Initialize collect and process threads
        protected void InitializeCollectThread()
        {
            collectFacesTimer = new DispatcherTimer();
            collectFacesTimer.Tick += new EventHandler(CollectFacesFromCapture);
            collectFacesTimer.Interval = TimeSpan.FromSeconds(ControllerSettings.Default.interactionTableFaceCaptureSecondInterval);
            collectFacesTimer.Start();
        }

        protected void InitializeProcessThread()
        {
            processFacesTimer = new DispatcherTimer();
            processFacesTimer.Tick += new EventHandler(ProcessFacesFromCapture);
            processFacesTimer.Interval = TimeSpan.FromSeconds(ControllerSettings.Default.interactionTableFaceProcessSecondInterval);
            processFacesTimer.Start();
        }
        #endregion

        #region InitializePeriodicTableElementsExistance
        protected void InitializePeriodicTableElementsExistance()
        {
            elements = new List<Element>();

            for (int row = 0; row < rowNumber; row++)
            {
                for (int column = 0; column < columnNumber; column++)
                {
                    if (!((row == 0 && column > 0 && column < 17) || (row < 3 && column > 1 && column < 12)))
                    {
                        Element element = new Element(new Point(column, row));
                        element.FadeElementImage = periodicTableControl.FadeElementImage;
                        elements.Add(element);
                    }
                }
            }
        }
        #endregion
    }
}
