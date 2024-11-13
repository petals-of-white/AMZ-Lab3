using System.Drawing;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Lab1.Models;
using Lab1.Models.Tools.ROI;

namespace Lab1.ViewModels;

public class DicomViewModel : SimpleNotifier
{
    private float currentDepth = 0f;
    private DicomManager? dicomManager;
    private PointF lastSetPoint;
    private RectangleROI? selectedROI;

    public DicomViewModel()
    {
        SetDicomCommand = new RelayCommand<DicomManager>(SetDicom);
        SetPointCommand = new RelayCommand<PointF>(SetPoint);
        DisplayROICommand = new RelayCommand(ToggleROI);
        SelectRegionCommand = new RelayCommand<Models.Shapes.Rectangle>(SelectRegion);
        AdvanceDepthCommand = new RelayCommand<int>(AdvanceInDepth);
    }

    public ICommand AdvanceDepthCommand { get; }

    public float CurrentDepth
    {
        get => currentDepth;
        private set
        {
            currentDepth = value switch
            {
                (< 0f) => 0f,
                (> 1f) => 1f,
                var other => other
            };
            NotifyPropertyChanged(nameof(CurrentDepth));
        }
    }

    public DicomManager? DicomManager
    {
        get => dicomManager;
        private set { dicomManager = value; NotifyPropertyChanged(nameof(DicomManager)); }
    }

    public ICommand DisplayROICommand { get; }

    public RectangleROI? SelectedROI
    {
        get => selectedROI;
        private set
        {
            selectedROI = value;
            NotifyPropertyChanged(nameof(SelectedROI));
        }
    }

    public ICommand SelectRegionCommand { get; }
    public ICommand SetDicomCommand { get; }
    public ICommand SetPointCommand { get; }

    private void AdvanceInDepth(int move)
    {
        if (dicomManager is not null) CurrentDepth += ((float) move / dicomManager.Depth);
    }

    private void SelectRegion(Models.Shapes.Rectangle region)
    {
        SelectedROI = new RectangleROI { Region = region, IsActive = true, IsDisplayed = true };
    }

    private void SetDicom(DicomManager dicom)
    {
        DicomManager = dicom;
    }

    private void SetPoint(PointF newPoint)
    {
        if (SelectedROI is not null)
        {
            var newRegion = new Models.Shapes.Rectangle { P1 = lastSetPoint, P2 = newPoint };
            SelectedROI.Region = newRegion;
            NotifyPropertyChanged(nameof(SelectedROI));
            lastSetPoint = newPoint;
        }
    }

    private void ToggleROI()
    {
        var roi = (SelectedROI ??= new RectangleROI() { IsActive = true, IsDisplayed = true });
        roi.IsDisplayed = !roi.IsDisplayed;
        NotifyPropertyChanged(nameof(SelectedROI));
    }
}