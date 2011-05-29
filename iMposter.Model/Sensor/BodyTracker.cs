using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nui.Vision;
using System.Windows.Media;
using System.Windows.Forms;

namespace iMposter.Model.Sensor
{
    public sealed class BodyTracker : IBodyTracker
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
                // Stworzenie obiektu śledzącego użytkownika
                Tracker = new NuiUserTracker(@ModelSettings.Default.sensorTrackerConfig);
            }
            catch (Exception e)
            {
                if (e.Message == "Configuration file not found.")
                {
                    // System nie posiada podpiętego sensora odległości
                    MessageBox.Show("There is no depth sensor in the system");
                }
            }
        }

        public void AddNewUserGestureHander(NuiUserTracker.UserUpdatedHandler handler)
        {
            if (Tracker != null)
            {
                Tracker.UserUpdated += handler;
            }
        }
    }
}
