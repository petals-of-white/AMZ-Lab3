using System.Windows;
using Lab1.Models;
using Lab1.Views.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Wpf;
using ScottPlot;

namespace Lab1.App;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
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

        sagittalViewer.InitOpenGL(settings);

        coronalViewer.InitOpenGL(settings);

        glContext.MakeCurrent();
        DicomScene dicomScene = new();

        axialViewer.LoadScene(dicomScene);
        //sagittalViewer.LoadScene(dicomScene);
        //coronalViewer.LoadScene(dicomScene);


        //double [] heights = SampleData.MaleHeights();
        //var hist = ScottPlot.Statistics.Histogram.WithBinCount(10, heights);

        //var barPlot = WpfPlot1.Plot.Add.Bars();


        //// Customize the style of each bar
        //foreach (var bar in barPlot.Bars)
        //{
        //    bar.Size = hist.FirstBinSize;
        //    bar.LineWidth = 0;
        //    bar.FillStyle.AntiAlias = false;
        //}
        //WpfPlot1.Plot.Axes.Margins(bottom: 0);
        //WpfPlot1.Plot.YLabel("Number of People");
        //WpfPlot1.Plot.XLabel("Height (cm)");

        //WpfPlot1.Refresh();
    }

    private void DrawHistogram(IReadOnlyCollection<short> pixels) {
        double [] heights = SampleData.MaleHeights();
        var hist = ScottPlot.Statistics.Histogram.WithBinCount(10, pixels.Select(px => (double) px));


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
    private void ROIViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

        if (e.PropertyName == nameof(axialViewer.ViewModel.ROIViewModel.Histogram))
        {
            DrawHistogram(axialViewer.ViewModel.ROIViewModel.Histogram.PixelsInRegion());
        }
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

            //sagittalViewer.ViewModel.SetDicomCommand.Execute(dicomData);
            //coronalViewer.ViewModel.SetDicomCommand.Execute(dicomData);
        }
    }

    private void RoiBtn_Click(object sender, RoutedEventArgs e)
    {
        //axialViewer.ViewModel.DisplayROICommand.Execute(null);
        //coronalViewer.ViewModel.DisplayROICommand.Execute(null);
        //sagittalViewer.ViewModel.DisplayROICommand.Execute(null);
        axialViewer.ViewModel.ROIViewModel?.ToggleROICommand.Execute(null);
        //coronalViewer.ViewModel.DisplayROICommand.Execute(null);
        //sagittalViewer.ViewModel.DisplayROICommand.Execute(null);
    }
}