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
using System.Windows.Shapes;
using iMposter.Controller.Interaction;

namespace iMposter.View
{
    /// <summary>
    /// Interaction logic for Wall.xaml
    /// </summary>
    public partial class Wall : Window
    {
        protected IPeriodicTableInteractionControler periodicTableInteractionControler;

        public Wall()
        {
            InitializeComponent();
            periodicTableInteractionControler = new PeriodicTableInteractionControler(periodicTableControl);
        }
    }
}
