namespace Lab1.Views.Tools.ROI;

using FellowOakDicom.Media;
using Lab1.Views.Tools;


public abstract class ROITool(DicomGLViewer img) : IOverlayTool
{
    public DicomGLViewer ViewImage { get; set; } = img;
    public bool IsActivated { get; private set; }
    public bool IsROIActive { get; private set; }
    public string ToolName => "Зона дослідження";
    public void DisplayInfo()
    {
        throw new NotImplementedException();
    }
    public abstract void DisplayRegion();
    public void Highlight()
    {
        
    }
    public void Activate()
    {
        IsActivated = true;
        throw new NotImplementedException();
    }
    public void Deactivate()
    {
        IsActivated = false;
        throw new NotImplementedException();
    }
}