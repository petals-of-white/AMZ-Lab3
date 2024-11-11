namespace Lab1.Models.Shapes;

using System.Drawing;

public record struct Ellipse(PointF Center, PointF R1, PointF R2);

public record struct Rectangle(PointF P1, PointF P2);