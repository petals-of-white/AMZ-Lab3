using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.IO.Buffer;

namespace Lab1.Models;

public class DicomManager
{
    private DicomPixelData pixelData;
    public IReadOnlyCollection<IByteBuffer> RawFrames => EnumerateFrames(pixelData).ToArray();
    public BitDepth BitDepth { get; private set; }
    public ushort Height { get; private set; }
    public ushort Width { get; private set; }
    public int Depth => pixelData.NumberOfFrames;
    public PhotometricInterpretation PhotometricInterpretation { get; private set; }
    public PixelRepresentation PixelRepresentation { get; private set; }

    private static IEnumerable<IByteBuffer> EnumerateFrames(DicomPixelData pixelData)
    {
        for (var i = 0; i < pixelData.NumberOfFrames; i++)
        {
            yield return pixelData.GetFrame(i);
        }
    }
    public DicomManager(DicomFile dicomFile)
    {
        var ds = dicomFile.Dataset;
        var pixelData = DicomPixelData.Create(ds);

        this.pixelData = pixelData;

        BitDepth = pixelData.BitDepth;
        Height = pixelData.Height;
        Width = pixelData.Width;
        PhotometricInterpretation = pixelData.PhotometricInterpretation;
        PixelRepresentation = pixelData.PixelRepresentation;
    }

    private bool SamePatient(DicomManager dcm1, DicomManager dcm2)
    {
        throw new NotImplementedException();
    }
    public DicomManager(IReadOnlyList<DicomFile> dicomFiles)
    {
        switch (dicomFiles)
        {

            case ([var first, ..]):

                var ds = first.Dataset;
                var pxData = DicomPixelData.Create(ds);

                pixelData = pxData;

                BitDepth = pixelData.BitDepth;
                Height = pixelData.Height;
                Width = pixelData.Width;
                PhotometricInterpretation = pixelData.PhotometricInterpretation;
                PixelRepresentation = pixelData.PixelRepresentation;

                dicomFiles.Skip(1).
                    SelectMany((file) => EnumerateFrames(DicomPixelData.Create(file.Dataset))).
                    Each((pxData) => pixelData!.AddFrame(pxData));


                break;
            default:
                throw new ArgumentException("dicomFiles should contain at leasst one file.", nameof(dicomFiles));
        }

    }


    public static DicomManager FromFile(string file) => new(DicomFile.Open(file));
    public static DicomManager FromFiles(IEnumerable<string> files) => new(files.Select((f) => DicomFile.Open(f)).ToArray());
    public static DicomManager FromDicomFolder(string dicomFolder)
    {
        return FromFiles(Directory.EnumerateFiles("*.dcm"));

    }
}
