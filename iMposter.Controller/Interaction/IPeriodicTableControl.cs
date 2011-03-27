using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace iMposter.Controller.Interaction
{
    public interface IPeriodicTableControl
    {
        Image GetElement(int row, int column);
        bool[,] GetElementExists();
        int GetRowNumber();
        int GetColumnNumber();
    }
}
