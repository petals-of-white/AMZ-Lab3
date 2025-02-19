﻿namespace Lab1.Models.Shapes;

using System.Drawing;

public record struct Rectangle(PointF P1, PointF P2)
{
    public readonly float Height => Math.Abs(P1.Y - P2.Y);
    public readonly float Width => Math.Abs(P1.X - P2.X);
    public readonly float Area => Height * Width;
    public readonly float Perimeter => Height * 2 + Width * 2;
    public readonly bool Contains(PointF P)
    {
        float
            minX = Math.Min(P1.X, P2.X), maxX = Math.Max(P1.X, P2.X),
            minY = Math.Min(P1.Y, P2.Y), maxY = Math.Max(P1.Y, P2.Y);

        float px = P.X, py = P.Y;

        return px >= minX && px <= maxX && py >= minY && py <= maxY;
    }
}