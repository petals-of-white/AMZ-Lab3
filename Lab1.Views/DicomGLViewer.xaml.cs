using System.Windows.Controls;
using Lab1.Models;
using Lab1.Views.Graphics;
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
    public SharpGL.OpenGL? GL { get; set; }

    public void UploadDicom(DicomManager dicomMng) => glState?.LoadDicomTexture(dicomMng);

    private void DicomGLViewer_OpenGLDraw(object sender, OpenGLRoutedEventArgs args) => glState?.DrawVertices(CurrentDepth);

    private void DicomGLViewer_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        GL = args.OpenGL;
        glState = new DicomGLState(GL);
    }
}