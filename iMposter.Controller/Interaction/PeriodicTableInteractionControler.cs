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


namespace iMposter.Controller.Interaction
{
    public class PeriodicTableInteractionControler : IPeriodicTableInteractionControler
    {
        protected IPeriodicTableControl periodicTableControl;
        protected IFaceDetector faceDetector;
        protected DispatcherTimer collectFacesTimer;

        protected List<Element> elements;

        protected int columnNumber = 18;
        protected int rowNumber = 7;

        public PeriodicTableInteractionControler(IPeriodicTableControl periodicTableControl)
        {
            this.periodicTableControl = periodicTableControl;
            this.faceDetector = new FaceDetector();
            InitializePeriodicTableElementsExistance();

            this.periodicTableControl.InitializePeriodicTableElements(elements);

            collectFacesTimer = new DispatcherTimer();
            collectFacesTimer.Tick += new EventHandler(CollectFacesFromCapture);
            collectFacesTimer.Interval = TimeSpan.FromMilliseconds(5000);
            collectFacesTimer.Start();
        }

        protected void CollectFacesFromCapture(object sender, EventArgs e)
        {
            var faces = faceDetector.DetectFaces();
            int timeOffset = 0;
            foreach (var face in faces)
            {
                Element randomElement = GetRandomNotOverriddenElementElement();
                if (randomElement != null)
                {
                    // TODO animate transition - http://blogs.windowsclient.net/swt62/archive/2009/12/10/fade-out-status-bar-in-wpf.aspx
                    randomElement.NewImageSource = face;
                    randomElement.IsOverridden = true;

                    DispatcherTimer elementTimer = new DispatcherTimer();
                    elementTimer.Interval = TimeSpan.FromMilliseconds(++timeOffset * 1000);
                    elementTimer.Tag = randomElement;
                    elementTimer.Tick += new EventHandler(ElementOverrideByNewCallback);
                    elementTimer.Start();
                }
            }
        }

        protected void ElementOverrideByNewCallback(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            Element element = (Element)(sender as DispatcherTimer).Tag;
            if (element.NewImageSource != null)
            {
                element.FadeOutElementImage();
                element.SetNewImage();
                element.NewImageSource = null;

                DispatcherTimer elementTimer = new DispatcherTimer();
                elementTimer.Interval = TimeSpan.FromMilliseconds(5000);
                elementTimer.Tag = element;
                elementTimer.Tick += new EventHandler(ElementOverrideByDefaultCallback);
                elementTimer.Start();
            }
        }

        protected void ElementOverrideByDefaultCallback(object sender, EventArgs e)
        {
            Element element = (Element)(sender as DispatcherTimer).Tag;
            element.RevertDefaultImage();
        }

        protected Element GetRandomNotOverriddenElementElement()
        {
            // elements which are not currently animated
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
