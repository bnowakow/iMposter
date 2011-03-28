using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;

namespace iMposter.Model.Camera
{
    public class CameraImage : ICameraImage
    {
        protected Capture capture;
        protected bool useFakeCamera = ModelSettings.Default.useFakeCamera;

        public CameraImage()
        {
            try
            {
                capture = new Capture(ModelSettings.Default.cameraIndex);
            }
            catch (Exception e)
            {
                if (e.Message == "Error: Unable to create capture from camera" + ModelSettings.Default.cameraIndex)
                {
                    // There is no camera in the system
                    useFakeCamera = true;
                }
            }
            // Initialize capture stream to not receive an empty frame
            //System.Threading.Thread.Sleep(1000);
        }

        public Image<Bgr, byte> GetNextImage()
        {
            Image<Bgr, byte> image;
            if (!useFakeCamera)
            {
                image = capture.QueryFrame();
            }
            else
            {
                // Use random image from FakeCamera folder each time
                DirectoryInfo fakeCameraCaptureDirectory = new DirectoryInfo(@"Camera\FakeCamera\");
                FileInfo[] fakeCameraCaptureFiles = fakeCameraCaptureDirectory.GetFiles("FakeCameraCapture_*.jpg");
                Random random = new Random();
                int randomFileIndex = random.Next(0, fakeCameraCaptureFiles.Count());
                image = new Image<Bgr, byte>(@"Camera\FakeCamera\" + fakeCameraCaptureFiles[randomFileIndex]);
            }
            return image;
        }
    }
}
