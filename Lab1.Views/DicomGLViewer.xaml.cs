using System.Windows;
using System.Windows.Controls;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Mathematics;
using FellowOakDicom.Imaging.Render;
using FellowOakDicom.Media;
using Lab1.Models;
using Lab1.Views.Graphics;
using SharpGL.WPF;

namespace Lab1.Views;

public partial class DicomGLViewer : UserControl
{
    public DicomGLViewer()
    {
        InitializeComponent();
    }
    private DicomFile []? dicoms;
    private DicomGLState? glState;

    //public static readonly DependencyProperty DicomsProperty =
    //    DependencyProperty.Register(
    //        name: nameof(Dicoms),
    //        propertyType: typeof(DicomFile []),
    //        ownerType: typeof(DicomGLViewer),
    //        typeMetadata: new FrameworkPropertyMetadata(defaultValue: Array.Empty<DicomFile>()));

    public DicomFile []? Dicoms
    {
        get => dicoms;
        set { if (value is not null) AddDicom(value); }
    }

    //public DicomFile []? Dicoms
    //{
    //    get => (DicomFile []) GetValue(DicomsProperty);
    //    set
    //    {
    //        SetValue(DicomsProperty, value);
    //        if (value is not null) AddDicom(value);
    //    }
    //}

    private void RenderDicom()
    {
        if (dicoms is not null)
        {
            var dicomMng = new DicomManager(dicoms);
            glState?.LoadDicomTexture(dicomMng);
        }
    }

    public void RenderText(string text, int fontSize, Point2D topLeft, int width, int height, Color32 color)
    {
        throw new NotImplementedException();
    }

    public void AddDicom(DicomFile [] dicomFiles)
    {
        var ds = dicomFiles [0].Dataset;
        var pixelData = DicomPixelData.Create(ds);

        Width = pixelData.Width;
        Height = pixelData.Height;


        dicoms = dicomFiles;

        RenderDicom();
    }

    private void DicomGLViewer_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        glState?.DrawVertices();
    }

    private void DicomGLViewer_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        glState = new DicomGLState(args.OpenGL);
    }

}
