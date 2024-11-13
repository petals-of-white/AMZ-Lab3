using Lab1.Models.Shapes;
using WPF = System.Windows;

namespace Lab1.Views.Extensions;

public static class RectangleWPFExtensions
{
    public static double Left(this Rectangle rect) => rect.TopLeft().X;

    public static double Top(this Rectangle rect) => rect.TopLeft().Y;

    public static WPF.Point TopLeft(this Rectangle rect) => rect switch
    {
        Rectangle { P1: var p1, P2: var p2 } => new(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y))
    };
}