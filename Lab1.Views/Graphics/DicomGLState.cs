using System.IO;
using Lab1.Models;
using MathNet.Numerics.LinearAlgebra;
using OpenTK.Graphics.OpenGL;
using static Lab1.Views.Graphics.OpenGLHelpers;

namespace Lab1.Views.Graphics;

public class DicomGLState : IDisposable
{
    private static readonly float [] coords = {
        -1f, -1f,     0f, 0f,
        -1f, 1f,      0f, 1f,
        1f,  -1f,     1f, 0f,
        1f,  1f,      1f, 1f,
    };

    private bool disposed;
    private uint texture3D;
    private uint vao;
    private uint vbo;
    private int vertShader, fragShader, program;

    public DicomGLState()
    {
        while (GL.GetError() is not ErrorCode.NoError) ;

        CreateVertices();
        CreateProgram();
        CreateTexture();

        UnbindAll();
    }

    public static string FragShaderLoc { get; } = "Shaders/shader.frag";
    public static string VertShaderLoc { get; } = "Shaders/shader.vert";
    public bool IsTextureLoaded { get; private set; } = false;

    public void Dispose()
    {
        if (!disposed)
        {
            UnbindAll();
            GL.DeleteTextures(1, [texture3D]);
            GL.DeleteVertexArrays(1, [vao]);
            GL.DeleteBuffers(1, [vbo]);
            GL.DeleteProgram(program);
            GL.DeleteShader(fragShader);
            GL.DeleteShader(vertShader);
            disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    public void DrawVertices(float depth)
    {
        if (IsTextureLoaded)
        {
            //BindAll();

            var transformMatrix = CreateMatrix.DenseOfArray(new float [,] {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, depth },
            { 0, 0, 0, 1 }
        });
            ThrowIfGLError();
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            //var checkData = GetBufferSubData(gl, 16);

            GL.UseProgram(program);

            ThrowIfGLError();

            GL.ActiveTexture(TextureUnit.Texture0);

            GL.BindTexture(TextureTarget.Texture3D, texture3D);

            ThrowIfGLError();

            var transMatLoc = GL.GetUniformLocation(program, "u_transform_matrix");

            GL.UniformMatrix4(transMatLoc, 1, false, transformMatrix.ToRowMajorArray());

            ThrowIfGLError();

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            // unbind
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindTexture(TextureTarget.Texture3D, 0);
            GL.UseProgram(0);
            ThrowIfGLError();
        }
    }

    public unsafe void LoadDicomTexture(DicomManager dicomMng)
    {
        var converter = new DicomToGLConverter(dicomMng);

        ThrowIfGLError();

        GL.BindTexture(TextureTarget.Texture3D, texture3D);

        ThrowIfGLError();

        byte [] byteArray = converter.TextureData;

        GL.TexImage3D(TextureTarget.Texture3D, 0, converter.InternalFormat,
            converter.Width, converter.Height, converter.Depth,
            0, converter.Format, converter.Type, byteArray);

        ThrowIfGLError();
        GL.BindTexture(TextureTarget.Texture3D, 0);

        ThrowIfGLError();

        GL.UseProgram(program);

        // Upload window-level uniforms
        int windowLoc = GL.GetUniformLocation(program, "winLevel.ww");
        int levelLoc = GL.GetUniformLocation(program, "winLevel.wl");

        GL.Uniform1(windowLoc, 1000f);
        GL.Uniform1(levelLoc, 500f);

        ThrowIfGLError();

        // upload normalization uniforms
        int minPeakLoc = GL.GetUniformLocation(program, "minPeak");
        int maxPeakLoc = GL.GetUniformLocation(program, "maxPeak");
        short [] texArray = new short [byteArray.Length / 2];
        System.Buffer.BlockCopy(byteArray, 0, texArray, 0, byteArray.Length);
        var maxVal = texArray.Max();
        var minVal = texArray.Min();
        GL.Uniform1(minPeakLoc, (float) minVal);
        GL.Uniform1(maxPeakLoc, (float) maxVal);

        ThrowIfGLError();

        GL.UseProgram(0);
        IsTextureLoaded = true;
    }

    public void UnbindAll()
    {
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.UseProgram(0);
        GL.BindTexture(TextureTarget.Texture3D, 0);
    }

    private void BindAll()
    {
        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        GL.UseProgram(program);

        GL.BindTexture(TextureTarget.Texture3D, texture3D);
    }

    private void CreateProgram()
    {
        vertShader = MakeShader(ShaderType.VertexShader, File.ReadAllText(VertShaderLoc));
        fragShader = MakeShader(ShaderType.FragmentShader, File.ReadAllText(FragShaderLoc));

        program = GL.CreateProgram();
        GL.AttachShader(program, vertShader);
        GL.AttachShader(program, fragShader);
        GL.LinkProgram(program);
        GL.ValidateProgram(program);

        // TODO: validate progam
    }

    private void CreateTexture()
    {
        uint [] textures = new uint [1];
        GL.GenTextures(1, textures);
        texture3D = textures [0];

        GL.UseProgram(program);

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture3D, texture3D);

        ThrowIfGLError();

        GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
        ThrowIfGLError();
        GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
        ThrowIfGLError();
        GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR, (int) TextureWrapMode.Repeat);
        ThrowIfGLError();
        GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
        ThrowIfGLError();
        GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Linear);

        ThrowIfGLError();

        var texUniLoc = GL.GetUniformLocation(program, "u_texture");
        int sampler3d = 0;
        GL.Uniform1(texUniLoc, sampler3d);

        ThrowIfGLError();

        // unbind all
        GL.BindTexture(TextureTarget.Texture3D, 0);
        GL.UseProgram(0);

        // info log. WHY?
        //switch (shaderProgram.GetInfoLog(gl))
        //{
        //    case "": break;
        //    case (string nonempty): throw new Exception(nonempty);
        //}
    }

    private void CreateVertices()
    {
        uint [] vaos = new uint [1];
        uint [] vbos = new uint [1];
        GL.GenVertexArrays(1, vaos);
        GL.BindVertexArray(vaos [0]);

        GL.GenBuffers(1, vbos);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbos [0]);
        GL.BufferData(BufferTarget.ArrayBuffer, coords.Length * sizeof(float), coords, BufferUsageHint.StaticDraw);

        ThrowIfGLError();

        // buffer data...
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        // unbindg vao
        GL.BindVertexArray(0);

        // unbind vbo

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        vbo = vbos [0];
        vao = vaos [0];

        ThrowIfGLError();
    }
}