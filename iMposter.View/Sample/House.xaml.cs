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
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace iMposter.View.Sample
{
    /// <summary>
    /// Interaction logic for House.xaml
    /// </summary>
    public partial class House : Page
    {
        public House()
        {
            InitializeComponent();

            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0;
            doubleAnimation.To = 360;
            doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:5"));
            doubleAnimation.RepeatBehavior = new RepeatBehavior(2);
            cameraTransformRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimation);

        }
    }
}
