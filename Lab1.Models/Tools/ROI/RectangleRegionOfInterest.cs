using Lab1.Models.Shapes;

namespace Lab1.Models.Tools.ROI;

public record class RectangleRegionOfInterest(Rectangle Region, double PixelWidth, double PixelHeight) : IRegionOfInterestInfo
{
    public int NumberOfPixels
    {
        get
        {
            Rectangle roundedRectangle = Region switch
            {
                {
                    P1: { X: var x1, Y: var y1 },
                    P2: { X: var x2, Y: var y2 }
                } =>

                    new Rectangle(
                        new(float.Round(x1), float.Round(y1)),
                        new(float.Round(x2), float.Round(y2))
                    )
            };

            return Convert.ToInt32(roundedRectangle.Area);
        }
    }
    public double Area => (Region.Height * PixelHeight) * (Region.Width * PixelWidth);
}
//public abstract record class RectangleRegionOfInterest(Rectangle Region) : IRegionOfInterestInfo
//{
//    public abstract double PixelWidth { get; }
//    public abstract double PixelHeight { get; }
//    public int NumberOfPixels
//    {
//        get
//        {
//            Rectangle roundedRectangle = Region switch
//            {
//                {
//                    P1: { X: var x1, Y: var y1 },
//                    P2: { X: var x2, Y: var y2 }
//                } =>

//                    new Rectangle(
//                        new(float.Round(x1), float.Round(y1)),
//                        new(float.Round(x2), float.Round(y2))
//                    )
//            };

//            return Convert.ToInt32(roundedRectangle.Area);
//        }
//    }
//    public double Area => (Region.Height * PixelHeight) * (Region.Width * PixelWidth);
//}