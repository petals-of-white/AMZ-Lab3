using Lab1.Models;

namespace Lab1.Views;

public class CoordsPixelLength
{
    public CoordsPixelLength(IDicomData dicomData)
    {
        switch (dicomData.DefaultPlane)
        {
            case AnatomicPlane.Axial:
                XPixels = dicomData.Width;
                YPixels = dicomData.Height;
                ZPixels = (uint) dicomData.Depth;
                break;

            case AnatomicPlane.Coronal:
                XPixels = dicomData.Width;
                YPixels = (uint) dicomData.Depth;
                ZPixels = dicomData.Height;
                break;

            case AnatomicPlane.Saggital:
                XPixels = (uint) dicomData.Depth;
                YPixels = dicomData.Height;
                ZPixels = dicomData.Width;
                break;
        }
    }

    public uint XPixels { get; private init; }
    public uint YPixels { get; private init; }
    public uint ZPixels { get; private init; }
}