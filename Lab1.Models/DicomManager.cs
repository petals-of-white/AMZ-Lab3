using System;
using System.Collections.Immutable;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.IO.Buffer;

namespace Lab1.Models;

public class DicomManager {

    public ImmutableArray<IByteBuffer> RawFrames { get; private set; }
    public BitDepth BitDepth { get; private set; }
    public ushort Height { get; private set; }
    public ushort Width { get; private set; }
    public PhotometricInterpretation PhotometricInterpretation { get; private set; }
    public PixelRepresentation PixelRepresentation { get; private set; }


    public static IEnumerable<IByteBuffer> EnumerateFrames(DicomFile dicom)
    {
        var ds = dicom.Dataset;
        var pixelData = DicomPixelData.Create(ds);

        List<IByteBuffer> buffers = new(pixelData.NumberOfFrames);
        for (var i = 0; i < pixelData.NumberOfFrames; i++)
        {
            yield return pixelData.GetFrame(i);
        }
    }
    public DicomManager(DicomFile dicomFile)
    {
        var ds = dicomFile.Dataset;
        var pixelData = DicomPixelData.Create(ds);

        //List<IByteBuffer> buffers = new(pixelData.NumberOfFrames);
        //for (var i = 0; i < pixelData.NumberOfFrames; i++)
        //{
        //    buffers.Add(pixelData.GetFrame(i));
        //}

        RawFrames = EnumerateFrames(dicomFile).ToImmutableArray();
        BitDepth = pixelData.BitDepth;
        Height = pixelData.Height;
        Width = pixelData.Width;
        PhotometricInterpretation = pixelData.PhotometricInterpretation;
        PixelRepresentation = pixelData.PixelRepresentation;
    }

    private bool SamePatient (DicomManager dcm1, DicomManager dcm2)
    {
        throw new NotImplementedException();
        //if (dcm1.Width == dcm2.Width &&  dcm1.Height == dcm2.Height && dcm1.)
    }
    public DicomManager(IEnumerable<DicomFile> dicomFiles)
    {
        throw new NotImplementedException();
    }


    public static DicomManager FromFile(string file) => throw new NotImplementedException();
    public static DicomManager FromFiles(IEnumerable<string> files) => throw new NotImplementedException();
    public static DicomManager FromDicomFolder(string dicomFolder) => throw new NotImplementedException();
}
