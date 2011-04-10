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
    public class KinectCameraPreview : VideoPreview
    {
        protected BodyTracker bodyTracker;

        public KinectCameraPreview()
            : base()
        {
            previewLabel.Content = "KinectCameraPreview";
        }

        public override void InitializeCamera()
        {
            bodyTracker = new BodyTracker();
        }

        public override void UpdateCamera()
        {
            previewImage.Source = bodyTracker.Tracker.RawImage;
        }
    }
}
