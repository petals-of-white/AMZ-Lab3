namespace Lab1.Views.Tools;

public interface IOverlayTool
{
    /// <summary>
    /// Deactivate and hide tool overlay
    /// </summary>
    void Deactivate();

    /// <summary>
    /// Activate and draw tool overlay
    /// </summary>
    void Activate();
    string ToolName { get; }
    bool IsActivated { get; }
}
