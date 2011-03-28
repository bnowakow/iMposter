using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using iMposter.Controller.Face;
using System.Windows.Threading;
using System.Drawing;


namespace iMposter.Controller.Interaction
{
    public class PeriodicTableInteractionControler : IPeriodicTableInteractionControler
    {
        protected IPeriodicTableControl periodicTableControl;
        protected IFaceDetector faceDetector;
        protected bool[,] elementImageOverridden;
        protected DispatcherTimer dt;

        protected bool[,] elementExists;
        protected List<Point> existingElements;
        protected int columnNumber = 18;
        protected int rowNumber = 7;
        protected int elementWidth = 165;

        public PeriodicTableInteractionControler(IPeriodicTableControl periodicTableControl)
        {
            this.periodicTableControl = periodicTableControl;
            this.faceDetector = new FaceDetector();
            InitializePeriodicTableElementsExistance();
            InitializeElementImageOverridden();
            
            this.periodicTableControl.InitializePeriodicTableElements(elementExists, rowNumber, columnNumber);

            dt = new DispatcherTimer();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = TimeSpan.FromMilliseconds(1000);
            dt.Start();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            this.CollectFacesFromCapture();
        }

        protected void CollectFacesFromCapture()
        {
            var faces = faceDetector.DetectFaces();
            foreach (var face in faces) 
            {
                // TODO catch exception
                Point randomElementPoint = GetRandomExistingElementCoordinates();
                // TODO animate transition
                // TODO set callback and timeout for renewal of oryginal image
                periodicTableControl.GetElement(randomElementPoint.X, randomElementPoint.Y).Source = face;                
            }
            //speriodicTableControl.GetElement(5, 5).Source = new BitmapImage(new Uri(@"Sample\strong-hamster-small.jpg", UriKind.Relative));
        }

        protected void InitializeElementImageOverridden()
        {
            this.elementImageOverridden = new bool[rowNumber, columnNumber];
        }

        protected Point GetRandomExistingElementCoordinates()
        {
            // select by Linq elements which are not currently animated
            int maxTries = 2 * existingElements.Count();
            for (int i = 0; i < maxTries; i++)
            {
                Point randomElementPoint = GetRandomElementCoordinates();
                if (elementExists[randomElementPoint.X, randomElementPoint.Y])
                {
                    return randomElementPoint;
                }
            }
            throw new Exception("Could not find random existing element in " + maxTries + " tries.");
        }

        protected Point GetRandomElementCoordinates()
        {
            Random random = new Random();
            int randomElementIndex = random.Next(0, existingElements.Count());
            return existingElements[randomElementIndex];
        }

        #region InitializePeriodicTableElementsExistance
        protected void InitializePeriodicTableElementsExistance()
        {
            elementExists = new bool[rowNumber, columnNumber];
            existingElements = new List<Point>();

            for (int row = 0; row < rowNumber; row++)
            {
                for (int column = 0; column < columnNumber; column++)
                {
                    if ((row == 0 && column > 0 && column < 17) || (row < 3 && column > 1 && column < 12))
                    {
                        elementExists[row, column] = false;
                    }
                    else
                    {
                        elementExists[row, column] = true;
                        existingElements.Add(new Point(row, column));
                    }
                }
            }
        }
        #endregion
    }
}
