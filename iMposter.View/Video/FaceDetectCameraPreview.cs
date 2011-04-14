using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iMposter.Model.Camera;
using System.Windows.Media.Imaging;
using iMposter.Model;
using Emgu.CV;
using Emgu.CV.Structure;
using iMposter.Controller.Face;
using iMposter.Model.ExtensionMethod;

namespace iMposter.View.Video
{
    public class FaceDetectCameraPreview : VideoPreview
    {
        protected ICameraImage cameraImage;
        protected IFaceDetector faceDetector;

        public FaceDetectCameraPreview()
            : base()
        {
            previewLabel.Content = "FaceDetectCameraPreview";
        }

        public override void InitializeCamera()
        {
            cameraImage = CameraImage.Instance;
            faceDetector = new FaceDetector();
        }

        public override void UpdateCamera()
        {
            Image<Bgr, byte> image = cameraImage.GetNextImage();
            if (image != null)
            {
                foreach (var face in faceDetector.DetectFaces(image))
                {
                    image.Draw(face.rect, new Bgr(0, 0, double.MaxValue), 3);
                }
            }
            BitmapSource bs = image.ToBitmapSource();
            if (bs != null)
            {
                previewImage.Source = bs;
            }
        }
    }
}
