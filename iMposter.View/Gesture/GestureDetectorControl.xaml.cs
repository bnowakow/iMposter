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
        protected IList<double[][]> inputs;
        protected IList<int> outputs;
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
                gestureButton.Content = gestureLabel;
                gestureButton.Click += new RoutedEventHandler(gestureButton_Click);
                gestureNamesPlaceholder.Children.Add(gestureButton);
                i++;
            }

            inputs = new List<double[][]>();
            outputs = new List<int>();

            gestureDetector.GestureCaptureComplete += new GestureCaptureCompleteDelegate(gestureDetector_GestureCaptureComplete);
        }

        void gestureButton_Click(object sender, RoutedEventArgs e)
        {
            Button learnButton = sender as Button;
            LearnCapturedGesture((int)learnButton.Tag);
        }

        protected void LearnCapturedGesture(int output)
        {
            //var input = gestureDetector.GetCapturedGestureSequence();
            var input = capturedGesture;
            inputs.Add(input);
            outputs.Add(output);
            double[][][] inputsArray = new double[inputs.Count][][];
            int i = 0;
            foreach (var inputGesture in inputs)
            {
                inputsArray[i++] = inputGesture;
            }
            gestureDetector.HiddenMarkovModelLearn(inputsArray, outputs.ToArray());
        }

        void gestureDetector_GestureCaptureComplete(double[][] gesturePath)
        {
            gestureCaptureStatusTextbox.Dispatcher.BeginInvoke((Action)delegate
            {
                gestureCaptureStatusTextbox.Text = "Gesture Capture Completed";
            });
            capturedGesture = gesturePath;
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
            //var gesture = gestureDetector.GetCapturedGestureSequence();
            var gesture = capturedGesture;
            int gestureDetectedIndex = gestureDetector.HiddenMarkovModelDetect(gesture);
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

    }
}
