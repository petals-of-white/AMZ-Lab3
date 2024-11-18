using System.Numerics;

namespace Lab1.Models;

public enum AnatomicPlane
{
    Saggital,
    Axial,
    Coronal
}

public static class AnatomicPlaneRelations
{
    public static Matrix4x4 PlaneTransform(AnatomicPlane sourcePlane, AnatomicPlane targetPlane)
    {
        return (sourcePlane, targetPlane) switch
        {
            (var plane1, var plane2) when plane1 == plane2 => Matrix4x4.Identity,

            (AnatomicPlane.Axial, AnatomicPlane.Saggital) => Matrix4x4.CreateRotationY(float.Pi),

            (AnatomicPlane.Axial, AnatomicPlane.Coronal) => Matrix4x4.CreateRotationX(float.Pi),

            (AnatomicPlane.Saggital, AnatomicPlane.Axial) => Matrix4x4.CreateRotationY(-float.Pi),

            (AnatomicPlane.Saggital, AnatomicPlane.Coronal) => Matrix4x4.CreateRotationZ(float.Pi),

            (AnatomicPlane.Coronal, AnatomicPlane.Axial) => Matrix4x4.CreateRotationX(-float.Pi),

            (AnatomicPlane.Coronal, AnatomicPlane.Saggital) => Matrix4x4.CreateRotationZ(-float.Pi),

            _ => throw new ArgumentException("Either source plane or target plane is not valid.")
        };
    }
}