using System.IO;
using System.Net.WebSockets;
using System.Numerics;
using Lab1.Models;
using MathNet.Numerics.LinearAlgebra;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
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

    private DicomToGLConverter? dicomGLData;
    private bool disposed;
    private AnatomicPlane? sourcePlane;
    private uint texture3D;
    private uint vao;
    private uint vbo;
    private int vertShader, fragShader, program;

    private CoordsPixelLength? volumePixSize;

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

    //public static float CalculateDepth(AnatomicPlane sourcePlane, AnatomicPlane targetPlane, int width, int height, int depth)
    //{
    //}

    public static OpenTK.Mathematics.Matrix4 ToOpenTKMatrix(Matrix4x4 matrix) => new(
        matrix.M11, matrix.M12, matrix.M13, matrix.M14,
        matrix.M21, matrix.M22, matrix.M23, matrix.M24,
        matrix.M31, matrix.M32, matrix.M33, matrix.M34,
        matrix.M41, matrix.M42, matrix.M43, matrix.M44);

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

    public void DrawVertices(AnatomicPlane targetPlane, int sliceNumber)
    {
        if (IsTextureLoaded)
        {
            float relativeDepth;

            switch (targetPlane)
            {
                case (AnatomicPlane.Axial):

                    relativeDepth = (float) sliceNumber / volumePixSize!.ZPixels;
                    break;

                case (AnatomicPlane.Saggital):
                    relativeDepth = (float) sliceNumber / volumePixSize!.XPixels;
                    break;

                case (AnatomicPlane.Coronal):
                    relativeDepth = (float) sliceNumber / volumePixSize!.YPixels;
                    break;

                default:
                    throw new NotImplementedException("Dicom GL only supports Axial plane as default.");
            }

            Matrix4x4 addDepth = Matrix4x4.CreateTranslation(0, 0, relativeDepth);
            Matrix4x4 changePlanes = AnatomicPlaneRelations.PlaneTransform((AnatomicPlane) sourcePlane!, targetPlane);

            var transformMatrix = changePlanes * addDepth;
            var opentkmatrix = ToOpenTKMatrix(transformMatrix);
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

            GL.UniformMatrix4(transMatLoc, false, ref opentkmatrix);

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

    public void LoadDicomTexture(IDicomData dicomData)
    {
        var converter = new DicomToGLConverter(dicomData);
        dicomGLData = converter;
        sourcePlane = dicomData.DefaultPlane;
        volumePixSize = new CoordsPixelLength(dicomData);
        ThrowIfGLError();

        GL.BindTexture(TextureTarget.Texture3D, texture3D);

        ThrowIfGLError();

        byte [] byteArray = dicomData.ToArray();

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

        GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Clamp);
        ThrowIfGLError();
        GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Clamp);
        ThrowIfGLError();
        GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR, (int) TextureWrapMode.Clamp);
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