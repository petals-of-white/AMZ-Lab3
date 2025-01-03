using Lab1.Models.Histogram;
using Lab1.Models.Shapes;

namespace Lab1.ViewModels;

public class RectangleROIViewModel : ROIViewModel
{
    private readonly RectangleROIDicomDataHistogram histogram;

    //private Rectangle region;
    private System.Drawing.PointF lastPoint;

    public RectangleROIViewModel(System.Drawing.PointF startPoint,
        RectangleROIDicomDataHistogram histogram)

    {
        this.histogram = histogram;
        lastPoint = startPoint;
        Region = new Rectangle(startPoint, startPoint);
    }

    public override IHistogram<short> Histogram => histogram;

    public Rectangle Region
    {
        get => histogram.Region; private set
        {
            histogram.Region = value;
            NotifyPropertyChanged(nameof(Region));
            NotifyPropertyChanged(nameof(SelectedPixels));
            NotifyPropertyChanged(nameof(Histogram));
        }
    }

    public override IReadOnlyCollection<short> SelectedPixels => Histogram.PixelsInRegion();

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
        //if (IsActive)
        //{
        Region = new Rectangle(lastPoint, point);

        //Region = Region with { P2 = point };
        lastPoint = point;
        //}
    }
}