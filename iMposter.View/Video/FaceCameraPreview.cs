using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iMposter.Model.Camera;
using System.Windows.Media.Imaging;
using iMposter.Model;
using Emgu.CV;
using Emgu.CV.Structure;

namespace iMposter.View.Video
{
    public class FaceCameraPreview : VideoPreview
    {
        protected ICameraImage cameraImage;

        public FaceCameraPreview()
            : base()
        {
            previewLabel.Content = "FaceCameraPreview";
        }

        public override void InitializeCamera()
        {
            cameraImage = CameraImage.Instance;
        }

        public override void UpdateCamera()
        {
            Image<Bgr, byte> image = cameraImage.GetNextImage();
            if (image != null)
            {
                BitmapSource bs = image.ToBitmapSource();
                previewImage.Source = bs;
            }
        }
    }
}
