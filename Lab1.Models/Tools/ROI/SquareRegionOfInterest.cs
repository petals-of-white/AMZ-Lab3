using Lab1.Models.Shapes;

namespace Lab1.Models.Tools.ROI;

public record class SquareRegionOfInterest (Square Region) : IHistogram<byte>
{
    public IReadOnlyDictionary<byte, int> GetHistogram()
    {
        throw new NotImplementedException();
    }

}
