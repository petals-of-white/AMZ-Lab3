using FellowOakDicom.Imaging.Mathematics;
using Lab1.Views.Colors;
using Lab1.Views.Graphics;
using SharpGL;

namespace Lab1.Views.Tools.ROI;

public abstract class ROITool : IOverlayTool
{
    private readonly RegionOfInterestGL roiGL;

    public ROITool(OpenGL gl)
    {
        //ViewImage = img;;
        roiGL = new(gl)
        {
            LineColor = new RGBA<float> { R = 1, G = 1, B = 0, A = 1 },
            ReferencePointColor = new RGBA<float> { R = 1, G = 0, B = 0, A = 1 },
        };
    }

    public bool IsROIActive { get; protected set; }

    public bool IsShown { get; protected set; }

    public abstract uint PrimitiveType { get; }

    public virtual string ToolName => "Зона дослідження";

    //public DicomGLViewer ViewImage { get; }
    protected abstract Point2D [] Contour { get; }

    protected abstract Point2D [] ReferencePoints { get; }

    public void Hide()
    {
        IsShown = false;
    }

    public void Show()
    {
        DisplayRegion();
        //DisplayInfo();
        IsShown = true;
    }

    public void UploadPoints()
    {
        roiGL.ReferencePoints = ReferencePoints;
        roiGL.RegionContour = Contour;
    }

    protected void DisplayInfo() => throw new NotImplementedException();

    protected void DisplayRegion() => roiGL.DrawRegionContour(PrimitiveType);
}