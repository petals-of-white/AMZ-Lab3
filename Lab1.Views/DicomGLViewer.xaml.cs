using System.Windows.Controls;
using Lab1.Models;
using Lab1.Views.Graphics;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph;
using SharpGL.WPF;

namespace Lab1.Views;

public partial class DicomGLViewer : UserControl
{
    private DicomGLState? glState;

    // TODO: Introduce ViewModel for DicomManager
    public DicomGLViewer()
    {
        InitializeComponent();

    }

    public float CurrentDepth { get; set; } = 0.0f;
    public OpenGL? GL { get; set; }

    public void UploadDicom(DicomManager dicomMng) => glState?.LoadDicomTexture(dicomMng);

    private void DicomGLViewer_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        var gl = args.OpenGL;

        while (gl.GetErrorCode() is not ErrorCode.NoError);

        GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
        glState?.DrawVertices(CurrentDepth);

    }

    private void DicomGLViewer_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        GL = args.OpenGL;
        GL.ClearColor(0.0f, 0.3f, 0.5f, 1f);
        


        glState = new DicomGLState(GL);
    }
}