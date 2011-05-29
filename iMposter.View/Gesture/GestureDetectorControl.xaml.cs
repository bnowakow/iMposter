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
using iMposter.Controller.Gesture;

namespace iMposter.View.Gesture
{
    /// <summary>
    /// Interaction logic for GestureDetectorControl.xaml
    /// </summary>
    public partial class GestureDetectorControl : UserControl
    {
        protected GestureDetector gestureDetector;
        protected double[][] capturedGesture;

        public GestureDetectorControl()
        {
            InitializeComponent();

            gestureDetector = GestureDetector.Instance;
            int i = 0;
            foreach (var gestureLabel in gestureDetector.Labels)
            {
                Button gestureButton = new Button();
                gestureButton.Tag = i;
                gestureButton.FontSize = 20;
                gestureButton.Content = gestureLabel;
                gestureButton.Click += new RoutedEventHandler(gestureButton_Click);
                gestureNamesPlaceholder.Children.Add(gestureButton);
                i++;
            }
            SetToggleContinuesCapturingButtonName();

            gestureDetector.GestureCaptureComplete += new GestureCaptureCompleteDelegate(gestureDetector_GestureCaptureComplete);
        }

        void gestureButton_Click(object sender, RoutedEventArgs e)
        {
            Button learnButton = sender as Button;
            LearnCapturedGesture((int)learnButton.Tag);
        }

        protected void LearnCapturedGesture(int output)
        {
            // nadanie gestowi modelu reprezentującego przebieg gestu
            gestureDetector.TrainingData.InputsList.Add(capturedGesture);
            // nadanie gestowi etykiety przez nauczyciela
            gestureDetector.TrainingData.OutputsList.Add(output);
            // nauka klasyfikatora gestu
            gestureDetector.HiddenMarkovModelLearn();
        }

        void gestureDetector_GestureCaptureComplete(double[][] gesturePath)
        {
            gestureCaptureStatusTextbox.Dispatcher.BeginInvoke((Action)delegate
            {
                gestureCaptureStatusTextbox.Text = "Gesture Capture Completed";
            });
            capturedGesture = gesturePath;
            gestureDetectButton_Click(null, null);
        }

        private void gestureStartCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            gestureCaptureStatusTextbox.Dispatcher.BeginInvoke((Action)delegate
            {
                gestureCaptureStatusTextbox.Text = "Gesture Capture Started";
            });            
            gestureDetector.Capturing = true;
        }

        private void gestureDetectButton_Click(object sender, RoutedEventArgs e)
        {
            int gestureDetectedIndex = gestureDetector.HiddenMarkovModelDetect(capturedGesture);
            String newStatus;
            if (gestureDetectedIndex != -1)
            {
                newStatus = gestureDetector.Labels[gestureDetectedIndex];
            }
            else
            {
                newStatus = "Unknown";
            }
            gestureDetectStatusTextbox.Dispatcher.BeginInvoke((Action)delegate
            {
                gestureDetectStatusTextbox.Text = newStatus;
            });
        }

        private void toggleContinuesCapturingButton_Click(object sender, RoutedEventArgs e)
        {
           
            gestureDetector.ContinuesCapturing = !gestureDetector.ContinuesCapturing;
            SetToggleContinuesCapturingButtonName();
        }

        protected void SetToggleContinuesCapturingButtonName()
        {
            toggleContinuesCapturingButton.Content = "Toggle ContinuesCapturing [" + gestureDetector.ContinuesCapturing + "]";
        }

    }
}
