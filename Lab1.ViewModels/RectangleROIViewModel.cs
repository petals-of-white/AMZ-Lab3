using Lab1.Models.Shapes;
using Lab1.ViewModels.Tools.ROI;

namespace Lab1.ViewModels;

public abstract class RectangleROIViewModel : ROIViewModel
{
    private readonly System.Drawing.PointF startPoint;
    private Rectangle region;

    public RectangleROIViewModel(System.Drawing.PointF startPoint)
    {
        this.startPoint = startPoint;
        Region = new Rectangle(startPoint, startPoint);
    }
    public Rectangle Region
    {
        get => region; private set
        {
            region = value;

            NotifyPropertyChanged(nameof(Region));
        }
    }

    public RectangleROIViewModel(Rectangle region)
    {
        Region = region;
    }

    //protected override void ToggleROI()
    //{
    //    IsShown = !IsShown;
    //    //var roi = (SelectedROI ??= new RectangleRegion(new(), false, true));
    //    //SelectedROI = roi with { IsDisplayed = !roi.IsDisplayed };
    //    //NotifyPropertyChanged(nameof(IsShown));
    //    //NotifyPropertyChanged(nameof(ROIInfo));
    
    //}
    protected override void SetPoint(System.Drawing.PointF point)
    {
        if (IsActive)
        {
            Region = Region with { P2 = point };

        }
    }
}
