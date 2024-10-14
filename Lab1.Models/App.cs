using FellowOakDicom;
using Lab1.Models.Tools.ROI;

namespace Lab1.Models;

public class App
{
    public RectangleROITool? ROI { get; set; }
    public DicomFile []? Dicoms { get; set; }
}
