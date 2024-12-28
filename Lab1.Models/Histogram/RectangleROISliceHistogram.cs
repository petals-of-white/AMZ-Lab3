using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Models.Shapes;

namespace Lab1.Models.Histogram;

public class RectangleROISliceHistogram<TPixel> : From2DArrayHistogram<TPixel> where TPixel : notnull
{
    public Rectangle Region { get; set; }
    public RectangleROISliceHistogram(TPixel [,] slice, Rectangle region) : base(slice)
    {
        Region = region;
    }

    public override IReadOnlyDictionary<TPixel, int> GetHistogram()
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
        return new Histogram<TPixel>(resultList).GetHistogram();
    }
}
