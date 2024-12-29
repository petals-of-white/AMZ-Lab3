using System.Collections;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.IO.Buffer;

namespace Lab1.Models;

public class DicomManager : IDicomData
{
    private readonly List<byte> bytes;
    private readonly DicomDataset dataset;
    private readonly DicomPixelData pixelData;

    public DicomManager(DicomFile dicomFile)
    {
        var ds = dicomFile.Dataset;
        var pixData = DicomPixelData.Create(ds);
        pixelData = pixData;
        dataset = ds;
        bytes = new List<byte>(pixData.GetFrame(0).Data);
        Depth = 1;
    }

    public DicomManager(IReadOnlyList<DicomFile> dicomFiles)
    {
        switch (dicomFiles)
        {
            case ([var first, ..]):

                var ds = first.Dataset;
                dataset = ds;
                var pxData = DicomPixelData.Create(ds);

                pixelData = pxData;
                bytes = new List<byte>(pxData.GetFrame(0).Data);

                Depth = 1;

                dicomFiles.Skip(1).
                      SelectMany(file => EnumerateFrames(DicomPixelData.Create(file.Dataset))).
                      Each(pxData => { bytes.AddRange(pxData.Data); Depth++; });

                break;

            default:
                throw new ArgumentException("dicomFiles should contain at least one file.", nameof(dicomFiles));
        }
    }

    public BitDepth BitDepth => pixelData.BitDepth;
    public int Count => bytes.Count;
    public AnatomicPlane DefaultPlane => AnatomicPlane.Axial;
    public int Depth { get; private set; }
    public ushort Height => pixelData.Height;
    public PhotometricInterpretation PhotometricInterpretation => pixelData.PhotometricInterpretation;
    public PixelRepresentation PixelRepresentation => pixelData.PixelRepresentation;

    public (double VerticalSpacing, double HorizontalSpacing) PixelSpacing
    {
        get
        {
            double [] spacing = dataset.GetValues<double>(DicomTag.PixelSpacing);
            return (spacing [0], spacing [1]);
        }
    }

    public ushort Width => pixelData.Width;
    public byte this [int index] => bytes [index];

    public static DicomManager FromDicomFolder(string dicomFolder)
    {
        return FromFiles(Directory.EnumerateFiles(dicomFolder, "*.dcm"));
    }

    public static DicomManager FromFile(string file) => new(DicomFile.Open(file));

    public static DicomManager FromFiles(IEnumerable<string> files) => new(files.Order().Select((f) => DicomFile.Open(f)).ToArray());

    public IEnumerator<byte> GetEnumerator() => bytes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => bytes.GetEnumerator();

    private static IEnumerable<IByteBuffer> EnumerateFrames(DicomPixelData pixelData)
    {
        for (var i = 0; i < pixelData.NumberOfFrames; i++)
        {
            yield return pixelData.GetFrame(i);
        }
    }
}