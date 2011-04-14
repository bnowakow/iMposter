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

        /*
        // TODO optimize ;x
        public static bool EqualsByValues(this NuiUser userA, NuiUser userB)
        {
            if (userA.LeftHand.X == userB.LeftHand.X &&
                userA.LeftHand.Y == userB.LeftHand.Y &&
                userA.LeftHand.Z == userB.LeftHand.Z)
            {
                return true;
            }
            return false;   
        }
         */
    }
}