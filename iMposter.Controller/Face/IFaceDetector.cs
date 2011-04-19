using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Emgu.CV.Structure;
using Emgu.CV;

namespace iMposter.Controller.Face
{
    public interface IFaceDetector
    {
        List<BitmapSource> DetectFaces();
        MCvAvgComp[] DetectFacesBitmaps(Image<Bgr, byte> image);
    }
}
