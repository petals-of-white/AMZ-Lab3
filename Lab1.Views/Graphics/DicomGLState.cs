using System.IO;
using System.Text;
using Lab1.Models;
using SharpGL;
using static SharpGL.OpenGL;
namespace Lab1.Views.Graphics;

public class DicomGLState
{
    private readonly OpenGL gl;
    private uint program;
    private uint vertShader;
    private uint fragShader;
    private uint vao;
    private uint vbo;
    private uint texture3D;

    public void UnbindAll()
    {
        gl.BindVertexArray(0);
        gl.UseProgram(0);
        gl.BindTexture(GL_TEXTURE_3D, 0);
    }

    public void BindALl()
    {
        gl.BindVertexArray(vao);
        gl.UseProgram(program);
        gl.BindTexture(GL_TEXTURE_3D, texture3D);
    }

    public DicomGLState(OpenGL openGL)
    {
        gl = openGL;
        CreateVAO();
        CreateProgram();
        CreateTexture();
    }

    public static string VertShaderLoc { get; } = "Shaders/shader.vert";
    public static string FragShaderLoc { get; } = "Shaders/shader.frag";

    public unsafe void LoadDicomTexture(DicomManager dicomMng)
    {

        var converter = new DicomToGLConverter(dicomMng);

        gl.BindTexture(GL_TEXTURE_3D, texture3D);
        fixed (byte* ptr = converter.TextureData)
        {
            gl.TexImage3D(GL_TEXTURE_3D, 0, (int) converter.InternalFormat, converter.Width, converter.Height, converter.Depth,
            0, converter.Format, converter.Type, (nint) ptr);
        }

    }

    private void CreateTexture()
    {
        uint [] textures = new uint [1];
        gl.GenTextures(1, textures);
        texture3D = textures [0];
        gl.BindTexture(GL_TEXTURE_3D, texture3D);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_WRAP_S, [GL_REPEAT]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_WRAP_T, [GL_REPEAT]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_WRAP_R, [GL_REPEAT]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_MIN_FILTER, [GL_LINEAR]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_MAG_FILTER, [GL_LINEAR]);
        gl.BindTexture(GL_TEXTURE_3D, 0);
    }
    private void CreateVAO()
    {
        uint [] vaos = new uint [1];
        uint [] vbos = new uint [1];
        gl.GenVertexArrays(1, vaos);
        gl.GenBuffers(1, vbos);
        gl.BindVertexArray(vaos [0]);

        gl.BindBuffer(GL_ARRAY_BUFFER, vbos [0]);

        // buffer data...

        gl.VertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), 0);
        gl.EnableVertexAttribArray(0);

        gl.BindBuffer(GL_ARRAY_BUFFER, vbos [0]);
        gl.BindVertexArray(vaos [0]);

        vbo = vbos [0];
        vao = vaos [0];
    }
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
            Console.WriteLine($"Shader compilation error: {infoLog}.");
        }
        return shader;
    }
}