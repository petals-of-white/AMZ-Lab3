using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Lab1.Models.Histogram;
using Lab1.Models.Tools.ROI;
using Lab1.ViewModels.Tools.ROI;

namespace Lab1.ViewModels;

public abstract class ROIViewModel : SimpleNotifier
{
    bool isActive, isShown;

    public abstract IRegionOfInterestInfo ROIInfo { get; }
    public abstract IHistogram<TPixel> Histogram<TPixel>();
    public ROIViewModel()
    {
        SetPointCommand = new RelayCommand<PointF>(SetPoint);
        ToggleROICommand = new RelayCommand(ToggleROI);


    }
    public ICommand SetPointCommand { get; }
    public ICommand ToggleROICommand { get; }
    protected abstract void SetPoint(PointF point);
    public bool IsActive
    {
        get => isActive; set
        {
            isActive = value; NotifyPropertyChanged(nameof(IsActive));
        }
    }

    //protected abstract void ToggleROI();
    protected void ToggleROI()
    {
        IsShown = !IsShown;
    }

    public bool IsShown
    {
        get => isShown; set
        {
            isShown = value; NotifyPropertyChanged(nameof(IsShown));
        }
    }

}
