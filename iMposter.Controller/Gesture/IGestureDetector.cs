using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMposter.Controller.Gesture
{
    public interface IGestureDetector
    {
        void HiddenMarkovModelLearn();
        int HiddenMarkovModelDetect(double[][] gesturePath);
    }
}
