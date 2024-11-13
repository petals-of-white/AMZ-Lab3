using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lab1.Models;
using Lab1.Models.Tools.ROI;
using Lab1.ViewModels;
using Lab1.Views.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;

namespace Lab1.Views;

public partial class DicomGLViewer : UserControl
{
    private readonly DicomGLState glState;

    // TODO: Introduce ViewModel for DicomManager
    public DicomGLViewer()
    {
        InitializeComponent();
        var settings = new GLWpfControlSettings()
        {
            MajorVersion = 4,
            MinorVersion = 3
        };

        openTkControl.Start(settings);
        GL.ClearColor(0, 0, 0, 1);

        glState = new DicomGLState();

        viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    public DicomViewModel ViewModel => viewModel;
    public DicomViewModel ViewModelState { set => viewModel = value; }

    private void DisplayRegionClick(object sender, MouseButtonEventArgs e)
    {
        if (viewModel.DicomManager is DicomManager dicom && viewModel.SelectedROI is RectangleROI roi)
        {
            var control = (UIElement) sender;
            var coords = e.GetPosition(control);
            viewModel.SetPointCommand.Execute(new PointF((float) coords.X, (float) coords.Y));
        }
    }

    private void OpenTkControl_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        viewModel.AdvanceDepthCommand.Execute(int.Sign(e.Delta));
    }

    private void OpenTkControl_Render(TimeSpan obj)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        glState.DrawVertices(viewModel.CurrentDepth);
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(viewModel.DicomManager) && viewModel.DicomManager is DicomManager dicom)
        {
            glState.LoadDicomTexture(dicom);
        }
    }
}