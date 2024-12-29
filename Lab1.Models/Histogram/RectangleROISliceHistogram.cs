using Lab1.Models.Shapes;

namespace Lab1.Models.Histogram;

public class RectangleROISliceHistogram<TPixel> : RectangleROIHistogram<TPixel> where TPixel : notnull
{
    public RectangleROISliceHistogram(TPixel [,] slice)
    {
        Data = slice;
    }
    public RectangleROISliceHistogram(TPixel [,] slice, Rectangle region)
    {
        Region = region;
        Data = slice;
    }

    public TPixel [,] Data { get; }

    public override IReadOnlyDictionary<TPixel, int> GetHistogram()
    {
        return new Histogram<TPixel>(PixelsInRegion()).GetHistogram();
    }

    public override IReadOnlyCollection<TPixel> PixelsInRegion()
    {
        List<TPixel> resultList = new();
        for (int i = 0; i < Data.GetLength(0); i++)
        {
            for (int j = 0; j < Data.GetLength(1); j++)
            {
                if (Region.Contains(new(i, j)))
                    resultList.Add(Data [i, j]);
            }
        }
        return resultList;
    }
}