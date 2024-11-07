namespace Lab1.Views.Tools.ROI;
public abstract class ROITool(DicomGLViewer img) : IOverlayTool
{
    public DicomGLViewer ViewImage { get; } = img;
    public bool IsShown { get; protected set; }
    public bool IsROIActive { get; protected set; }
    public virtual string ToolName => "Зона дослідження";
    protected abstract void DisplayInfo();
    protected abstract void DisplayRegion();
    public void Show()
    {
        DisplayRegion();
        DisplayInfo();
        IsShown = true;
    }
    public void Hide()
    {
        IsShown = false;
        throw new NotImplementedException();
    }
}