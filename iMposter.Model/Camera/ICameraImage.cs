using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace iMposter.Model.Camera
{
    public interface ICameraImage
    {
        Image<Bgr, byte> GetNextImage();
    }
}
