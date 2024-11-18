using FellowOakDicom.Imaging;

namespace Lab1.Models;

public interface IDicomData : IReadOnlyList<byte>
{
    BitDepth BitDepth { get; }
    AnatomicPlane DefaultPlane { get; }
    int Depth { get; }
    ushort Height { get; }
    PhotometricInterpretation PhotometricInterpretation { get; }
    PixelRepresentation PixelRepresentation { get; }
    (double VerticalSpacing, double HorizontalSpacing) PixelSpacing { get; }
    ushort Width { get; }
}