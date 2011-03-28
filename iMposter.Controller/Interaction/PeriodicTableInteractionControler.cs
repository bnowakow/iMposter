using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using iMposter.Controller.Face;
using System.Windows.Threading;
using System.Drawing;
using iMposter.Model.PeriodicTable;


namespace iMposter.Controller.Interaction
{
    public class PeriodicTableInteractionControler : IPeriodicTableInteractionControler
    {
        protected IPeriodicTableControl periodicTableControl;
        protected IFaceDetector faceDetector;
        protected DispatcherTimer dt;

        protected List<Element> elements;

        protected int columnNumber = 18;
        protected int rowNumber = 7;

        public PeriodicTableInteractionControler(IPeriodicTableControl periodicTableControl)
        {
            this.periodicTableControl = periodicTableControl;
            this.faceDetector = new FaceDetector();
            InitializePeriodicTableElementsExistance();

            this.periodicTableControl.InitializePeriodicTableElements(elements);

            dt = new DispatcherTimer();
            dt.Tick += new EventHandler(CollectFacesFromCapture);
            dt.Interval = TimeSpan.FromMilliseconds(1000);
            dt.Start();
        }

        protected void CollectFacesFromCapture(object sender, EventArgs e)
        {
            var faces = faceDetector.DetectFaces();
            foreach (var face in faces)
            {
                Element randomElement = GetRandomNotOverriddenElementElement();
                if (randomElement != null)
                {
                    // TODO animate transition
                    // TODO set callback and timeout for renewal of oryginal image
                    randomElement.Image.Source = face;
                }
            }
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
                        elements.Add(element);
                    }
                }
            }
        }
        #endregion
    }
}
