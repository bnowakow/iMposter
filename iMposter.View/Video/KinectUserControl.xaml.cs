using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using iMposter.Model.Sensor;
using Nui.Vision;
using iMposter.Model.ExtensionMethod;

namespace iMposter.View.Video
{
    /// <summary>
    /// Interaction logic for KinectUserControl.xaml
    /// </summary>
    public partial class KinectUserControl : UserControl
    {
        protected ImageSource imageSource { get; set; }
        protected BackgroundWorker worker;
        protected BodyTracker bodyTracker;
        protected Color darkBlue = Color.FromArgb(200, 22, 44, 65);

        public KinectUserControl()
        {
            InitializeComponent();
            bodyTracker = BodyTracker.Instance;
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            bodyTracker.Tracker.UserUpdated += new Nui.Vision.NuiUserTracker.UserUpdatedHandler(Tracker_UserUpdated);
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        }

        void Tracker_UserUpdated(object sender, Nui.Vision.NuiUserEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                kinectImageCanvas.Children.Clear();
                foreach (var user in e.Users)
                {
                    NuiUserBodyPart[] points = new NuiUserBodyPart[] { user.LeftElbow, user.LeftHand, user.LeftShoulder, user.RightElbow, user.RightHand, user.RightShoulder };
                    foreach (var point in points)
                    {
                        Ellipse ellipse = new Ellipse
                        {
                            Fill = new SolidColorBrush(darkBlue),
                            Width = 15,
                            Height = 15,
                            Margin = new Thickness(point.normalizedX() * kinectImageCanvas.ActualWidth, point.normalizedY() * kinectImageCanvas.ActualHeight, 0, 0)
                        };

                        kinectImageCanvas.Children.Add(ellipse);
                    }
                    TextBlock text = new TextBlock()
                    {
                        Text = user.Id.ToString(),
                        Foreground = new SolidColorBrush(darkBlue),
                        Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                        FontSize = 34,
                        Margin = new Thickness(user.Head.normalizedX() * kinectImageCanvas.ActualWidth, user.Head.normalizedY() * kinectImageCanvas.ActualHeight, 0, 0)
                    };
                    kinectImageCanvas.Children.Add(text);
                }
            });
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                kinectImage.Source = bodyTracker.Tracker.DepthImage;
                if (kinectImageCanvas.Width != kinectImage.ActualWidth || 
                    kinectImageCanvas.Height != kinectImage.ActualHeight)
                {
                    kinectImageCanvas.Width = kinectImage.ActualWidth;
                    kinectImageCanvas.Height = kinectImage.ActualHeight;
                }
            });
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }
    }
}
