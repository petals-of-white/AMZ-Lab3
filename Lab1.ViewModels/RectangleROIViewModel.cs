using Lab1.Models.Histogram;
using Lab1.Models.Shapes;

namespace Lab1.ViewModels;

public class RectangleROIViewModel : ROIViewModel
{
    private readonly RectangleROIDicomDataHistogram histogram;
    //private Rectangle region;

    public RectangleROIViewModel(System.Drawing.PointF startPoint,
        RectangleROIDicomDataHistogram histogram)

    {
        this.histogram = histogram;
        Region = new Rectangle(startPoint, startPoint);
    }

    public override IHistogram<short> Histogram => histogram;

    public Rectangle Region
    {
        get => histogram.Region; private set
        {
            histogram.Region = value;
            NotifyPropertyChanged(nameof(Region));
            NotifyPropertyChanged(nameof(Histogram));
        }
    }

    public int SliceNumber
    {
        get => histogram.SliceNumber; set
        {
            histogram.SliceNumber = value;
            NotifyPropertyChanged(nameof(SliceNumber));
        }
    }

    protected override void SetPoint(System.Drawing.PointF point)
    {
        //if (IsActive)
        //{
        Region = Region with { P2 = point };
        //}
    }
}