using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Models.Histogram;

public interface IHistogram<TPixel>
{
    IReadOnlyDictionary<TPixel, int> GetHistogram();
}
