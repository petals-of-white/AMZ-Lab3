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
    private bool isActive, isShown;

    public ROIViewModel()
    {
        SetPointCommand = new RelayCommand<PointF>(SetPoint);
        ToggleROICommand = new RelayCommand(ToggleROI);
    }

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

    public abstract IReadOnlyCollection<short> SelectedPixels { get; }
    public ICommand SetPointCommand { get; }

    public ICommand ToggleROICommand { get; }

    protected abstract void SetPoint(PointF point);

    protected void ToggleROI()
    {
        IsShown = !IsShown;
    }
}