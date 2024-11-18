using Lab1.Models.Shapes;

namespace Lab1.ViewModels.Tools.ROI;

public record class RectangleRegion(Rectangle Region, bool IsActive, bool IsDisplayed);