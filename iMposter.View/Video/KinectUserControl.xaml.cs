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
using System.Windows.Media.Animation;

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
        protected bool hideControl = true;

        public KinectUserControl()
        {
            InitializeComponent();
            bodyTracker = BodyTracker.Instance;
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            bodyTracker.AddNewUserGestureHander(
                new Nui.Vision.NuiUserTracker.UserUpdatedHandler(Tracker_UserUpdated));
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        }

        protected void fade(UIElement element, double from, double to)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(5.0),

            };
            element.BeginAnimation(OpacityProperty, animation);
        }

        void Tracker_UserUpdated(object sender, Nui.Vision.NuiUserEventArgs e)
        {
            kinectImageGrid.Dispatcher.BeginInvoke((Action)delegate
            {
                if (hideControl && kinectImageGrid.Opacity == 0.4)
                {
                    fade(kinectImageGrid, 0.4, 0.0);
                }
                if (!hideControl && kinectImageGrid.Opacity == 0.0)
                {
                    fade(kinectImageGrid, 0.0, 0.4);
                }
            });
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
                if (bodyTracker.Tracker != null)
                {
                    kinectImage.Source = bodyTracker.Tracker.DepthImage;
                    if (kinectImageCanvas.Width != kinectImage.ActualWidth ||
                        kinectImageCanvas.Height != kinectImage.ActualHeight)
                    {
                        kinectImageCanvas.Width = kinectImage.ActualWidth;
                        kinectImageCanvas.Height = kinectImage.ActualHeight;
                    }
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

        private void kinectHideButton_Click(object sender, RoutedEventArgs e)
        {
            hideControl = !hideControl;
        }

        private void kinectHideButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var button = sender as Button;
            if (button.Opacity == 0.0)
            {
                fade(button, 0.0, 1.0);
            }
        }

        private void kinectHideButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var button = sender as Button;
            if (button.Opacity == 1.0)
            {
                fade(button, 1.0, 0.0);
            }
        }
    }
}
