using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Models.Histogram;

public class Histogram<TPixel>(IReadOnlyCollection<TPixel> pixels) : IHistogram<TPixel> where TPixel : notnull
{
    public IReadOnlyDictionary<TPixel, int> GetHistogram() =>
        pixels.GroupBy(pixel => pixel).ToDictionary(group => group.Key, group => group.Count());
}

