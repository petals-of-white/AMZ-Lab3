using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Models.Histogram;

public abstract class From2DArrayHistogram<TPixel>(TPixel [,] slice) : IHistogram<TPixel>
{
    public TPixel [,] Data { get; } = slice;
    public abstract IReadOnlyDictionary<TPixel, int> GetHistogram();
}
