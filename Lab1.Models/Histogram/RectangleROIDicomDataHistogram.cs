namespace Lab1.Models.Histogram;

public class RectangleROIDicomDataHistogram : RectangleROIHistogram<short>
{
    //private double [,] currentSlice;
    private short [,,] data;

    private IDicomData dicomData;
    private int sliceNumber;

    public RectangleROIDicomDataHistogram(IDicomData dicomData, int sliceNumber)
    {
        DicomData = dicomData;
        SliceNumber = sliceNumber;
    }

    public IDicomData DicomData
    {
        get => dicomData; set
        {
            dicomData = value;

            data = DicomTextureCaster.CastTo3DArray<short>(value);
        }
    }

    public int SliceNumber
    {
        get => sliceNumber; set
        {
            if (value >= 0 || value < DicomData.Depth)
            {
                sliceNumber = value;
            }
        }
    }

    public override IReadOnlyDictionary<short, int> GetHistogram()
    {
        return new Histogram<short>(PixelsInRegion()).GetHistogram();
    }

    public override IReadOnlyCollection<short> PixelsInRegion()
    {
        List<short> resultList = new();
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                if (Region.Contains(new(i, j)))
                    resultList.Add(data [i, j, sliceNumber]);
            }
        }
        return resultList;
    }
}