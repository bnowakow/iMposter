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
using iMposter.Model.ExtensionMethod;

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
            bodyTracker.AddNewUserGestureHander(
                new Nui.Vision.NuiUserTracker.UserUpdatedHandler(Tracker_UserUpdated));
        }

        protected NuiUserBodyPart tmpMaxDepthValues = new NuiUserBodyPart { X = -1, Y = -1, Z = -1 };
        void Tracker_UserUpdated(object sender, Nui.Vision.NuiUserEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                canvasRoot.Children.Clear();
                foreach (var user in e.Users)
                {
                    NuiUserBodyPart[] points = new NuiUserBodyPart[] { user.LeftElbow, user.LeftHand, user.LeftHip, user.RightElbow, user.RightHand, user.RightHip };
                    foreach (var point in points)
                    {
                        if (point.X > tmpMaxDepthValues.X) tmpMaxDepthValues.X = point.X;
                        if (point.Y > tmpMaxDepthValues.Y) tmpMaxDepthValues.Y = point.Y;
                        if (point.Z > tmpMaxDepthValues.Z) tmpMaxDepthValues.Z = point.Z;
                        Ellipse ellipse = new Ellipse
                        {
                            Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),
                            Width = 10,
                            Height = 10,
                            Margin = new Thickness(point.normalizedX() * canvasRoot.ActualWidth, point.normalizedY() * canvasRoot.ActualHeight, 0, 0)
                        };

                        canvasRoot.Children.Add(ellipse);
                    }
                }
            });
        }

        public override void UpdateCamera()
        {
            if (bodyTracker.Tracker != null)
            {
                previewImage.Source = bodyTracker.Tracker.RawImage;
            }
        }
    }
}
