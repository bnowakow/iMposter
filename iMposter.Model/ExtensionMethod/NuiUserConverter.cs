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
        /*
        public static IList<NuiUserBodyPart> gestureSingleSubsequence(this NuiUser user)
        {
            IList<NuiUserBodyPart> subsequence = new List<NuiUserBodyPart> { 
                //new NuiUserBodyPart() { X = user.LeftElbow.X, Y = user.LeftElbow.Y, Z = user.LeftElbow.Z},
                new NuiUserBodyPart() { X = user.LeftHand.X, Y = user.LeftHand.Y, Z = user.LeftHand.Z},
                new NuiUserBodyPart() { X = user.LeftHip.X, Y = user.LeftHip.Y, Z = user.LeftHip.Z},
                //new NuiUserBodyPart() { X = user.RightElbow.X, Y = user.RightElbow.Y, Z = user.RightElbow.Z},
                new NuiUserBodyPart() { X = user.RightHand.X, Y = user.RightHand.Y, Z = user.RightHand.Z},
                new NuiUserBodyPart() { X = user.RightHip.X, Y = user.RightHip.Y, Z = user.RightHip.Z},
                };
            return subsequence;
        }
         */

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