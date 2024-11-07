using System.IO;
using FellowOakDicom.Imaging.Mathematics;
using Lab1.Views.Colors;
using SharpGL;
using SharpGL.Shaders;
using SharpGL.VertexBuffers;

namespace Lab1.Views.Graphics;

public class RegionOfInterestGL
{
    private readonly VertexBuffer contourBuffer = new();
    private readonly OpenGL gl;
    private readonly VertexBuffer refPointsBuffer = new();
    private readonly ShaderProgram shaderProgram = new();
    private Point2D [] referencePoints = Array.Empty<Point2D>();
    private Point2D [] regionContour = Array.Empty<Point2D>();

    public RegionOfInterestGL(OpenGL opengl)
    {
        gl = opengl;
        contourBuffer.Create(gl);
        refPointsBuffer.Create(gl);
        CreateProgram();
    }

    public static string FragShaderLoc { get; } = "Shaders/ROI.frag";
    public static string VertShaderLoc { get; } = "Shaders/ROI.vert";
    public RGBA<float> LineColor { get; set; }
    public RGBA<float> ReferencePointColor { get; set; }

    public Point2D [] ReferencePoints
    {
        get => referencePoints;
        set
        {
            var floats = value.
                SelectMany((p2d) => new float [] { (float) p2d.X, (float) p2d.Y })
                .ToArray();

            refPointsBuffer.SetData(gl, 0, floats, false, 2);

            referencePoints = value;
        }
    }

    public Point2D [] RegionContour
    {
        get => regionContour;
        set
        {
            var floats = value.
                SelectMany((p2d) => new float [] { (float) p2d.X, (float) p2d.Y })
                .ToArray();

            contourBuffer.SetData(gl, 0, floats, false, 2);

            regionContour = value;
        }
    }

    public void DrawReferencePoints()
    {
        throw new NotImplementedException();
    }

    public void DrawRegionContour(uint mode)
    {
        shaderProgram.Bind(gl);
        var colorLoc = shaderProgram.GetUniformLocation(gl, "u_color");
        gl.Uniform4(colorLoc, LineColor.R, LineColor.G, LineColor.B, LineColor.A);

        contourBuffer.Bind(gl);
        gl.DrawArrays(mode, 0, regionContour.Length);
    }

    private void CreateProgram()
    {
        shaderProgram.Create(gl, File.ReadAllText(VertShaderLoc), File.ReadAllText(FragShaderLoc), []);
        shaderProgram.AssertValid(gl);
    }
}