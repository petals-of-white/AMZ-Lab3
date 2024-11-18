using System.Drawing;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Lab1.Models;
using Lab1.Models.Tools.ROI;
using Lab1.ViewModels.Tools.ROI;

namespace Lab1.ViewModels;

public class DicomViewModel : SimpleNotifier
{
    private uint currentDepth = 1;
    private AnatomicPlane currentPlane = AnatomicPlane.Axial;
    private int currentSlice;
    private IDicomData? dicomManager;
    private PointF lastSetPoint;
    private RectangleRegion? selectedROI;

    public DicomViewModel()
    {
        SetDicomCommand = new RelayCommand<DicomManager>(SetDicom);
        SetPointCommand = new RelayCommand<PointF>(SetPoint);
        DisplayROICommand = new RelayCommand(ToggleROI);
        SelectRegionCommand = new RelayCommand<Models.Shapes.Rectangle>(SelectRegion);
        //AdvanceDepthCommand = new RelayCommand<int>(AdvanceInDepth);
    }

    public uint CurrentDepth
    {
        get => currentDepth;
        set
        {
            currentDepth = value switch
            {
                (< 1u) => 1u,
                var depth => dicomManager?.Depth switch
                {
                    int dicomDepth when depth <= dicomDepth => depth,
                    int dicomDepth when depth > dicomDepth => (uint) dicomDepth,
                    _ => currentDepth
                }
            };
            NotifyPropertyChanged(nameof(CurrentDepth));
        }
    }

    public AnatomicPlane CurrentPlane
    {
        get => currentPlane; set
        {
            currentPlane = value; NotifyPropertyChanged(nameof(CurrentPlane));
        }
    }

    public int CurrentSlice
    {
        get => currentSlice; set
        {
            currentSlice = value;
            NotifyPropertyChanged(nameof(CurrentSlice));
        }
    }

    //public ICommand AdvanceDepthCommand { get; }
    public IDicomData? DicomManager
    {
        get => dicomManager;
        private set { dicomManager = value; NotifyPropertyChanged(nameof(DicomManager)); }
    }

    public ICommand DisplayROICommand { get; }

    public IRegionOfInterestInfo? ROIInfo => (selectedROI, dicomManager) switch
    {
        (RectangleRegion { IsDisplayed: true, Region: var region }, IDicomData dicomData) => new DicomRectangleROI(region, dicomData),
        _ => null
    };

    public RectangleRegion? SelectedROI
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

    private void SelectRegion(Models.Shapes.Rectangle rectangle) => SelectedROI = SelectedROI switch
    {
        RectangleRegion rectRegion => rectRegion with { Region = rectangle },
        null => null
    };

    private void SetDicom(DicomManager dicom)
    {
        DicomManager = dicom;
        NotifyPropertyChanged(nameof(ROIInfo));
    }

    private void SetPoint(PointF newPoint)
    {
        if (SelectedROI is not null)
        {
            var newRegion = new Models.Shapes.Rectangle { P1 = lastSetPoint, P2 = newPoint };
            SelectedROI = SelectedROI with { Region = newRegion };
            NotifyPropertyChanged(nameof(SelectedROI));
            lastSetPoint = newPoint;
            NotifyPropertyChanged(nameof(ROIInfo));
        }
    }

    private void ToggleROI()
    {
        var roi = (SelectedROI ??= new RectangleRegion(new(), false, true));
        SelectedROI = roi with { IsDisplayed = !roi.IsDisplayed };
        NotifyPropertyChanged(nameof(SelectedROI));
        NotifyPropertyChanged(nameof(ROIInfo));
    }
}