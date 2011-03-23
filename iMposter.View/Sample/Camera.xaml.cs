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
using iMposter.Model;

namespace iMposter.View.Sample
{
    /// <summary>
    /// Interaction logic for Camera.xaml
    /// </summary>
    public partial class Camera : Page
    {
        protected ICameraImage ci;

        public Camera()
        {
            InitializeComponent();

            //System.Windows.Forms.Application.Idle
            //System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, null);

            ci = new CameraImage();            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Need to add references for Emgu.CV and Emgu.Util in order to get extension method working
            BitmapSource bs = ci.GetNextImage().ToBitmapSource();
            imageViewer.Source = bs;
        }
    }
}
