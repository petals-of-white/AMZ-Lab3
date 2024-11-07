using System.Drawing;
using FellowOakDicom.Imaging.Mathematics;
using Lab1.Models.Tools.ROI;
using SharpGL;

namespace Lab1.Views.Tools.ROI;

public class RectangleROITool(OpenGL gl) : ROITool(gl)
{
    public override uint PrimitiveType => OpenGL.GL_LINE_STRIP;
    public RectangleROI Tool { get; } = new();

    protected override Point2D [] Contour =>
        Tool.Region is Models.Shapes.Rectangle { P1: PointF { X: var x1, Y: var y1 }, P2: PointF { X: var x2, Y: var y2 } }
        ? [new(x1, y1), new(x1, y2), new(x2, y2), new(x2, y1)]
        : Array.Empty<Point2D>();

    protected override Point2D [] ReferencePoints => Contour;
}