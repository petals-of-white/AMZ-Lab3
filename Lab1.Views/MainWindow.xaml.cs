using System.Windows;
using Lab1.Models;
using Lab1.ViewModels;
using Lab1.Views.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Wpf;
using ScottPlot;

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
        //axialViewer.ViewModel.ROIViewModel.PropertyChanged += ROIViewModel_PropertyChanged1;

        //sagittalViewer.InitOpenGL(settings);

        //coronalViewer.InitOpenGL(settings);

        //sagittalViewer.LoadScene(dicomScene);
        //coronalViewer.LoadScene(dicomScene);
    }

    public ImageStatistics2DViewModel ImageStats2DViewModel => (ImageStatistics2DViewModel) Resources ["statistics2DViewModel"];
    public ImageStatisticsViewModel ImageStatsViewModel => (ImageStatisticsViewModel) Resources ["statistics1DViewModel"];

    private void DrawHistogram(IReadOnlyCollection<short> pixels)
    {
        double [] heights = SampleData.MaleHeights();
        var hist = ScottPlot.Statistics.Histogram.WithBinCount(10, pixels.Select(px => (double) px));

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
        WpfHistogram1.Plot.YLabel("Number of People");
        WpfHistogram1.Plot.XLabel("Height (cm)");

        WpfHistogram1.Refresh();
    }

    private void OpenDicom_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog() { Multiselect = true };
        if (dialog.ShowDialog() == true)
        {
            string [] files = dialog.FileNames;
            var dicomData = DicomManager.FromFiles(files);

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
        if (e.PropertyName == nameof(axialViewer.ViewModel.ROIViewModel.Histogram))
        {
            DrawHistogram(axialViewer.ViewModel.ROIViewModel.Histogram.PixelsInRegion());
        }
        if (e.PropertyName == nameof(ROIViewModel.SelectedPixels))
        {
            ImageStatsViewModel.Pixels = axialViewer.ViewModel.ROIViewModel.SelectedPixels.Select(px => (ushort) px).ToArray();
        }
    }

    private void toggleStatBtn_Click(object sender, RoutedEventArgs e)
    {
        ImageStatsViewModel.IsShown = !ImageStatsViewModel.IsShown;
        ImageStats2DViewModel.IsShown = !ImageStats2DViewModel.IsShown;
    }
}