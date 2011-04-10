using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nui.Vision;
using System.Windows.Media;
using System.Windows.Forms;

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
            try
            {
                Tracker = new NuiUserTracker(@ModelSettings.Default.sensorTrackerConfig);
            }
            catch (Exception e)
            {
                if (e.Message == "Configuration file not found.")
                {
                    // There is no Kinect in the system
                    MessageBox.Show("There is no sensor in the system, using fakeKinect instead");
                    // TODO use fakeKinect instead
                }
            }
        }
    }
}
