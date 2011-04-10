using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nui.Vision;
using System.Windows.Media;

namespace iMposter.Model.Sensor
{
    public class BodyTracker
    {
        public NuiUserTracker Tracker { get; set; }

        public BodyTracker() {
            // TODO catch exception
            string text = System.IO.File.ReadAllText(@ModelSettings.Default.sensorTrackerConfig);
            Tracker = new NuiUserTracker(@ModelSettings.Default.sensorTrackerConfig);
            Tracker.UserUpdated += new NuiUserTracker.UserUpdatedHandler(tracker_UserUpdated);
        }

        void tracker_UserUpdated(object sender, NuiUserEventArgs e)
        {
            foreach (var user in e.Users)
            {
                float leftElbowX = user.LeftElbow.X;
                float leftElbowY = user.LeftElbow.Y;

                float leftHandX = user.LeftHand.X;
                float leftHandY = user.LeftHand.Y;

                float rightElbowX = user.RightElbow.X;
                float rightElbowY = user.RightElbow.Y;

                float rightHandX = user.RightHand.X;
                float rightHandY = user.RightHand.Y;
            }
        }
    }
}
