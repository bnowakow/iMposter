using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iMposter.Model.Sensor;
using Nui.Vision;
using iMposter.Model.ExtensionMethod;
using Accord.Statistics.Models.Markov.Topology;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Distributions.Fitting;
using Accord.Statistics.Models.Markov.Learning;
using Accord.Statistics.Distributions.Multivariate;
using Sharp3D.Math.Core;
using System.Xml.Serialization;
using System.IO;
using iMposter.Model.Gesture;

namespace iMposter.Controller.Gesture
{
    public delegate void GestureCaptureCompleteDelegate(double[][] gesturePath);

    public class GestureDetector : IGestureDetector
    {
        static GestureDetector instance = null;
        static readonly object padlock = new object();

        public event GestureCaptureCompleteDelegate GestureCaptureComplete;

        public GestureDetectorTrainingData TrainingData { get; set; }

        public bool Capturing { get; set; }
        public bool ContinuesCapturing { get; set; }
        protected ContinuousSequenceClassifier classifier;
        protected IList<NuiUser> singleGestureElements;

        public String[] Labels { get { return new String[] { "Idle gesture", "Navigation gesture", "Zoom gesture" }; } }
        public enum GestureCodes { IDLE, NAVIGATION, ZOOM };

        protected int singleGestureLegth = 5; // number of consecutive points of move in gesture
        protected int singleGestureElementDiemension = 4; // left\right and hand\forearm angles
        protected bool centerStatistics = false;

        public static GestureDetector Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GestureDetector();
                    }
                    return instance;
                }
            }
        }

        private GestureDetector()
        {
            BodyTracker bodyTracker = BodyTracker.Instance;
            bodyTracker.Tracker.UserUpdated += new Nui.Vision.NuiUserTracker.UserUpdatedHandler(Tracker_UserMoved);

            ClearSingleGesture();
            TrainingData = GestureDetectorTrainingData.ReadFromXml();
            HiddenMarkovModelLearn();

            bool learningMode = false;
            if (learningMode)
            {
                singleGestureLegth = 20;
            }
            else
            {
                Capturing = true;
                ContinuesCapturing = true;
            }
        }

        protected void ClearSingleGesture()
        {
            singleGestureElements = new List<NuiUser>();
        }

        public void HiddenMarkovModelLearn()
        {
            GestureDetectorTrainingData.SaveToXml(TrainingData);
            double[][][] inputs = TrainingData.Inputs;
            int[] outputs = TrainingData.Outputs;

            NormalDistribution initial = new NormalDistribution(singleGestureElementDiemension);

            int states = 6;
            double tolerance = 0.01;
            int iterations = 0;
            classifier = new ContinuousSequenceClassifier(Labels.Count(), new Forward(states), initial, Labels);

            var teacher = new ContinuousSequenceClassifierLearning(
                classifier,
                j => new ContinuousBaumWelchLearning(classifier.Models[j])
                {
                    Tolerance = tolerance,
                    Iterations = iterations,
                    FittingOptions = new NormalOptions() { Regularization = 0.1 }
                }
            );
            teacher.UpdateThreshold = false;
            teacher.Run(inputs, outputs);

            // check procedure
            int i = 0;
            int matches = 0;
            foreach (var gesture in inputs)
            {
                int checkedIndex = HiddenMarkovModelDetect(gesture);
                if (outputs[i++] == checkedIndex)
                {
                    matches++;
                }
            }
        }

        public int HiddenMarkovModelDetect(double[][] gesturePath)
        {
            double[] responses;
            int classifierResult = classifier.Compute(
                gesturePath,
                out responses);
            return classifierResult;
        }

        // TODO implement as extension method to NuiUser
        protected double CalculateAngleBetweenBodyParts(NuiUserBodyPart beginA, NuiUserBodyPart endA, NuiUserBodyPart beginB, NuiUserBodyPart endB)
        {
            Vector3D lefForearmVector = new Vector3D(
                beginA.X - endA.X,
                beginA.Y - endA.Y,
                beginA.Z - endA.Z
                );
            Vector3D lefArmVector = new Vector3D(
                beginB.X - endB.X,
                beginB.Y - endB.Y,
                beginB.Z - endB.Z
                );
            var cosAngle = Vector3D.DotProduct(lefForearmVector, lefArmVector) / (lefForearmVector.GetLength() * lefArmVector.GetLength());
            var angle = Math.Acos(cosAngle);
            return angle;
        }

        protected double[][] GetCapturedGestureSequence()
        {
            if (singleGestureElements.Count == singleGestureLegth)
            {
                Capturing = false;

                double[][] sequence = new double[singleGestureElements.Count][];

                int i = 0;
                foreach (var singleGestureElement in singleGestureElements)
                {
                    var leftForearmArmAngle = CalculateAngleBetweenBodyParts(
                        singleGestureElement.LeftElbow, singleGestureElement.LeftHand,
                        singleGestureElement.LeftElbow, singleGestureElement.LeftShoulder);
                    var rightForearmArmAngle = CalculateAngleBetweenBodyParts(
                        singleGestureElement.RightElbow, singleGestureElement.RightHand,
                        singleGestureElement.RightElbow, singleGestureElement.RightShoulder);
                    var leftArmHipAngle = CalculateAngleBetweenBodyParts(
                        singleGestureElement.LeftShoulder, singleGestureElement.LeftElbow,
                        singleGestureElement.LeftShoulder, singleGestureElement.LeftHip);
                    var rightArmHipAngle = CalculateAngleBetweenBodyParts(
                        singleGestureElement.RightShoulder, singleGestureElement.RightElbow,
                        singleGestureElement.RightShoulder, singleGestureElement.RightHip);
                    sequence[i++] = new double[] { leftForearmArmAngle, rightForearmArmAngle, leftArmHipAngle, rightArmHipAngle };
                }

                if (centerStatistics)
                {
                    Accord.Statistics.Tools.Center(sequence);
                }

                ClearSingleGesture();
                if (ContinuesCapturing)
                {
                    Capturing = true;
                }
                return sequence;
            }
            else
            {
                return null;
            }
        }

        protected void Tracker_UserMoved(object sender, Nui.Vision.NuiUserEventArgs e)
        {
            if (Capturing)
            {
                if (singleGestureElements.Count == singleGestureLegth)
                {
                    if (GestureCaptureComplete != null)
                    {
                        GestureCaptureComplete(GetCapturedGestureSequence());
                    }
                }
                else
                {
                    // TODO deal with multiple users
                    var user = e.Users.First();
                    singleGestureElements.Add(user.Copy());
                }
            }
        }
    }
}
