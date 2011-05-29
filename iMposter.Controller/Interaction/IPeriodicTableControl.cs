using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using iMposter.Model.PeriodicTable;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using Nui.Vision;

namespace iMposter.Controller.Interaction
{
    public interface IPeriodicTableControl
    {
        void InitializePeriodicTableElements(IList<Element> elements);
        void FadeElementImage(Element element, double from, double to);
        int GetFadeTimeMiliseconds();
        void OnNavigationGesture(NuiUser user);
        void OnZoomGesture(double distance, double zoomDirection);
    }
}
