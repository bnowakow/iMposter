using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nui.Vision;

namespace iMposter.Model.Sensor
{
    public interface IBodyTracker
    {
        void AddNewUserGestureHander(NuiUserTracker.UserUpdatedHandler handler);
    }
}
