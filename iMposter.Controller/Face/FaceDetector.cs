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

namespace iMposter.Controller.Face
{
    public class FaceDetector
    {
        protected CameraImage cameraImage;
        protected HaarCascade haarCascade;
        protected double haarScaleFactor = 1.4;
        protected int haarMinNeighbours = 4;

        public FaceDetector()
        {
            cameraImage = new CameraImage();
            // TODO read from config which haar cascade file should be used
            haarCascade = new HaarCascade(@"Face\haarcascade_frontalface_alt2.xml");
        }

        public BitmapSource DetectFaces()
        {
            Image<Bgr, byte> image = cameraImage.GetNextImage();
            Image<Gray, byte> grayImage = image.Convert<Gray, byte>();
            // TODO read from config what minimum resolotion of face should it have according to camera resolution and faces distance
            int minFaceImageDivider = 20;
            Size faceMinSize = new Size(image.Width / minFaceImageDivider, image.Height / minFaceImageDivider);
            var faces = grayImage.DetectHaarCascade(haarCascade, haarScaleFactor, haarMinNeighbours, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, faceMinSize)[0];
            // TODO detect faces that lays on each other
            foreach (var face in faces)
            {
                image.Draw(face.rect, new Bgr(0, double.MaxValue, 0), 3);
            }
            return image.ToBitmapSource();
        }
    }
}
