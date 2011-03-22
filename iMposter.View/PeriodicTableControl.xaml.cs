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
using System.IO;

namespace iMposter.View
{
    /// <summary>
    /// Interaction logic for PeriodicTableControl.xaml
    /// </summary>
    public partial class PeriodicTableControl : UserControl
    {
        protected bool[,] elementExists;
        protected int columnNumber = 18;
        protected int rowNumber = 7;
        protected int elementWidth = 165;

        public PeriodicTableControl()
        {
            InitializeComponent();
            InitializePeriodicTableElementsExistance();
            InitializePeriodicTableElements();

            //this.MouseMove += new MouseEventHandler(PeriodicTableControl_MouseMove);
            //this.MouseWheel += new MouseWheelEventHandler(PeriodicTableControl_MouseWheel);
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

        protected void InitializePeriodicTableElements() {
            for (int row = 0; row < rowNumber; row++) 
            {
                for (int column = 0; column < columnNumber; column++)
                {
                    if (elementExists[row, column])
                    {
                        TranslateTransform3D translate = new TranslateTransform3D(column * 2.5, row * -2.5, 0);
                        Transform3DGroup tranformGroup = new Transform3DGroup();
                        tranformGroup.Children.Add(translate);

                        MeshGeometry3D mesh = new MeshGeometry3D();
                        mesh.Positions = new Point3DCollection(new Point3D[] { new Point3D(-1, 1, 0), new Point3D(-1, -1, 0), new Point3D(1, -1, 0), new Point3D(1, 1, 0) });
                        mesh.TextureCoordinates = new PointCollection(new Point[] { new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 0) });
                        mesh.TriangleIndices = new Int32Collection(new Int32[] { 0, 1, 2, 0, 2, 3 });

                        DiffuseMaterial material = new DiffuseMaterial(Brushes.Gray);
                        material.SetValue(Viewport2DVisual3D.IsVisualHostMaterialProperty, true);

                        Image image = new Image();
                        image.Width = elementWidth;
                        image.Source = new BitmapImage(new Uri(@"Img\Elements\01_01.jpg", UriKind.Relative));

                        Viewport2DVisual3D viewport = new Viewport2DVisual3D();
                        viewport.Transform = tranformGroup;
                        viewport.Geometry = mesh;
                        viewport.Material = material;
                        viewport.Visual = image;

                        this.mainViewport.Children.Add(viewport);
                    }
                }
            }
        }

        #region InitializePeriodicTableElementsExistance
        private void InitializePeriodicTableElementsExistance()
        {
            elementExists = new bool[rowNumber, columnNumber];
            for (int row = 0; row < rowNumber; row++) 
            {
                for (int column = 0; column < columnNumber; column++)
                {
                    if ((row == 0 && column > 0 && column < 17) || (row < 3 && column > 1 && column < 12))
                    {
                        elementExists[row, column] = false;
                    }
                    else
                    {
                        elementExists[row, column] = true;
                    }
                }
            }
        }
        #endregion
    }
}
