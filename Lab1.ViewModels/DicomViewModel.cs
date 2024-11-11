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
        LoadDicomCommand = new RelayCommand<string>(LoadDicom);
        SetPointCommand = new RelayCommand<PointF>(SetPoint);
        DisplayROICommand = new RelayCommand(DisplayROI);
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

    public ICommand LoadDicomCommand { get; }

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

    public ICommand SetPointCommand { get; }

    private void AdvanceInDepth(int move)
    {
        CurrentDepth += (move / CurrentDepth);
    }

    private void DisplayROI()
    {
        (SelectedROI ??= new RectangleROI() { IsActive = true, IsDisplayed = true }).IsDisplayed = true;
    }

    private void LoadDicom(string filePath)
    {
        DicomManager = DicomManager.FromFile(filePath);
    }

    private void SelectRegion(Models.Shapes.Rectangle region)
    {
        SelectedROI = new RectangleROI { Region = region, IsActive = true, IsDisplayed = true };
    }

    private void SetPoint(PointF newPoint)
    {
        if (SelectedROI is not null)
        {
            var newRegion = new Models.Shapes.Rectangle { P1 = lastSetPoint, P2 = newPoint };
            SelectedROI.Region = newRegion;
            lastSetPoint = newPoint;
        }
    }
}