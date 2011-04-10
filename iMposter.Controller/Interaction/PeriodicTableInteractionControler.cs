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


namespace iMposter.Controller.Interaction
{
    public class PeriodicTableInteractionControler : IPeriodicTableInteractionControler
    {
        protected IPeriodicTableControl periodicTableControl;
        protected IFaceDetector faceDetector;
        protected DispatcherTimer collectFacesTimer;
        protected DispatcherTimer processFacesTimer;

        // TODO check if thread synchronization is needed
        protected List<BitmapSource> facesToProcess;
        protected IList<Element> elements;

        protected int columnNumber = 18;
        protected int rowNumber = 7;

        protected BodyTracker bodyTracker;

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

            //bodyTracker = new BodyTracker();
        }
        #endregion

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
            element.IsOverridden = true;
        }

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
