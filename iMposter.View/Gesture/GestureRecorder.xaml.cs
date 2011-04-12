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
using iMposter.Model.Sensor;
using iMposter.Model.ExtensionMethod;
using Nui.Vision;
using System.Threading;
using iMposter.Controller.Gesture;

namespace iMposter.View.Gesture
{
    /// <summary>
    /// Interaction logic for GestureRecorder.xaml
    /// </summary>
    public partial class GestureRecorder : UserControl
    {
        protected int recordedGestures;
        public String[] Labels { get { return new String[] { "Navigation gesture", "Zoom gesture" }; } }
        protected GestureDetector gestureDetector;

        public GestureRecorder()
        {
            InitializeComponent();
            gestureDetector = new GestureDetector();
        }

        void GestureRecorder_GestureComplete_UpdateRecordedGesturesTextblock(double[][] gesturePath)
        {
            if (recordedGestures < 40)
            {
                recordedGestures++;
                string parameterInitializationString = "";
                parameterInitializationString += "new double[][] {";
                foreach (var element in gesturePath)
                {
                    parameterInitializationString += "new double[] {";
                    for (int i = 0; i < gestureDetector.RecorderSequenceSubsequenceDimension; i++)
                    {
                        parameterInitializationString += element[i].ToString().Replace(",", ".") + ", ";
                    }
                    parameterInitializationString += "}, ";
                }
                parameterInitializationString += "},";
                recordedGesturesTextblock.Dispatcher.BeginInvoke((Action)delegate
                {
                    recordedGesturesTextblock.AppendText(parameterInitializationString);
                    recordedGesturesTextblock.AppendText("\n");
                });
            }
        }

        protected void recordButton_Click(object sender, RoutedEventArgs e)
        {
            recordedGestures = 0;
            gestureDetector.GestureComplete += new MyEventHandler(GestureRecorder_GestureComplete_UpdateRecordedGesturesTextblock);
        }

        private void recognizeButton_Click(object sender, RoutedEventArgs e)
        {
            gestureDetector.GestureComplete += new MyEventHandler(GestureRecorder_GestureComplete_UpdateRecognizedGesturesTextblock);
        }

        void GestureRecorder_GestureComplete_UpdateRecognizedGesturesTextblock(double[][] gesturePath)
        {
            recognizedGesturesTextblock.Dispatcher.BeginInvoke((Action)delegate
            {
                int recognizedLabelIndex = gestureDetector.HiddenMarkovModelDetect(gesturePath);
                if (recognizedLabelIndex != -1) {
                    recognizedGesturesTextblock.Text = gestureDetector.Labels[recognizedLabelIndex];
                } else {
                    recognizedGesturesTextblock.Text = "Unknown";
                }
            });
        }
    }
}
