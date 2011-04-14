using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Emgu.CV;
using System.Runtime.InteropServices;
using System.Windows;
using Emgu.CV.Structure;
using System.Drawing;
using Nui.Vision;

namespace iMposter.Model.ExtensionMethod
{
    public static class NuiUserBodyPartConverter
    {
        public static double normalizedX(this NuiUserBodyPart point)
        {
            return point.X / ModelSettings.Default.sensorCameraResolutionWidth;
        }

        public static double normalizedY(this NuiUserBodyPart point)
        {
            return point.Y / ModelSettings.Default.sensorCameraResolutionHeight;
        }

        public static double normalizedZ(this NuiUserBodyPart point)
        {
            return point.Z / ModelSettings.Default.sensorCameraResolutionDepth;
        }

        public static NuiUserBodyPart Copy(this NuiUserBodyPart part)
        {
            return new NuiUserBodyPart()
            {
                X = part.X,
                Y = part.Y,
                Z = part.Z
            };
        }
    }
}