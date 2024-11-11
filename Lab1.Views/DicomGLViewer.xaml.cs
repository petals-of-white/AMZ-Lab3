using System.ComponentModel;
using System.Windows.Controls;
using Lab1.Models;
using Lab1.Models.Tools.ROI;
using Lab1.Views.Graphics;
using Lab1.Views.Tools.ROI;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;

namespace Lab1.Views;

public partial class DicomGLViewer : UserControl
{
    private readonly DicomGLState glState;

    private RectangleROITool? roiTool;

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

    private void OpenTkControl_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
    {
        viewModel.AdvanceDepthCommand.Execute(e.Delta);
    }

    private void OpenTkControl_Render(TimeSpan obj)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        glState.DrawVertices(viewModel.CurrentDepth);
        roiTool?.Draw();
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(viewModel.DicomManager) && viewModel.DicomManager is DicomManager dicom)
        {
            glState.LoadDicomTexture(dicom);
        }
        else if (e.PropertyName == nameof(viewModel.SelectedROI) && viewModel.SelectedROI is RectangleROI roi && roiTool is not null)
        {
            roiTool = new() { Tool = roi };
            roiTool.UploadPoints();
        }
        //else if (e.PropertyName == nameof(viewModel.CurrentDepth))
        //{
        //}
    }
}