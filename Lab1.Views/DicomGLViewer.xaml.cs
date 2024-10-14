using System.Windows;
using System.Windows.Controls;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Mathematics;
using FellowOakDicom.Imaging.Render;
using FellowOakDicom.Media;
using Lab1.Views.Graphics;
using SharpGL.WPF;

namespace Lab1.Views;

public partial class DicomGLViewer : UserControl
{
    private DicomFile []? dicoms;
    private DicomGLState? glState;

    public static readonly DependencyProperty DicomsProperty =
        DependencyProperty.Register("Dicoms", typeof(DicomFile []), typeof(DicomGLViewer), new PropertyMetadata(null));

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
    //        if (value is not null) AddDicom(value);
    //    }
    //}

    private void RenderDicom()
    {
        throw new NotImplementedException();
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

        SetValue(DicomsProperty, dicomFiles);
        //dicoms = dicomFiles;

        RenderDicom();


        //int rows, columns;
        //float pixelSpacing, spacingBetweenSlices, sliceThickness;

        //List<string> errors = [];
        //string errorCaption = "Помилка!";
        //var msgBoxImg = MessageBoxImage.Error;
        //var okBtn = MessageBoxButton.OK;

        //if (!ds.TryGetSingleValue(DicomTag.Rows, out rows)) errors.Add("Не задано кількість рядків");
        //if (!ds.TryGetSingleValue(DicomTag.Columns, out columns)) errors.Add("Не задано кількість стовпців");
        //if (!ds.TryGetSingleValue(DicomTag.PixelSpacing, out pixelSpacing)) errors.Add("Не задано PixelSpacing");
        //if (!ds.TryGetSingleValue(DicomTag.SpacingBetweenSlices, out spacingBetweenSlices)) errors.Add("Не задано SpacingBetweenSlides");
        //if (!ds.TryGetSingleValue(DicomTag.SliceThickness, out sliceThickness)) errors.Add("Не задано SliceThickness");

        //if (errors.Count > 0)
        //{
        //    MessageBox.Show(string.Join(Environment.NewLine, errors), errorCaption, okBtn, msgBoxImg);
        //}
        //else
        //{

        //    RenderDicom();
        //}
    }

    private void DicomGLViewer_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {

    }

    private void DicomGLViewer_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        glState = new DicomGLState(args.OpenGL);
    }

}
