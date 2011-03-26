using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using iMposter.Model.Camera;
using System.Windows.Threading;
using iMposter.Model;
using System.Windows.Media.Imaging;

namespace iMposter.View
{
    /// <summary>
    /// Interaction logic for FaceDetectPreview.xaml
    /// </summary>
    public partial class FaceDetectPreview : UserControl
    {
        protected ICameraImage ci;
        protected DispatcherTimer dt;

        public FaceDetectPreview()
        {
            InitializeComponent();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            // Need to add references for Emgu.CV and Emgu.Util in order to get extension method working
            BitmapSource bs = ci.GetNextImage().ToBitmapSource();
            cameraPreview.Source = bs;
        }

        private void captureFaceButton_Click(object sender, RoutedEventArgs e)
        {
            ci = new CameraImage();
            dt = new DispatcherTimer();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = TimeSpan.FromMilliseconds(500);
            dt.Start();
        }
    }
}
