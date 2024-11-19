using System.Windows;
using Lab1.Models;
using Lab1.Views.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Wpf;

namespace Lab1.App;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ExampleScene scene1 = new ExampleScene();
    private ExampleScene scene2 = new ExampleScene();
    private ExampleScene scene3 = new ExampleScene();

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
        sagittalViewer.LoadScene(dicomScene);
        coronalViewer.LoadScene(dicomScene);
    }

    private void OpenDicom_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog() { Multiselect = true };
        if (dialog.ShowDialog() == true)
        {
            string [] files = dialog.FileNames;
            var dicomData = DicomManager.FromFiles(files);

            axialViewer.ViewModel.SetDicomCommand.Execute(dicomData);
            sagittalViewer.ViewModel.SetDicomCommand.Execute(dicomData);
            coronalViewer.ViewModel.SetDicomCommand.Execute(dicomData);
        }
    }

    private void RoiBtn_Click(object sender, RoutedEventArgs e)
    {
        axialViewer.ViewModel.DisplayROICommand.Execute(null);
        coronalViewer.ViewModel.DisplayROICommand.Execute(null);
        sagittalViewer.ViewModel.DisplayROICommand.Execute(null);
        //Command="{Binding ElementName=axialViewer,Path=ViewModel.DisplayROICommand}"
    }
}