using Lab1.Models.Tools.Shapes;

namespace Lab1.Models.Tools.ROI;

public record class RectangleROITool(Rectangle Rect, bool IsSelected, bool IsActive) : IROI;
