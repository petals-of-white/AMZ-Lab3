namespace Lab1.Models.Shapes;

using System.Drawing;

public record struct Ellipse(PointF Center, PointF R1, PointF R2);

public record struct Rectangle(PointF P1, PointF P2)
{
    public readonly float Height => Math.Abs(P1.Y - P2.Y);
    public readonly float Width => Math.Abs(P1.X - P2.X);
}