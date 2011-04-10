using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iMposter.Model.Camera;
using System.Windows.Media.Imaging;
using iMposter.Model;
using iMposter.Model.Sensor;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using Nui.Vision;

namespace iMposter.View.Video
{
    public class KinectJointDetectCameraPreview : VideoPreview
    {
        protected BodyTracker bodyTracker;

        public KinectJointDetectCameraPreview()
            : base()
        {
            previewLabel.Content = "KinectJointDetectCameraPreview";
        }

        public override void InitializeCamera()
        {
            bodyTracker = BodyTracker.Instance;
            bodyTracker.Tracker.UserUpdated += new Nui.Vision.NuiUserTracker.UserUpdatedHandler(Tracker_UserUpdated);
        }

        void Tracker_UserUpdated(object sender, Nui.Vision.NuiUserEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                canvasRoot.Children.Clear();
                foreach (var user in e.Users)
                {
                    NuiUserBodyPart[] points = new NuiUserBodyPart[] { user.LeftElbow, user.LeftHand, user.RightElbow, user.RightHand };
                    foreach (var point in points)
                    {
                        Ellipse ellipse = new Ellipse
                        {
                            Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),
                            Width = 10,
                            Height = 10,
                            Margin = new Thickness(point.X / 640.0 * previewImage.ActualWidth, point.Y / 480.0 * previewImage.ActualHeight, 0, 0)
                        };

                        canvasRoot.Children.Add(ellipse);
                    }
                }
            });
        }

        public override void UpdateCamera()
        {
            previewImage.Source = bodyTracker.Tracker.RawImage;
        }
    }
}
