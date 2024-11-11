using System.Drawing;

//using System.Windows;
//using System.Windows.Shapes;
using Lab1.Views.Colors;
using OpenTK.Graphics.OpenGL;

//using SharpGL;
//using SharpGL.Shaders;
//using SharpGL.VertexBuffers;
using static Lab1.Views.Graphics.OpenGLHelpers;

namespace Lab1.Views.Graphics;

public class RegionOfInterestGL : IDisposable
{
    private readonly int vertShader, fragShader, program;
    private uint contourBuffer, refPointsBuffer;
    private uint contourVAO, refPointsVAO;
    private bool disposedValue;

    private PointF [] referencePoints = Array.Empty<PointF>();

    private PointF [] regionContour = Array.Empty<PointF>();

    public RegionOfInterestGL()
    {
        CreateBuffers();
        (vertShader, fragShader, program) = CreateProgram(VertShaderLoc, FragShaderLoc);
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~RegionOfInterestGL()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public static string FragShaderLoc { get; } = "Shaders/ROI.frag";
    public static SizeF SquareSize { get; } = new SizeF(0.05f, 0.05f);
    public static string VertShaderLoc { get; } = "Shaders/ROI.vert";
    public RGBA<float> LineColor { get; set; }
    public RGBA<float> ReferencePointColor { get; set; }

    public PointF [] ReferencePoints
    {
        get => referencePoints;
        set
        {
            var points = value.
                SelectMany(center =>
                {
                    var square = SquareFromPoint(center, SquareSize);

                    return new PointF [] {
                        square.Location, new(square.Right, square.Top),
                        new(square.Bottom, square.Left), new(square.Bottom, square.Right)
                    };
                }).ToArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, refPointsBuffer);

            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 2 * points.Length, points, BufferUsageHint.DynamicDraw);

            referencePoints = value;
        }
    }

    public PointF [] RegionContour
    {
        get => regionContour;
        set
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, contourBuffer);

            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 2 * value.Length, value, BufferUsageHint.DynamicDraw);

            regionContour = value;
        }
    }

    public static RectangleF SquareFromPoint(PointF point, SizeF size) => new RectangleF(point - (size / 2), size);

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void DrawReferencePoints()
    {
        GL.UseProgram(program);

        var colorLoc = GL.GetUniformLocation(program, "u_color");

        GL.Uniform4(colorLoc, ReferencePointColor.R, ReferencePointColor.G, ReferencePointColor.B, ReferencePointColor.A);
        ThrowIfGLError();
        GL.BindVertexArray(refPointsVAO);

        GL.DrawArrays(PrimitiveType.TriangleStrip, 0, referencePoints.Length);
        ThrowIfGLError();
    }

    public void DrawRegionContour(PrimitiveType mode)
    {
        GL.UseProgram(program);

        var colorLoc = GL.GetUniformLocation(program, "u_color");

        GL.Uniform4(colorLoc, LineColor.R, LineColor.G, LineColor.B, LineColor.A);
        ThrowIfGLError();
        GL.BindVertexArray(contourVAO);
        GL.DrawArrays(mode, 0, regionContour.Length);
        ThrowIfGLError();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            GL.UseProgram(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DeleteShader(vertShader);
            GL.DeleteShader(fragShader);
            GL.DeleteProgram(program);

            GL.DeleteBuffer(contourBuffer);
            GL.DeleteBuffer(refPointsBuffer);

            disposedValue = true;
        }
    }

    private void CreateBuffers()
    {
        var buffers = new uint [2];
        var vaos = new uint [2];

        GL.BindVertexArray(0);

        GL.GenBuffers(2, buffers);
        GL.GenVertexArrays(2, vaos);

        ThrowIfGLError();
        GL.BindVertexArray(vaos [0]);
        GL.BindBuffer(BufferTarget.ArrayBuffer, buffers [0]);
        GL.EnableVertexAttribArray(0);
        ThrowIfGLError();
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        ThrowIfGLError();

        GL.BindVertexArray(vaos [1]);
        GL.BindBuffer(BufferTarget.ArrayBuffer, buffers [1]);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        ThrowIfGLError();

        contourBuffer = buffers [0];
        refPointsBuffer = buffers [1];
        contourVAO = vaos [0];
        refPointsVAO = vaos [1];

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);

        ThrowIfGLError();
    }
}