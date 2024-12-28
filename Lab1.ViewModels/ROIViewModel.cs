using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Models;
using Lab1.Models.Tools.ROI;

namespace Lab1.ViewModels;

public abstract class ROIViewModel : SimpleNotifier
{
    bool isActive, isShown;

    public abstract IRegionOfInterestInfo ROIInfo { get; }
    public abstract IHistogram<TPixel> Histogram<TPixel>();
    public abstract void SetPoint(PointF point);
    public bool IsActive
    {
        get => isActive; set
        {
            isActive = value; NotifyPropertyChanged(nameof(IsActive));
        }
    }
    public bool IsShown
    {
        get => isShown; set
        {
            isShown = value; NotifyPropertyChanged(nameof(IsShown));
        }
    }

}
