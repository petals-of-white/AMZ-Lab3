using System.Drawing;
using Lab1.Models.Tools.ROI;
using OpenTK.Graphics.OpenGL;

namespace Lab1.Views.Tools.ROI;

public class RectangleROITool : ROITool
{
    public override bool IsDisplayed => Tool.IsDisplayed;
    public override PrimitiveType PrimitiveType => PrimitiveType.LineStrip;
    public RectangleROI Tool { get; set; } = new();

    protected override PointF [] Contour =>
        Tool.Region is Models.Shapes.Rectangle
        {
            P1: PointF { X: var x1, Y: var y1 },
            P2: PointF { X: var x2, Y: var y2 }
        }
        ? [new(x1, y1), new(x1, y2), new(x2, y2), new(x2, y1)]
        : Array.Empty<PointF>();

    protected override PointF [] ReferencePoints => Contour;
}