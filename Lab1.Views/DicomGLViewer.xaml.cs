using System.Windows.Controls;
using Lab1.Models;
using Lab1.Views.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;

namespace Lab1.Views;

public partial class DicomGLViewer : UserControl
{
    private DicomGLState? glState;

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
        

        glState = new DicomGLState();
    }

    public float CurrentDepth { get; set; } = 0.0f;
    //public OpenGL? GL { get; set; }

    public void UploadDicom(DicomManager dicomMng) => glState?.LoadDicomTexture(dicomMng);

    private void openTkControl_Render(TimeSpan obj)
    {
        GL.ClearColor(0, 0, 0, 1);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        glState?.DrawVertices(CurrentDepth);
    }
}