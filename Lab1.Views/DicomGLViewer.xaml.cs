using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Lab1.Models;
using Lab1.ViewModels;
using Lab1.Views.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Wpf;

namespace Lab1.Views;

public partial class DicomGLViewer : UserControl
{
    //private (float, float, float, float) color = (Random.Shared.NextSingle(), Random.Shared.NextSingle(), Random.Shared.NextSingle(), 1);
    private DicomScene? glState;

    //private RectangleROIViewModel? roiViewModel;
    private DicomViewModel viewModel = new();

    public DicomGLViewer()
    {
        InitializeComponent();
        ViewModel = new();
    }

    //public RectangleROIViewModel? ROIViewModel
    //{
    //    get => roiViewModel; set
    //    {
    //        //roiViewModel.PropertyChanged -= ROIViewModel_PropertyChanged;

    //        //value.PropertyChanged += ROIViewModel_PropertyChanged;

    //        roiViewModel = value;
    //    }
    //}

    public DicomViewModel ViewModel
    {
        get => viewModel;

        set
        {
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;

            value.PropertyChanged += ViewModel_PropertyChanged;
            DataContext = value;
            viewModel = value;
        }
    }

    public IGraphicsContext InitOpenGL(GLWpfControlSettings settings)
    {
        openTkControl.Start(settings);

        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);

        GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) =>
        {
            Debug.WriteLine($"OpenGL Debug: {Marshal.PtrToStringAnsi(message)}");
        }, IntPtr.Zero);

        return openTkControl.Context!;
    }

    public void LoadScene(DicomScene? dicomScene = null)
    {
        glState = dicomScene is null ? new() : dicomScene;
    }

    private void DisplayRegionClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        //if (viewModel.DicomData is not null && viewModel.SelectedROI is not null)
        //{
        //    var control = (UIElement) sender;
        //    var coords = e.GetPosition(control);
        //    viewModel.SetPointCommand.Execute(new PointF((float) coords.X, (float) coords.Y));
        //}

        if (viewModel.DicomData is IDicomData dicomData)
        {
            var control = (UIElement) sender;
            var coords = e.GetPosition(control);
            var newPoint = new PointF((float) coords.X, (float) coords.Y);

            viewModel.ROIViewModel?.SetPointCommand.Execute(newPoint);
        }

    }

    private void OpenTkControl_Render(TimeSpan obj)
    {
        openTkControl.Context?.MakeCurrent();
        GL.Clear(ClearBufferMask.ColorBufferBit);
        glState?.DrawVertices(viewModel.CurrentPlane, viewModel.CurrentSlice);
    }

    private void ROIViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        //if (e.PropertyName == nameof(roiViewModel.))
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(viewModel.DicomData) && viewModel.DicomData is DicomManager dicom)
        {
            glState?.LoadDicomTexture(dicom);
        }
    }
}