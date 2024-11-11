using System.Windows;
using Lab1.Models;

namespace Lab1.App;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
    }

    private void OpenDicom_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog() { Multiselect = true };
        if (dialog.ShowDialog() == true)
        {
            string [] files = dialog.FileNames;
            viewer.ViewModel.SetDicomCommand.Execute(DicomManager.FromFiles(files));
        }
    }
}