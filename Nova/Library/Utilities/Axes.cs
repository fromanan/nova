using UnityEngine;

namespace Nova.Utilities
{
    public static class Axes
    {
        public enum Axis
        {
            X,
            Y,
            Z,
            XY,
            XZ,
            YZ,
            XYZ
        }

        public static Vector3 VectorFromMask(Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return Vector3.right;
                case Axis.Y:
                    return Vector3.up;
                case Axis.Z:
                    return Vector3.forward;
                case Axis.XY:
                    return Vector3.right + Vector3.up;
                case Axis.XZ:
                    return Vector3.right + Vector3.forward;
                case Axis.YZ:
                    return Vector3.up + Vector3.forward;
                case Axis.XYZ:
                    return Vector3.one;
                default:
                    return Vector3.zero;
            }
        }
    }
}