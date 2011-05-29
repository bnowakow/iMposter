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

namespace iMposter.View.SamplePeriodicTableSubsitute
{
    /// <summary>
    /// Interaction logic for Ball.xaml
    /// </summary>
    public partial class Ball : UserControl
    {
        public Ball()
        {
            InitializeComponent();
        }

        public void AnimateBall()
        {
            ball.Dispatcher.BeginInvoke((Action)delegate
            {
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    //By = 0.2,
                    AutoReverse = true,
                    RepeatBehavior = new RepeatBehavior(1.0),
                    Duration = TimeSpan.FromSeconds(1.0)
                };
                ball.BeginAnimation(OpacityProperty, animation);
            });
        }
    }
}
