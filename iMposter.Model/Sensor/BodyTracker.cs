using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nui.Vision;
using System.Windows.Media;

namespace iMposter.Model.Sensor
{
    public sealed class BodyTracker
    {
        static BodyTracker instance = null;
        static readonly object padlock = new object();

        public NuiUserTracker Tracker { get; set; }

        public static BodyTracker Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new BodyTracker();
                    }
                    return instance;
                }
            }
        }

        private BodyTracker() {
            // TODO catch exception
            Tracker = new NuiUserTracker(@ModelSettings.Default.sensorTrackerConfig);
            Tracker.UserUpdated += new NuiUserTracker.UserUpdatedHandler(tracker_UserUpdated);
        }

        private void tracker_UserUpdated(object sender, NuiUserEventArgs e)
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
