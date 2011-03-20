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
using iMposter.Controller.Face;

namespace iMposter.View.Sample
{
    /// <summary>
    /// Interaction logic for CameraFaceDetect.xaml
    /// </summary>
    public partial class CameraFaceDetect : Page
    {
        protected IFaceDetector faceDetector;

        public CameraFaceDetect()
        {
            InitializeComponent();

            faceDetector = new FaceDetector();
        }

        private void detectButton_Click(object sender, RoutedEventArgs e)
        {
            var faces = faceDetector.DetectFaces();
            if (faces.Count > 0)
            {
                imageViewer.Source = faces.First();
            }
        }
    }
}
