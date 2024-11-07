namespace Lab1.Views.Tools;

public interface IOverlayTool
{
    /// <summary>
    /// Deactivate and hide tool overlay
    /// </summary>
    void Hide();

    /// <summary>
    /// Activate and draw tool overlay
    /// </summary>
    void Show();

    string ToolName { get; }
    bool IsShown { get; }
}
