using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace iMposter.Controller.Face
{
    public interface IFaceDetector
    {
        List<BitmapSource> DetectFaces();
    }
}
