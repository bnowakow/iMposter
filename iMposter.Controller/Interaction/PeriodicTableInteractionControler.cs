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
using iMposter.Model;
using System.ComponentModel;


namespace iMposter.Controller.Interaction
{
    public class PeriodicTableInteractionControler : IPeriodicTableInteractionControler
    {
        protected volatile IPeriodicTableControl periodicTableControl;
        protected IFaceDetector faceDetector;
        protected DispatcherTimer collectFacesTimer;
        protected DispatcherTimer processFacesTimer;

        protected List<BitmapSource> facesToProcess;
        protected IList<Element> elements;

        protected int columnNumber = 18;
        protected int rowNumber = 7;

        protected BodyTracker bodyTracker;
        protected GestureDetector gestureDetector;
        protected BufferList<int> lastGestureCodes;
        protected BufferList<NuiUser> lastUserCoordinates;

        #region Constructor
        public PeriodicTableInteractionControler(IPeriodicTableControl periodicTableControl)
        {
            this.periodicTableControl = periodicTableControl;
            this.faceDetector = new FaceDetector();
            this.facesToProcess = new List<BitmapSource>();
            // TODO change to settings and name it descriptively
            int consecutiveGesturesRequired = ControllerSettings.Default.consecutiveGesturesRequired;
            this.lastGestureCodes = new BufferList<int>(consecutiveGesturesRequired);
            this.lastUserCoordinates = new BufferList<NuiUser>(consecutiveGesturesRequired);

            InitializePeriodicTableElementsExistance();
            this.periodicTableControl.InitializePeriodicTableElements(elements);

            if (ControllerSettings.Default.interactionTableFaceStorageMaximalNumber > 0)
            {
                InitializeCollectThread();
                InitializeProcessThread();
            }
            
            bodyTracker = BodyTracker.Instance;
            // Wskazanie metody, do której bedą przekazane dane dotyczące śledzenia użytkownika
            bodyTracker.AddNewUserGestureHander(
                new Nui.Vision.NuiUserTracker.UserUpdatedHandler(ProcessUserGesture));

            gestureDetector = GestureDetector.Instance;
            // Wskazanie metody, do której bedą przekazane dane dotyczące wykrywanych gestów
            gestureDetector.GestureCaptureComplete += 
                new GestureCaptureCompleteDelegate(ColleUserGesture);
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
        public void ColleUserGesture(double[][] gesturePath)
        {
            lastGestureCodes.Add(gestureDetector.HiddenMarkovModelDetect(gesturePath));
        }

        public void ProcessUserGesture(object sender, Nui.Vision.NuiUserEventArgs e)
        {
            foreach (var user in e.Users)
            {
                if (lastUserCoordinates.List.Count() == 0 || !lastUserCoordinates.List.Last().Equals(user))
                {
                    lastUserCoordinates.Add(user.Copy());
                }
                if (lastGestureCodes.IsFilled && lastUserCoordinates.IsFilled)
                {
                    //periodicTableControl.GetCamera().Dispatcher.BeginInvoke((Action)delegate
                    //{
                        List<NuiUser> lastUserCoordinatesCopy = new List<NuiUser>(lastUserCoordinates.List);
                        var lastNavigationGestures = from code in lastGestureCodes.List
                                                     where code == (int)GestureDetector.GestureCodes.NAVIGATION
                                                     select code;
                        // TODO add thread synchronization for lastGestureCodes
                        if (lastNavigationGestures.Count() == lastGestureCodes.Capacity)
                        {
                            periodicTableControl.OnNavigationGesture(user.Copy());
                        }

                        var lastZoomGestures = from code in lastGestureCodes.List
                                               where code == (int)GestureDetector.GestureCodes.ZOOM
                                               select code;
                        if (lastZoomGestures.Count() == lastGestureCodes.Capacity)
                        {
                            // TODO add rotation ?
                            double prevDistance = 0.0;
                            double distance = 0.0;
                            bool ascending = true;
                            bool monotone = true;
                            // TODO add thread synchronization for lastGestureCodes
                            foreach (var userCoordinate in lastUserCoordinatesCopy)
                            {
                                distance = Math.Abs(userCoordinate.RightHand.normalizedX() - userCoordinate.LeftHand.normalizedX());
                                if (lastUserCoordinatesCopy.IndexOf(userCoordinate) == 0)
                                {
                                    prevDistance = Math.Abs(lastUserCoordinates.List.ElementAt(1).RightHand.normalizedX() - lastUserCoordinates.List.ElementAt(1).LeftHand.normalizedX());
                                    ascending = prevDistance > distance ? true : false;
                                }
                                else
                                {
                                    if ((ascending && prevDistance > distance) ||
                                        (!ascending && prevDistance < distance))
                                    {
                                        monotone = false;
                                        break;
                                    }
                                }
                                prevDistance = distance;
                            }
                            if (monotone)
                            {
                                double zoomDirection = ascending ? 1.5 : -1.5;
                                periodicTableControl.OnZoomGesture(distance, zoomDirection);
                            }
                        }
                    //});
                }
                // TODO deal with multiple users
                return;
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
