using Nova.Utilities;
using UnityEngine;

namespace Nova.Extensions
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
            return new Vector3(LerpNegative(a.x, b.x, t), LerpNegative(a.y, b.y, t), LerpNegative(a.z, b.z, t));
        }
        
        public static Vector3 Inverse(this Vector3 vector)
        {
            return new Vector3(-vector.x, -vector.y, -vector.z);
        }
        
        public static Vector3 Midpoint(this Vector3 a, Vector3 b)
        {
            return (a + b) / 2f;
        }
        
        // Returns a vector that points from a to b
        public static Vector3 Direction(this Vector3 a, Vector3 b)
        {
            return b - a;
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
        
        // In-place
        public static void Constrain(this Vector3 vector, Vector2 xConstraints, Vector2 yConstraints)
        {
            vector.x = Angle.Constrain(vector.x, xConstraints);
            vector.y = Angle.Constrain(vector.y, yConstraints);
        }
        
        public static Vector3 Constrained(this Vector3 vector, Vector2 xConstraints, Vector2 yConstraints)
        {
            return new Vector3(Angle.Constrain(vector.x, xConstraints), 
                Angle.Constrain(vector.y, yConstraints), vector.z);
        }

        // In-place
        public static void Constrain(this Vector3 vector, Vector2 xConstraints, Vector2 yConstraints, Vector2 zConstraints)
        {
            vector.x = Angle.Constrain(vector.x, xConstraints);
            vector.y = Angle.Constrain(vector.y, yConstraints);
            vector.z = Angle.Constrain(vector.z, zConstraints);
        }
        
        public static Vector3 Constrained(this Vector3 vector, Vector2 xConstraints, Vector2 yConstraints, Vector2 zConstraints)
        {
            return new Vector3(Angle.Constrain(vector.x, xConstraints), 
                Angle.Constrain(vector.y, yConstraints),
                Angle.Constrain(vector.z, zConstraints));
        }
        
        // TODO: Clamping

        public static Vector3 InDirectionOf(this Vector3 vector, Transform transform)
        {
            return transform.right * vector.x + transform.up * vector.y + transform.forward * vector.z;
        }

        public static Vector3 Mask(this Vector3 vector, Axes.Axis axes)
        {
            return vector.Hadamard(Axes.VectorFromMask(axes));
        }
        
        // In-place
        public static void Flip(this Vector3 vector)
        {
            (vector.x, vector.z) = (vector.z, vector.x);
        }
        
        public static Vector3 Flipped(this Vector3 vector)
        {
            return new Vector3(vector.z, vector.y, vector.x);
        }
    }
}