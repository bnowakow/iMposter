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
using iMposter.Model.Camera;
using System.Windows.Threading;
using iMposter.Model;

namespace iMposter.View
{
    /// <summary>
    /// Interaction logic for CameraPreview.xaml
    /// </summary>
    public partial class CameraPreview : UserControl
    {
        protected ICameraImage ci;

        public CameraPreview()
        {
            InitializeComponent();
            ci = new CameraImage();
            DispatcherTimer dt = new DispatcherTimer();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = TimeSpan.FromMilliseconds(500);
            //dt.Start();
            //dt_Tick(null, null);
        }

        void dt_Tick(object sender, EventArgs e)
        {
            // Need to add references for Emgu.CV and Emgu.Util in order to get extension method working
            BitmapSource bs = ci.GetNextImage().ToBitmapSource();
            cameraPreview.Source = bs;
        }
    }
}
