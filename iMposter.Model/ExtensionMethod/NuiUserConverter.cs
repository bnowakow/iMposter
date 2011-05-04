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
    public static class NuiUserConverter
    {
        public static NuiUser Copy(this NuiUser user)
        {
            return new NuiUser() {
                LeftElbow = user.LeftElbow.Copy(),
                LeftShoulder = user.LeftShoulder.Copy(),
                LeftHand = user.LeftHand.Copy(),
                LeftHip = user.LeftHip.Copy(),
                RightElbow = user.RightElbow.Copy(),
                RightShoulder = user.RightShoulder.Copy(),
                RightHand = user.RightHand.Copy(),
                RightHip = user.RightHip.Copy(),
            };
        }

        public static int GetZone(this NuiUser user)
        {
            if (user.LeftHip.normalizedZ() > 0.85)
            {
                return 2;
            }
            if (user.LeftHip.normalizedZ() > 0.1)
            {
                return 1;
            }
            return 0;
        }
    }
}