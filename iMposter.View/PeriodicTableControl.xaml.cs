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
using System.Windows.Media.Media3D;
using iMposter.Model.Camera;
using System.Windows.Threading;
using iMposter.Controller.Face;

namespace iMposter.View
{
    /// <summary>
    /// Interaction logic for PeriodicTableControl.xaml
    /// </summary>
    public partial class PeriodicTableControl : UserControl
    {
        public PeriodicTableControl()
        {
            InitializeComponent();

            this.MouseMove += new MouseEventHandler(PeriodicTableControl_MouseMove);
            this.MouseWheel += new MouseWheelEventHandler(PeriodicTableControl_MouseWheel);
        }

        void PeriodicTableControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            cameraTransformRotation.Angle += e.Delta / 10;
            if (cameraTransformRotation.Angle < -45)
                cameraTransformRotation.Angle = -45;
            if (cameraTransformRotation.Angle > 45)
                cameraTransformRotation.Angle = 45;
        }

        void PeriodicTableControl_MouseMove(object sender, MouseEventArgs e)
        {
            camera.Position = new Point3D(10.0 - (e.GetPosition(this).X / this.ActualWidth * 10.0), e.GetPosition(this).Y / this.ActualHeight * 10.0, camera.Position.Z);
        }
    }
}
