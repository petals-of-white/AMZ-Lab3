using FellowOakDicom.Imaging;
using Lab1.Models;
using static SharpGL.OpenGL;

namespace Lab1.Views;
public class DicomToGLConverter
{
    private readonly DicomManager dicomManager;

    public uint InternalFormat => dicomManager.PhotometricInterpretation.Value switch
    {

        var i when i == PhotometricInterpretation.Rgb.Value => GL_RGB,
        var i when i == PhotometricInterpretation.Monochrome2.Value => GL_RED,
        _ => throw new NotImplementedException("Other photometric interpretation is not implemented yet.")
    };

    public int Width => dicomManager.Width;
    public int Height => dicomManager.Height;
    public int Depth => dicomManager.Depth;
    public uint Format => dicomManager.PhotometricInterpretation.Value switch
    {

        var i when i == PhotometricInterpretation.Rgb.Value => GL_RGB,
        var i when i == PhotometricInterpretation.Monochrome2.Value => GL_RED,
        _ => throw new NotImplementedException("Other photometric interpretation is not implemented yet.")
    };
    public uint Type => dicomManager.BitDepth switch
    {
        { BitsStored: 8, IsSigned: true } => GL_BYTE,
        { BitsStored: 8, IsSigned: false } => GL_UNSIGNED_BYTE,
        { BitsStored: 16, IsSigned: true } => GL_SHORT,
        { BitsStored: 16, IsSigned: false } => GL_UNSIGNED_SHORT,
        { BitsStored: 32, IsSigned: true } => GL_INT,
        { BitsStored: 32, IsSigned: false } => GL_UNSIGNED_INT,
        _ => throw new NotImplementedException("Other bit depth is not implemented")
    };
    public DicomToGLConverter(DicomManager dicom) => dicomManager = dicom;
}
