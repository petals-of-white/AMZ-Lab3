using Lab1.Models.Shapes;

namespace Lab1.Models.Tools.ROI;

public class RectangleROI
{
    public Rectangle? Region { get; set; }
    public bool IsSelected { get; set; } = false;
    public bool IsActive { get; set; } = false;
}
