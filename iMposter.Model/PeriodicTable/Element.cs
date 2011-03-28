using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Controls;
using ControlsImage = System.Windows.Controls.Image;
using System.Windows.Media;

namespace iMposter.Model.PeriodicTable
{
    public class Element
    {
        public Point Location { get; set; }
        public bool IsOverridden { get; set; }
        public ControlsImage Image { get; set; }
        public ImageSource DefaultImageSource { get; set; }
        public ImageSource NewImageSource { get; set; }

        public Element(Point Location)
        {
            this.Location = Location;
            this.IsOverridden = false;
        }

        protected readonly Object ImageLock = new Object();

        public void SetNewImage()
        {
            Image.Source = NewImageSource;
            IsOverridden = true;

        }

        public void RevertDefaultImage()
        {
            Image.Source = DefaultImageSource;
            IsOverridden = false;
        }
    }
}
