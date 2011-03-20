using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows;

namespace iMposter.Model.Camera
{
    public class CameraImage : ICameraImage
    {
        protected Capture capture;

        public CameraImage()
        {
            capture = new Capture(ModelSettings.Default.cameraIndex);
            // Initialize capture stream to not receive an empty frame
            //System.Threading.Thread.Sleep(1000);
        }

        public Image<Bgr, byte> GetNextImage()
        {
            Image<Bgr, byte> image;
            image = capture.QueryFrame();
            return image;
        }
    }
}
