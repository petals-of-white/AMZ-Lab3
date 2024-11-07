using System.IO;
using Lab1.Models;
using SharpGL;
using SharpGL.Shaders;
using static SharpGL.OpenGL;

namespace Lab1.Views.Graphics;

using MathNet.Numerics.LinearAlgebra;

public class DicomGLState
{
    private static float [] coords = {
        -1, -1,     0, 0, 0
        -1, 1,      0, 1, 0,
        1,  -1,     1, 0, 0,
        1,  1,      1, 1, 0
    };

    private readonly OpenGL gl;
    private readonly ShaderProgram shaderProgram = new();

    private uint texture3D;
    private uint vao;
    private uint vbo;

    public DicomGLState(OpenGL openGL)
    {
        gl = openGL;
        CreateVAO();
        UploadCoords();
        CreateProgram();
        CreateTexture();
    }

    public static string FragShaderLoc { get; } = "Shaders/shader.frag";

    public static string VertShaderLoc { get; } = "Shaders/shader.vert";

    public void DrawVertices(float depth)
    {
        var transformMatrix = CreateMatrix.DenseOfArray(new float [,] {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, depth },
            { 0, 0, 0, 1 }
        });

        shaderProgram.SetUniformMatrix4(gl, "u_transfrom_matrix", transformMatrix.ToRowMajorArray());

        gl.DrawArrays(GL_TRIANGLE_STRIP, 0, 2);
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
    }

    public void UnbindAll()
    {
        gl.BindVertexArray(0);
        gl.UseProgram(0);

        gl.BindTexture(GL_TEXTURE_3D, 0);
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

        gl.BindTexture(GL_TEXTURE_3D, texture3D);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_WRAP_S, [GL_REPEAT]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_WRAP_T, [GL_REPEAT]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_WRAP_R, [GL_REPEAT]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_MIN_FILTER, [GL_LINEAR]);
        gl.TexParameterI(GL_TEXTURE_3D, GL_TEXTURE_MAG_FILTER, [GL_LINEAR]);
        gl.BindTexture(GL_TEXTURE_3D, 0);

        shaderProgram.SetUniform1(gl, "outTexture", 0);
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

        gl.VertexAttribPointer(0, 2, GL_FLOAT, false, 2 * sizeof(float), 0);
        gl.EnableVertexAttribArray(0);

        gl.VertexAttribPointer(1, 2, GL_FLOAT, false, 2 * sizeof(float), 2 * sizeof(float));
        gl.EnableVertexAttribArray(1);

        gl.BindBuffer(GL_ARRAY_BUFFER, vbos [0]);
        gl.BindVertexArray(vaos [0]);

        vbo = vbos [0];
        vao = vaos [0];
    }

    private void UploadCoords()
    {
        gl.BindVertexArray(vao);
        gl.BufferData(GL_ARRAY_BUFFER, coords, GL_STATIC_DRAW);
    }
}