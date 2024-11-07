namespace Lab1.Views.Colors;

public struct RGBA<Component> where Component : struct
{
    public Component R { get; set; }
    public Component G { get; set; }
    public Component B { get; set; }
    public Component A { get; set; }
}
