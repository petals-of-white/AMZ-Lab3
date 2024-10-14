using System.Collections.ObjectModel;
using System.Windows;
using FellowOakDicom;
namespace Lab1.App;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        //mainState = new ViewModel.App();
        InitializeComponent();
    }

    private void OpenDicom_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog() { Multiselect = true };
        if (dialog.ShowDialog() == true)
        {
            string [] files = dialog.FileNames;
            var dicoms = files.Select((file) => DicomFile.Open(file)).ToArray();
            mainState.Dicoms = new ObservableCollection<DicomFile>(dicoms);
            viewer.AddDicom(dicoms);
        }
    }

}