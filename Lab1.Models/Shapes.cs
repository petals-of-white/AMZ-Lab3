namespace Lab1.Models.Shapes;

using System.Drawing;

public struct Ellipse
{
    public PointF Center { get; set; }
    public PointF R1 { get; set; }
    public PointF R2 { get; set; }
}

public struct Rectangle
{
    public PointF P1 { get; set; }
    public PointF P2 { get; set; }
}