using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.IO.Buffer;

namespace Lab1.Models;

public class DicomManager
{
    private readonly DicomPixelData pixelData;

    public DicomManager(DicomFile dicomFile)
    {
        var ds = dicomFile.Dataset;
        var pixData = DicomPixelData.Create(ds);

        pixelData = pixData;
    }

    public DicomManager(IReadOnlyList<DicomFile> dicomFiles)
    {
        switch (dicomFiles)
        {
            case ([var first, ..]):

                var ds = first.Dataset;
                var pxData = DicomPixelData.Create(ds);
                

                pixelData = pxData;

                dicomFiles.Skip(1).
                    SelectMany((file) => EnumerateFrames(DicomPixelData.Create(file.Dataset))).
                    Each((pxData) => pixelData!.AddFrame(pxData));

                break;

            default:
                throw new ArgumentException("dicomFiles should contain at least one file.", nameof(dicomFiles));
        }
    }

    public BitDepth BitDepth => pixelData.BitDepth;
    public int Depth => pixelData.NumberOfFrames;
    public ushort Height => pixelData.Height;
    public PhotometricInterpretation PhotometricInterpretation => pixelData.PhotometricInterpretation;
    public PixelRepresentation PixelRepresentation => pixelData.PixelRepresentation;
    public IReadOnlyCollection<IByteBuffer> RawFrames => EnumerateFrames(pixelData).ToArray();
    public ushort Width => pixelData.Width;

    public static DicomManager FromDicomFolder(string dicomFolder)
    {
        return FromFiles(Directory.EnumerateFiles("*.dcm"));
    }

    public static DicomManager FromFile(string file) => new(DicomFile.Open(file));

    public static DicomManager FromFiles(IEnumerable<string> files) => new(files.Select((f) => DicomFile.Open(f)).ToArray());

    private static IEnumerable<IByteBuffer> EnumerateFrames(DicomPixelData pixelData)
    {
        for (var i = 0; i < pixelData.NumberOfFrames; i++)
        {
            yield return pixelData.GetFrame(i);
        }
    }
}