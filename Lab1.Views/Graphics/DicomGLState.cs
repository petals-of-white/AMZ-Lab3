using System.IO;
using Lab1.Models;
using MathNet.Numerics.LinearAlgebra;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.Shaders;
using static Lab1.Views.Graphics.OpenGLHelpers;
using static SharpGL.OpenGL;

namespace Lab1.Views.Graphics;

public class DicomGLState : IDisposable
{
    private static readonly float [] coords = {
        -1f, -1f,     0f, 0f,
        -1f, 1f,      0f, 1f,
        1f,  -1f,     1f, 0f,
        1f,  1f,      1f, 1f,
    };

    private readonly OpenGL gl;
    private readonly ShaderProgram shaderProgram = new();

    private bool disposed;
    private uint texture3D;
    private uint vao;
    private uint vbo;

    public DicomGLState(OpenGL openGL)
    {
        gl = openGL;

        while (gl.GetErrorCode() is not ErrorCode.NoError) ;

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
            gl.DeleteTextures(1, [texture3D]);
            gl.DeleteVertexArrays(1, [vao]);
            gl.DeleteBuffers(1, [vbo]);
            shaderProgram.Delete(gl);

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

            gl.BindVertexArray(vao);

            gl.BindBuffer(GL_ARRAY_BUFFER, vbo);
            //var checkData = GetBufferSubData(gl, 16);

            shaderProgram.Bind(gl);
            
            gl.ActiveTexture(GL_TEXTURE0);
            gl.BindTexture(GL_TEXTURE_3D, texture3D);

            ThrowIfGLError(gl);

            shaderProgram.SetUniformMatrix4(gl, "u_transform_matrix", transformMatrix.ToRowMajorArray());

            ThrowIfGLError(gl);

            gl.DrawArrays(GL_TRIANGLE_STRIP, 0, 4);

            switch (shaderProgram.GetInfoLog(gl))
            {
                case "": break;
                case (string nonempty): throw new Exception(nonempty);
            }

            // unbind
            gl.BindVertexArray(0);
            gl.BindBuffer(GL_ARRAY_BUFFER, 0);
            gl.BindTexture(GL_TEXTURE_3D, 0);
            shaderProgram.Unbind(gl);

            ThrowIfGLError(gl);
        }
    }

    public unsafe void LoadDicomTexture(DicomManager dicomMng)
    {
        var converter = new DicomToGLConverter(dicomMng);

        gl.BindTexture(GL_TEXTURE_3D, texture3D);
        fixed (byte* ptr = converter.TextureData)
        {
            var npointer = (nint) ptr;
            gl.TexImage3D(GL_TEXTURE_3D, 0, (int) converter.InternalFormat, converter.Width, converter.Height, converter.Depth,
            0, converter.Format, converter.Type, npointer);
        }

        gl.BindTexture(GL_TEXTURE_3D, 0);

        ThrowIfGLError(gl);

        IsTextureLoaded = true;
    }

    public void UnbindAll()
    {
        gl.BindVertexArray(0);
        gl.BindBuffer(GL_ARRAY_BUFFER, 0);

        shaderProgram.Unbind(gl);

        gl.BindTexture(GL_TEXTURE_3D, 0);
    }

    private void BindAll()
    {
        gl.BindVertexArray(vao);
        gl.BindBuffer(GL_ARRAY_BUFFER, vbo);

        shaderProgram.Bind(gl);

        gl.BindTexture(GL_TEXTURE_3D, texture3D);
    }

    private void CreateProgram()
    {
        shaderProgram.Create(gl, File.ReadAllText(VertShaderLoc), File.ReadAllText(FragShaderLoc), []);
        shaderProgram.AssertValid(gl);
    }

    private void CreateTexture()
    {
        uint [] textures = new uint [1];
        gl.GenTextures(1, textures);
        texture3D = textures [0];

        shaderProgram.Bind(gl);

        gl.ActiveTexture(GL_TEXTURE0);
        gl.BindTexture(GL_TEXTURE_3D, texture3D);
        gl.TexParameter(GL_TEXTURE_3D, GL_TEXTURE_WRAP_S, [GL_REPEAT]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_WRAP_T, [GL_REPEAT]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_WRAP_R, [GL_REPEAT]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_MIN_FILTER, [GL_LINEAR]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_MAG_FILTER, [GL_LINEAR]);

        //var texUniLoc = gl.GetUniformLocation(shaderProgram.ShaderProgramObject, "u_texture");
        //int sampler3d = 0;
        //gl.Uniform1(texUniLoc, sampler3d);

        // unbind all
        gl.BindTexture(GL_TEXTURE_3D, 0);
        shaderProgram.Unbind(gl);

        // info log. WHY?
        switch (shaderProgram.GetInfoLog(gl))
        {
            case "": break;
            case (string nonempty): throw new Exception(nonempty);
        }

        ThrowIfGLError(gl);
    }

    private void CreateVertices()
    {
        uint [] vaos = new uint [1];
        uint [] vbos = new uint [1];
        gl.GenVertexArrays(1, vaos);
        gl.BindVertexArray(vaos [0]);

        gl.GenBuffers(1, vbos);
        gl.BindBuffer(GL_ARRAY_BUFFER, vbos [0]);
        gl.BufferData(GL_ARRAY_BUFFER, coords, GL_STATIC_DRAW);

        ThrowIfGLError(gl);

        // buffer data...
        gl.VertexAttribPointer(0, 2, GL_FLOAT, false, 4 * sizeof(float), 0);
        gl.EnableVertexAttribArray(0);

        gl.VertexAttribPointer(1, 2, GL_FLOAT, false, 4 * sizeof(float), 2 * sizeof(float));
        gl.EnableVertexAttribArray(1);

        // unbindg vao
        gl.BindVertexArray(0);

        // unbind vbo
        gl.BindBuffer(GL_ARRAY_BUFFER, 0);
        vbo = vbos [0];
        vao = vaos [0];

        ThrowIfGLError(gl);
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~DicomGLState()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }
    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~DicomGLState()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }
}