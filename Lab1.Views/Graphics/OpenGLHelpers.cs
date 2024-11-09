using SharpGL;
using SharpGL.Enumerations;
using static SharpGL.OpenGL;

namespace Lab1.Views.Graphics;

public static class OpenGLHelpers
{
    public static unsafe float [] GetBufferSubData(OpenGL gl, int elementsNumber)
    {
        var arr = new float [elementsNumber];
        fixed (float* zuz = arr)
        {
            gl.GetBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(float) * elementsNumber, (nint) zuz);
        }

        return arr;
    }

    public static void ThrowIfGLError(OpenGL gl)
    {
        var error = gl.GetErrorCode();
        switch (error)
        {
            case ErrorCode.NoError:

                break;

            case (var other):
                throw new Exception(gl.GetErrorDescription(Convert.ToUInt32(other)));
        }
    }
}