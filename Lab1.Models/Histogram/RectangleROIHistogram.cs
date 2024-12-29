using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Models.Shapes;

namespace Lab1.Models.Histogram;

public abstract class RectangleROIHistogram<TPixel> : IHistogram<TPixel>
{
    public Rectangle Region { get; set; }

    public abstract IReadOnlyDictionary<TPixel, int> GetHistogram();
    public abstract IReadOnlyCollection<TPixel> PixelsInRegion();
}