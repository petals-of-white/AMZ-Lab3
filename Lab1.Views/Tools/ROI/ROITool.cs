using System.Drawing;
using Lab1.Models.Tools;
using Lab1.Views.Colors;
using Lab1.Views.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Lab1.Views.Tools.ROI;

public abstract class ROITool
{
    private readonly RegionOfInterestGL roiGL;

    public ROITool()
    {
        roiGL = new()
        {
            LineColor = new RGBA<float> { R = 1, G = 1, B = 0, A = 1 },
            ReferencePointColor = new RGBA<float> { R = 1, G = 0, B = 0, A = 1 },
        };
    }

    public abstract bool IsDisplayed { get; }

    public abstract PrimitiveType PrimitiveType { get; }

    public virtual string ToolName => "Зона дослідження";

    protected abstract PointF [] Contour { get; }

    protected abstract PointF [] ReferencePoints { get; }

    public void Draw()
    {
        DisplayRegion();
        //DisplayInfo();
    }

    public void UploadPoints()
    {
        roiGL.ReferencePoints = ReferencePoints;
        roiGL.RegionContour = Contour;
    }

    protected void DisplayInfo() => throw new NotImplementedException();

    protected void DisplayRegion()
    {
        roiGL.DrawRegionContour(PrimitiveType);
        roiGL.DrawReferencePoints();
    }
}