using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMposter.Controller.Interaction
{
    public interface IPeriodicTableInteractionControler
    {
        void CollectFacesFromCapture(object sender, EventArgs e);
        void ProcessFacesFromCapture(object sender, EventArgs e);
        void ColleUserGesture(double[][] gesturePath);
        void ProcessUserGesture(object sender, Nui.Vision.NuiUserEventArgs e);
    }
}
