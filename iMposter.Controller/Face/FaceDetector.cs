using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iMposter.Model.Camera;
using iMposter.Model;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using System.Drawing;
using Emgu.CV.CvEnum;
using System.Windows;

namespace iMposter.Controller.Face
{
    public class FaceDetector : IFaceDetector
    {
        protected ICameraImage cameraImage;
        protected HaarCascade haarCascade;
        protected double haarScaleFactor = ControllerSettings.Default.haarScaleFactor;
        protected int haarMinNeighbours = ControllerSettings.Default.haarMinNeighbours;
        protected int haarMinFaceImageDivider = ControllerSettings.Default.haarMinFaceImageDivider;

        public FaceDetector()
        {
            cameraImage = CameraImage.Instance;
            haarCascade = new HaarCascade(@ControllerSettings.Default.haarCascadeFile);
        }

        public List<BitmapSource> DetectFaces()
        {
            List<BitmapSource> faceBitmaps = new List<BitmapSource>();
            // supach tmp
            //return faceBitmaps;
            Image<Bgr, byte> image = cameraImage.GetNextImage();
            if (image != null)
            {
                // TODO detect faces that lays on each other
                // TODO hash faces - SURF feature detector in CSharp - http://www.emgu.com/wiki/index.php/SURF_feature_detector_in_CSharp
                // TODO detect face sharpeness to enlarge face region
                foreach (var face in DetectFaces(image))
                {
                    CroppedBitmap faceBitmap = new CroppedBitmap(image.ToBitmapSource(), face.rect.ToInt32Rect());
                    faceBitmaps.Add(faceBitmap);
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return faceBitmaps;
        }

        public MCvAvgComp[] DetectFaces(Image<Bgr, byte> image)
        {
            Image<Gray, byte> grayImage = image.Convert<Gray, byte>();
            System.Drawing.Size faceMinSize = new System.Drawing.Size(image.Width / haarMinFaceImageDivider, image.Height / haarMinFaceImageDivider);
            var faces = grayImage.DetectHaarCascade(haarCascade, haarScaleFactor, haarMinNeighbours, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, faceMinSize)[0];

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return faces;
        }
    }
}
