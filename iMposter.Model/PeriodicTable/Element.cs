using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Controls;

namespace iMposter.Model.PeriodicTable
{
    public class Element
    {
        public Point Location { get; set; }
        public bool IsOverridden { get; set; }
        public System.Windows.Controls.Image Image { get; set; }

        public Element(Point Location)
        {
            this.Location = Location;
            this.IsOverridden = false;
        }
    }
}
