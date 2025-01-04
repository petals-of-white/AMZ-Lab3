using Lab1.Models.Histogram;
using Lab1.Models.Shapes;

namespace Lab1.ViewModels;

public class RectangleROIViewModel : ROIViewModel
{
    private readonly RectangleROIDicomDataHistogram histogram;
    private System.Drawing.PointF lastPoint;

    public RectangleROIViewModel(System.Drawing.PointF startPoint,
        RectangleROIDicomDataHistogram histogram)

    {
        this.histogram = histogram;
        lastPoint = startPoint;
        Region = new Rectangle(startPoint, startPoint);
    }

    public RectangleROIViewModel(Rectangle region, RectangleROIDicomDataHistogram histogram)
    {
        this.histogram = histogram;
        lastPoint = region.P2;
        Region = region;
    }

    public Rectangle Region
    {
        get => histogram.Region; private set
        {
            histogram.Region = value;
            NotifyPropertyChanged(nameof(Region));
            NotifyPropertyChanged(nameof(SelectedPixels));
        }
    }

    public override IReadOnlyCollection<short> SelectedPixels => histogram.PixelsInRegion();

    public int SliceNumber
    {
        get => histogram.SliceNumber; set
        {
            histogram.SliceNumber = value;
            NotifyPropertyChanged(nameof(SliceNumber));
            NotifyPropertyChanged(nameof(SelectedPixels));
        }
    }

    protected override void SetPoint(System.Drawing.PointF point)
    {
        Region = new Rectangle(lastPoint, point);
        lastPoint = point;
    }
}