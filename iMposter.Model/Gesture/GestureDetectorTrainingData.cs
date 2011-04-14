using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace iMposter.Model.Gesture
{
    public class GestureDetectorTrainingData
    {
        [XmlIgnore()]
        public double[][][] Inputs
        {
            get
            {
                double[][][] inputsArray = new double[InputsList.Count][][];
                int i = 0;
                foreach (var inputGesture in InputsList)
                {
                    inputsArray[i++] = inputGesture;
                }
                return inputsArray;
            }
        }
        [XmlIgnore()]
        public int[] Outputs { get { return OutputsList.ToArray(); } }
        public List<double[][]> InputsList { get; set; }
        public List<int> OutputsList { get; set; }
        protected static String filename = @ModelSettings.Default.gestureDetectorTrainingDataFile;

        public GestureDetectorTrainingData()
        {
            InputsList = new List<double[][]>();
            OutputsList = new List<int>();
        }

        public static void SaveToXml(GestureDetectorTrainingData obj)
        {
            StreamWriter stream = File.CreateText(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(GestureDetectorTrainingData));
            serializer.Serialize(stream, obj);
            stream.Close();
        }

        public static GestureDetectorTrainingData ReadFromXml()
        {
            GestureDetectorTrainingData obj = new GestureDetectorTrainingData();
            Stream stream;
            try
            {
                stream = File.OpenRead(filename);
            }
            catch (FileNotFoundException e)
            {
                return obj;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(GestureDetectorTrainingData));
            obj = serializer.Deserialize(stream) as GestureDetectorTrainingData;
            stream.Close();
            return obj;
        }
    }
}
