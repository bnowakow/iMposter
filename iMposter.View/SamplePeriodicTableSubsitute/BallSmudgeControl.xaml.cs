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
using iMposter.Controller.Interaction;
using iMposter.Model.ExtensionMethod;

namespace iMposter.View.SamplePeriodicTableSubsitute
{
    /// <summary>
    /// Interaction logic for BallSmudgeControl.xaml
    /// </summary>
    public partial class BallSmudgeControl : UserControl, IPeriodicTableControl
    {
        RowDefinition row1;
        ColumnDefinition col1;
        int liczba_kolumn = 42;
        int liczba_wierszy = 14;
        protected Ball[][] balls;


        public BallSmudgeControl()
        {
            InitializeComponent();
        }

        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            if (balls == null)
            {
                liczba_kolumn = (int)Math.Floor(Width / 20);
                liczba_wierszy = (int)Math.Floor(Height / 20);

                //ustawienie szer i wys kolumn i wierszy grida

                GridLengthConverter myGridLengthConverter = new GridLengthConverter();
                double rozmiar = 100 / (double)liczba_kolumn;

                LengthConverter lc = new LengthConverter();
                string qualifiedDouble = "20px";
                double converted = (double)lc.ConvertFromString(qualifiedDouble);



                for (int col = 0; col < liczba_kolumn; col++)
                {
                    col1 = new ColumnDefinition();
                    col1.Width = new GridLength(converted, GridUnitType.Pixel);
                    Tlo.ColumnDefinitions.Add(col1);
                }
                Tlo.UpdateLayout();

                for (int row = 0; row < liczba_wierszy; row++)
                {
                    row1 = new RowDefinition();
                    row1.Height = new GridLength(converted, GridUnitType.Pixel);

                    Tlo.RowDefinitions.Add(row1);
                }

                balls = new Ball[liczba_kolumn][];
                for (int col = 0; col < liczba_kolumn; col++)
                {
                    balls[col] = new Ball[liczba_wierszy];
                    for (int row = 0; row < liczba_wierszy; row++)
                    {
                        Ball kolo = new Ball();
                        kolo.Name = "kolko" + col.ToString() + "x" + row.ToString();
                        Tlo.Children.Add(kolo);
                        Grid.SetColumn(kolo, col);
                        Grid.SetRow(kolo, row);
                        balls[col][row] = kolo;
                    }

                }
            }
        }

        public void InitializePeriodicTableElements(IList<Model.PeriodicTable.Element> elements)
        {
        }

        public void FadeElementImage(Model.PeriodicTable.Element element, double from, double to)
        {
            
        }

        public int GetFadeTimeMiliseconds()
        {
            return 1;
        }

        public void OnNavigationGesture(Nui.Vision.NuiUser user)
        {
            int row = (int)Math.Floor(user.RightHand.normalizedX() * liczba_kolumn);
            int col = (int)Math.Floor(user.RightHand.normalizedY() * liczba_wierszy);
            if (row < 0)
                row = 0;
            if (col < 0)
                col = 0;
            if (row >= liczba_kolumn)
                row = liczba_kolumn - 1;
            if (col >= liczba_wierszy)
                col = liczba_wierszy - 1;
            balls[row][col].AnimateBall();
        }

        public void OnZoomGesture(double distance, double zoomDirection)
        {

        }
    }

}
