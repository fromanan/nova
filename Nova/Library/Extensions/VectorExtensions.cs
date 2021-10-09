using UnityEngine;

namespace Nova.Library.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 Hadamard(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        
        public static float LerpNegative(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static Vector3 LerpNegative(this Vector3 a, Vector3 b, float t)
        {
            Vector3 c = Vector3.zero;
            c.x = LerpNegative(a.x, b.x, t);
            c.y = LerpNegative(a.y, b.y, t);
            c.z = LerpNegative(a.z, b.z, t);
            return c;
        }
        
        public static Vector3 Inverse(this Vector3 vector)
        {
            return new Vector3(-vector.x, -vector.y, -vector.z);
        }
        
        public static Vector3 Midpoint(this Vector3 a, Vector3 b)
        {
            return (a + b) / 2f;
        }

        public static Vector3 Closest(this Vector3 center, Vector3 a, Vector3 b)
        {
            return Vector3.Distance(center, a) < Vector3.Distance(center, b) ? a : b;
        }

        public static Vector3 Furthest(this Vector3 center, Vector3 a, Vector3 b)
        {
            return Vector3.Distance(center, a) > Vector3.Distance(center, b) ? a : b;
        }

        /*public static Vector3 Clamp(this Vector3 point, Vector3 min, Vector3 max)
        {
            return Vector3.negativeInfinity.Furthest(point, min)
        }*/
        
        public static Vector3 Rotate(this Vector3 vector, float angle, Vector3 axis)
        {
            return Quaternion.AngleAxis(angle, axis) * vector;
        }
        
        public static Vector3 Rotate(this Vector3 vector, Vector3 angles)
        {
            return Quaternion.AngleAxis(angles.x, Vector3.right) * 
                   Quaternion.AngleAxis(angles.y, Vector3.up) * 
                   Quaternion.AngleAxis(angles.z, Vector3.forward) * vector;
        }
    }
}