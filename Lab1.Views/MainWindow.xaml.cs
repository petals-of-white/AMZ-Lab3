using System.Windows;
using Lab1.Models;
using Lab1.Models.Histogram;
using Lab1.ViewModels;
using Lab1.Views.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Wpf;

namespace Lab1.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        //ImageStatsViewModel = new();
        //ImageStats2DViewModel = new();
        InitializeComponent();

        HistogramViewModel.PropertyChanged += HistogramViewModel_PropertyChanged;
        var settings = new GLWpfControlSettings()
        {
            MajorVersion = 4,
            MinorVersion = 6,
            ContextFlags = ContextFlags.Debug,
            Profile = ContextProfile.Compatability
        };

        IGraphicsContext glContext = axialViewer.InitOpenGL(settings);

        settings.ContextToUse = glContext;
        glContext.MakeCurrent();
        DicomScene dicomScene = new();
        axialViewer.LoadScene(dicomScene);
    }

    public HistogramViewModel HistogramViewModel => (HistogramViewModel) Resources ["histogramViewModel"];

    public ImageStatistics2DViewModel ImageStats2DViewModel => (ImageStatistics2DViewModel) Resources ["statistics2DViewModel"];

    public ImageStatisticsViewModel ImageStatsViewModel => (ImageStatisticsViewModel) Resources ["statistics1DViewModel"];

    public RectangleROIViewModel? SecondSliceViewModel { get; private set; }

    private void DrawHistogram(IEnumerable<double> pixels)
    {
        var hist = ScottPlot.Statistics.Histogram.WithBinCount(10, pixels);
        //WpfHistogram1.Plot
        WpfHistogram1.Plot.Clear();
        var barPlot = WpfHistogram1.Plot.Add.Bars(hist.Bins, hist.Counts);

        // Customize the style of each bar
        foreach (var bar in barPlot.Bars)
        {
            bar.Size = hist.FirstBinSize;
            bar.LineWidth = 0;
            bar.FillStyle.AntiAlias = false;
        }
        WpfHistogram1.Plot.Axes.Margins(bottom: 0);
        WpfHistogram1.Plot.YLabel("Кількість вокселів", 30);
        WpfHistogram1.Plot.Grid.XAxis.TickLabelStyle.FontSize = 20;
        WpfHistogram1.Plot.Grid.YAxis.TickLabelStyle.FontSize = 20;
        WpfHistogram1.Plot.XLabel("Інтенсивність", 30);

        WpfHistogram1.Refresh();
    }

    private void HistogramViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        DrawHistogram((sender as HistogramViewModel)!.Data);
    }

    private void OpenDicom_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog() { Multiselect = true };
        if (dialog.ShowDialog() == true)
        {
            string [] files = dialog.FileNames;
            var dicomData = DicomManager.FromFiles(files);

            SecondSliceViewModel = new(new System.Drawing.PointF(), new RectangleROIDicomDataHistogram(dicomData, 0));
            axialViewer.ViewModel.SetDicomCommand.Execute(dicomData);
            axialViewer.ViewModel.ROIViewModel!.PropertyChanged += ROIViewModel_PropertyChanged;
        }
    }

    private void RoiBtn_Click(object sender, RoutedEventArgs e)
    {
        axialViewer.ViewModel.ROIViewModel?.ToggleROICommand.Execute(null);
    }

    private void ROIViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ROIViewModel.SelectedPixels))
        {
            DicomViewModel dicomVM = axialViewer.ViewModel;
            var firstImage = dicomVM.ROIViewModel!.SelectedPixels.Select(px => (ushort) px).ToArray();
            ImageStatsViewModel.Pixels = firstImage;

            // create a new roi viewmodel with next slice as source
            var secondSliceROI = new RectangleROIViewModel(dicomVM.ROIViewModel!.Region, new(dicomVM.DicomData!, dicomVM.ROIViewModel.SliceNumber));
            secondSliceROI.SliceNumber++;
            var secondImage = secondSliceROI.SelectedPixels.Select(px => (ushort) px).ToArray();
            ImageStats2DViewModel.FirstImage = firstImage;
            ImageStats2DViewModel.SecondImage = secondImage;

            HistogramViewModel.Data = (sender as ROIViewModel)!.SelectedPixels.Select(sh => (double) sh).ToArray();
        }
    }

    private void toggleStatBtn_Click(object sender, RoutedEventArgs e)
    {
        ImageStatsViewModel.IsShown = !ImageStatsViewModel.IsShown;
        ImageStats2DViewModel.IsShown = !ImageStats2DViewModel.IsShown;
    }
}