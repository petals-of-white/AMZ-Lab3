using System.Windows.Controls;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Mathematics;
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

    public void RenderText(string text, int fontSize, Point2D topLeft, int width, int height, Color32 color)
    {
        throw new NotImplementedException();
    }

    public void UploadDicom(DicomManager dicomMng) => glState?.LoadDicomTexture(dicomMng);

    private void DicomGLViewer_OpenGLDraw(object sender, OpenGLRoutedEventArgs args) => glState?.DrawVertices(CurrentDepth);

    private void DicomGLViewer_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        GL = args.OpenGL;
        glState = new DicomGLState(args.OpenGL);
    }
}