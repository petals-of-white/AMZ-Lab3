using System.Drawing;
using System.IO;
using System.Text;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Mathematics;
using SharpGL;
using SharpGL.Shaders;
using static SharpGL.OpenGL;

namespace Lab1.Views.Graphics;

public class RegionOfInterestGL
{
    private readonly OpenGL gl;
    private uint vbo, vao, vertShader, fragShader, program;
    private Point2D [] referencePoints;
    public Color LineColor { get; set; }
    public Color ReferencePointColor { get; set; }

    public Point2D [] ReferencePoints
    {
        get => referencePoints;
        set
        {
            gl.BindBuffer(GL_ARRAY_BUFFER, vbo);
            var floats = value.
                SelectMany((p2d) => new float [] { (float) p2d.X, (float) p2d.Y })
                .ToArray();

            gl.BufferData(GL_ARRAY_BUFFER, floats, GL_STATIC_DRAW);
            gl.BindBuffer(GL_ARRAY_BUFFER, 0);

            referencePoints = value;
        }
    }

    public RegionOfInterestGL(OpenGL opengl)
    {
        gl = opengl;
        CreateVAO();
        CreateProgram();
    }


    public static string FragShaderLoc { get; } = "Shaders/ROI.frag";

    public static string VertShaderLoc { get; } = "Shaders/ROI.vert";


    private void CreateProgram()
    {
        program = gl.CreateProgram();
        vertShader = CreateShader(GL_VERTEX_SHADER, File.ReadAllText(VertShaderLoc));
        fragShader = CreateShader(GL_FRAGMENT_SHADER, File.ReadAllText(FragShaderLoc));
        gl.AttachShader(program, vertShader);
        gl.AttachShader(program, fragShader);
        gl.LinkProgram(program);
        gl.ValidateProgram(program);
    }

    private uint CreateShader(uint shaderType, string shaderSource)
    {
        uint shader = gl.CreateShader(shaderType);
        gl.ShaderSource(shader, shaderSource);
        gl.CompileShader(shader);

        int [] res = [0];
        gl.GetShader(shader, GL_COMPILE_STATUS, res);

        if (res [0] == 0)
        {
            StringBuilder infoLog = new(512);
            gl.GetShaderInfoLog(shader, 512, 0, infoLog);
            throw new ShaderCompilationException(infoLog.ToString());
        }
        return shader;
    }

    private void CreateVAO()
    {
        uint [] vaos = new uint [1];
        uint [] vbos = new uint [1];

        gl.GenVertexArrays(1, vaos);
        gl.GenBuffers(1, vbos);
        gl.BindVertexArray(vaos [0]);

        gl.BindBuffer(GL_ARRAY_BUFFER, vbos [0]);
        gl.VertexAttribPointer(0, 2, GL_FLOAT, false, 2 * sizeof(float), 0);
        gl.EnableVertexAttribArray(0);

        gl.BindBuffer(GL_ARRAY_BUFFER, vbos [0]);
        gl.BindVertexArray(vaos [0]);

        vbo = vbos [0];
        vao = vaos [0];
    }
}