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
using iMposter.Controller.Interaction;
using iMposter.Model.PeriodicTable;
using System.Windows.Media.Animation;

namespace iMposter.View
{
    /// <summary>
    /// Interaction logic for PeriodicTableControl.xaml
    /// </summary>
    public partial class PeriodicTableControl : UserControl, IPeriodicTableControl
    {
        protected int fadeTimeSeconds = ViewSettings.Default.fadeTimeSeconds;
        protected IList<Element> elements;
        protected int elementWidth = 165;

        public PeriodicTableControl()
        {
            InitializeComponent();

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

        private void sampleCameraTransitionButton_Click(object sender, RoutedEventArgs e)
        {
            Vector3DAnimation lookAnimation = new Vector3DAnimation(new Vector3D(-0.4, -0.7, -1), new Vector3D(-0.8, -0.7, -1), new Duration(TimeSpan.FromSeconds(20)));
            camera.BeginAnimation(PerspectiveCamera.LookDirectionProperty, lookAnimation);
        }

        public void InitializePeriodicTableElements(IList<Element> elements)
        {
            this.elements = elements;

            foreach (Element element in this.elements)
            {
                TranslateTransform3D translate = new TranslateTransform3D(element.Location.X * 2.5, element.Location.Y * -2.5, 0);
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
                element.DefaultImageSource = image.Source;
                element.Image = image;

                this.mainViewport.Children.Add(viewport);
            }
        }

        public void FadeElementImage(Element element, double from, double to)
        {
            Storyboard fade = new Storyboard();
            DoubleAnimation fadeOutAnimation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(fadeTimeSeconds)),
            };
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("(Opacity)"));
            Storyboard.SetTarget(fadeOutAnimation, element.Image);
            fade.Children.Add(fadeOutAnimation);
            fade.Begin();
        }

        public int GetFadeTimeMiliseconds()
        {
            return fadeTimeSeconds * 1000;
        }

        public PerspectiveCamera GetCamera()
        {
            return camera;
        }

        public void SetCamerLookDirection(Vector3D lookDirection)
        {
            double minX = -1.0;
            double maxX = 1.0;
            if (lookDirection.X > maxX)
            {
                lookDirection.X = maxX;
            }
            if (lookDirection.X < minX)
            {
                lookDirection.X = minX;
            }
            double minY = -0.2;
            double maxY = 0.2;
            if (lookDirection.Y > maxY)
            {
                lookDirection.Y = maxY;
            }
            if (lookDirection.Y < minY)
            {
                lookDirection.Y = minY;
            }
            camera.LookDirection = lookDirection;
        }

        public void SetCameraPosition(Point3D position)
        {
            double minZ = 20.0;
            double maxZ = 200.0;
            if (position.Z > maxZ)
            {
                position.Z = maxZ;
            }
            if (position.Z < minZ)
            {
                position.Z = minZ;
            }
            camera.Position = position;
        }

        public double GetRotationAngle()
        {
            return cameraTransformRotation.Angle;
        }

        public void SetCameraRotation(double angle)
        {
            double minAngle = -35.0;
            double maxAngle = 35.0;
            if (angle > maxAngle)
            {
                angle = maxAngle;
            }
            if (angle < minAngle)
            {
                angle = minAngle;
            }
            cameraTransformRotation.Angle = angle;
        }
    }
}
