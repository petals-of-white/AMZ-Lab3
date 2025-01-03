using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Lab1.Models;
using Lab1.Models.Histogram;

namespace Lab1.ViewModels;

public class DicomViewModel : SimpleNotifier
{
    private AnatomicPlane currentPlane = AnatomicPlane.Axial;
    private int currentSlice;
    private IDicomData? dicomData;
    private RectangleROIViewModel? roiViewModel;

    public DicomViewModel()
    {
        SetDicomCommand = new RelayCommand<DicomManager>(SetDicom);
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
            if (roiViewModel is not null)
                roiViewModel.SliceNumber = value;

            NotifyPropertyChanged(nameof(CurrentSlice));
        }
    }

    public IDicomData? DicomData
    {
        get => dicomData;
        private set
        {
            dicomData = value;
            NotifyPropertyChanged(nameof(DicomData));
            if (value is not null)
                ROIViewModel = new(default, new RectangleROIDicomDataHistogram(value, CurrentSlice)) { IsShown = true };
        }
    }

    public int? DisplayHeight => (currentPlane, dicomData) switch
    {
        (AnatomicPlane.Axial, IDicomData { DefaultPlane: AnatomicPlane.Axial, Height: var height }) => height,
        (AnatomicPlane.Coronal, IDicomData { DefaultPlane: AnatomicPlane.Axial, Width: var depth }) => depth,
        (AnatomicPlane.Saggital, IDicomData { DefaultPlane: AnatomicPlane.Axial, Height: var height }) => height,
        _ => null
    };

    public int? DisplayWidth => (currentPlane, dicomData) switch
    {
        (AnatomicPlane.Axial, IDicomData { DefaultPlane: AnatomicPlane.Axial, Width: var width }) => width,
        (AnatomicPlane.Coronal, IDicomData { DefaultPlane: AnatomicPlane.Axial, Width: var width }) => width,
        (AnatomicPlane.Saggital, IDicomData { DefaultPlane: AnatomicPlane.Axial, Depth: var depth }) => depth,
        _ => null
    };

    public RectangleROIViewModel? ROIViewModel
    {
        get => roiViewModel;
        private set
        {
            roiViewModel = value;
            NotifyPropertyChanged(nameof(ROIViewModel));
        }
    }

    public ICommand SetDicomCommand { get; }

    private void SetDicom(IDicomData dicom)
    {
        DicomData = dicom;
        //NotifyPropertyChanged(nameof(ROIInfo));
        NotifyPropertyChanged(nameof(DisplayHeight));
        NotifyPropertyChanged(nameof(DisplayWidth));
    }
}