using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iMposter.Model.Camera;
using System.Windows.Media.Imaging;
using iMposter.Model;
using iMposter.Model.Sensor;

namespace iMposter.View.Video
{
    public class KinectDepthCameraPreview : VideoPreview
    {
        protected BodyTracker bodyTracker;

        public KinectDepthCameraPreview()
            : base()
        {
            previewLabel.Content = "KinectDepthCameraPreview";
        }

        public override void InitializeCamera()
        {
            bodyTracker = BodyTracker.Instance;
        }

        public override void UpdateCamera()
        {
            previewImage.Source = bodyTracker.Tracker.DepthImage;
        }
    }
}
