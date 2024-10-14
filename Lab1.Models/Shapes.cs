namespace Lab1.Models.Tools.Shapes;

using System.Drawing;

public struct Ellipse(PointF center, PointF r1, PointF r2)
{
    public PointF Center { get; set; } = center;
    public PointF R1 { get; set; } = r1;
    public PointF R2 { get; set; } = r2;
}

public struct Rectangle(PointF p1, PointF p2)
{
    public PointF P1 { get; } = p1;
    public PointF P2 { get; } = p2;
}