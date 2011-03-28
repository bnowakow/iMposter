using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using iMposter.Model.PeriodicTable;

namespace iMposter.Controller.Interaction
{
    public interface IPeriodicTableControl
    {
        void InitializePeriodicTableElements(List<Element> elements);
    }
}
