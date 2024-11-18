namespace Lab1.Models;

public static class DicomTextureCaster
{
    public static unsafe T [,,] CastTo3DArray<T>(IDicomData dicomData) where T : unmanaged
    {
        var numberOfBytes = dicomData.Width * dicomData.Height * dicomData.Width * sizeof(T);
        if (dicomData.Count != numberOfBytes)
            throw new ArgumentException("The size of the byte collection does not match the specified dimensions.");

        var bytes = dicomData.ToArray();
        T [,,] result = new T [dicomData.Width, dicomData.Height, dicomData.Depth];

        Buffer.BlockCopy(bytes, 0, result, 0, numberOfBytes);

        return result;
    }
}