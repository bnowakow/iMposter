using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using iMposter.Controller.Face;

namespace iMposter.Controller.Interaction
{
    public class PeriodicTableInteractionControler : IPeriodicTableInteractionControler
    {
        protected IPeriodicTableControl periodicTableControl;
        protected IFaceDetector faceDetector;

        public PeriodicTableInteractionControler(IPeriodicTableControl periodicTableControl)
        {
            this.periodicTableControl = periodicTableControl;
            this.faceDetector = new FaceDetector();
            this.Omg();
        }

        private void Omg()
        {
            for (int i = 0; i < 5; i++)
            {
                var faces = faceDetector.DetectFaces();
                if (faces.Count > 0)
                {
                    periodicTableControl.GetElement(5, 5).Source = faces.First();
                }
            }
            //periodicTableControl.GetElement(5, 5).Source = new BitmapImage(new Uri(@"Sample\strong-hamster-small.jpg", UriKind.Relative));
        }
    }
}
