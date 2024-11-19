using System.Drawing;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Lab1.Models;
using Lab1.Models.Tools.ROI;
using Lab1.ViewModels.Tools.ROI;

namespace Lab1.ViewModels;

public class DicomViewModel : SimpleNotifier
{
    private AnatomicPlane currentPlane = AnatomicPlane.Axial;
    private int currentSlice;
    private IDicomData? dicomData;
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
        get => dicomData;
        private set { dicomData = value; NotifyPropertyChanged(nameof(DicomManager)); }
    }

    public int? DisplayHeight => (currentPlane, dicomData) switch
    {
        (AnatomicPlane.Axial, IDicomData { DefaultPlane: AnatomicPlane.Axial, Height: var height }) => height,
        (AnatomicPlane.Coronal, IDicomData { DefaultPlane: AnatomicPlane.Axial, Width: var depth }) => depth,
        (AnatomicPlane.Saggital, IDicomData { DefaultPlane: AnatomicPlane.Axial, Height: var height }) => height,
        _ => null
    };

    public ICommand DisplayROICommand { get; }

    public int? DisplayWidth => (currentPlane, dicomData) switch
    {
        (AnatomicPlane.Axial, IDicomData { DefaultPlane: AnatomicPlane.Axial, Width: var width }) => width,
        (AnatomicPlane.Coronal, IDicomData { DefaultPlane: AnatomicPlane.Axial, Width: var width }) => width,
        (AnatomicPlane.Saggital, IDicomData { DefaultPlane: AnatomicPlane.Axial, Depth: var depth }) => depth,
        _ => null
    };

    public IRegionOfInterestInfo? ROIInfo => (selectedROI, dicomData) switch
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
        NotifyPropertyChanged(nameof(DisplayHeight));
        NotifyPropertyChanged(nameof(DisplayWidth));
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