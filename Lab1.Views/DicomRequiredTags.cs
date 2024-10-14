using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Render;
using Lab1.ExtensionMethods;
namespace Lab1.Views;

public record class DicomRequiredTags
{
    public DicomRequiredTags(DicomDataset dcm)
    {
        Rows = dcm.TryMaybeValue<int>(DicomTag.Rows);
        Columns = dcm.TryMaybeValue<int>(DicomTag.Columns);

        PixelSpacing = dcm.TryMaybeValue<int>(DicomTag.PixelSpacing);
        SpacingBetweenSlices = dcm.TryMaybeValue<float>(DicomTag.SpacingBetweenSlices);
        SliceThickness = dcm.TryMaybeValue<float>(DicomTag.SliceThickness);

        PhotometricInterpetation = dcm.TryMaybeValue<string>(DicomTag.PhotometricInterpretation);
        RescaleSlope = dcm.TryMaybeValue<float>(DicomTag.RescaleSlope);
        RescaleIntercept = dcm.TryMaybeValue<float>(DicomTag.RescaleIntercept);

        PixelRepresentation = dcm.TryMaybeValue<byte>(DicomTag.PixelRepresentation);
        PixelData = dcm.TryMaybeValue<byte []>(DicomTag.PixelData);


        BitsAllocated = dcm.TryMaybeValue<int>(DicomTag.BitsAllocated);
        BitsStored = dcm.TryMaybeValue<int>(DicomTag.BitsStored);
        HighBit = dcm.TryMaybeValue<int>(DicomTag.HighBit);
        var a = DicomPixelData.Create(dcm);
    }

    public int? Rows { get; private set; }
    public int? Columns { get; private set; }
    public float? PixelSpacing { get; private set; }
    public float? SpacingBetweenSlices { get; private set; }
    public float? SliceThickness { get; private set; }
    public string? PhotometricInterpetation { get; private set; }
    public float? RescaleSlope { get; private set; }
    public float? RescaleIntercept { get; private set; }
    public byte? PixelRepresentation { get; private set; }
    public int? BitsAllocated { get; private set; }
    public int? BitsStored { get; private set; }
    public int? HighBit { get; private set; }
    public byte []? PixelData { get; private set; }
}


