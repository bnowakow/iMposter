using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Controls;
using ControlsImage = System.Windows.Controls.Image;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace iMposter.Model.PeriodicTable
{
    public delegate void FadeElementImageDelegate(Element element, double from, double to);

    public class Element
    {
        public Point Location { get; set; }
        public bool IsOverridden { get; set; }
        public ControlsImage Image { get; set; }
        public ImageSource DefaultImageSource { get; set; }
        public ImageSource NewImageSource { get; set; }
        public FadeElementImageDelegate FadeElementImage { get; set; }
        
        public Element(Point Location)
        {
            this.Location = Location;
            this.IsOverridden = false;
        }

        protected readonly Object ImageLock = new Object();

        public void SetNewImage()
        {
            if (Image != null)
            {
                Image.Source = NewImageSource;
            }
            IsOverridden = true;

        }

        public void RevertDefaultImage()
        {
            if (Image != null)
            {
                Image.Source = DefaultImageSource;
            }
            IsOverridden = false;
        }

        public void FadeInElementImage()
        {
            FadeElementImage(this, 0, 1);
        }

        public void FadeOutElementImage()
        {
            FadeElementImage(this, 1, 0);
        }
    }
}
