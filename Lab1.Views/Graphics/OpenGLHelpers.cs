﻿using OpenTK.Graphics.OpenGL;

namespace Lab1.Views.Graphics;

public static class OpenGLHelpers
{
    public static unsafe float [] GetBufferSubData(int elementsNumber)
    {
        var arr = new float [elementsNumber];
        fixed (float* zuz = arr)
        {
            GL.GetBufferSubData(BufferTarget.ArrayBuffer, 0, sizeof(float) * elementsNumber, (nint) zuz);
        }

        return arr;
    }

    public static int MakeShader(ShaderType shaderType, string source)
    {
        var shader = GL.CreateShader(shaderType);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);
        GL.GetShaderInfoLog(shader, out string info);

        if (info != "") throw new Exception(info);
        return shader;
    }

    public static void ThrowIfGLError()
    {
        var error = GL.GetError();
        switch (error)
        {
            case ErrorCode.NoError:

                break;

            case (var other):
                throw new Exception(Enum.GetName(other));
        }
    }
}