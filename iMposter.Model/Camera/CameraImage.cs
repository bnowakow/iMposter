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
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using AForge.Controls;

namespace iMposter.Model.Camera
{
    public sealed class CameraImage : ICameraImage
    {
        static CameraImage instance = null;
        static readonly object padlock = new object();

        private Capture openCVCapture;
        private VideoCaptureDevice aForgeCapture;
        private Bitmap aForgeLastFrame;
        private bool useFakeCamera = ModelSettings.Default.useFakeCamera;

        public static CameraImage Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CameraImage();
                    }
                    return instance;
                }

            }
        }

        private CameraImage()
        {
            try
            {
                //openCVCapture = new Capture(ModelSettings.Default.cameraIndex);
                FilterInfoCollection aForgeVideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                aForgeCapture = new VideoCaptureDevice(aForgeVideoDevices[ModelSettings.Default.cameraIndex].MonikerString);
                aForgeCapture.DesiredFrameRate = 10;
                //aForgeCapture.NewFrame += new NewFrameEventHandler(aForgeCapture_NewFrame);
                VideoSourcePlayer videoSource = new VideoSourcePlayer();
                videoSource.NewFrame += new VideoSourcePlayer.NewFrameHandler(videoSource_NewFrame);
                videoSource.VideoSource = aForgeCapture;
                videoSource.Start();
            }
            catch (Exception e)
            {
                if (e.Message == "Error: Unable to create capture from camera" + ModelSettings.Default.cameraIndex)
                {
                    // There is no camera in the system
                    MessageBox.Show("There is no camera in the system, using fakeCamera instead");
                    useFakeCamera = true;
                }
            }
        }

        private void videoSource_NewFrame(object sender, ref Bitmap image)
        {
            aForgeLastFrame = image.Clone() as Bitmap;
        }

        private void aForgeCapture_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            aForgeLastFrame = eventArgs.Frame.Clone() as Bitmap;
        }

        public Image<Bgr, byte> GetNextImage()
        {
            Image<Bgr, byte> image = null;
            if (!useFakeCamera)
            {
                //image = openCVCapture.QueryFrame();
                if (aForgeLastFrame != null) {
                    image = new Image<Bgr, byte>(aForgeLastFrame.Clone() as Bitmap);
                }
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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return image;
        }
    }
}
