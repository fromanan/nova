using UnityEngine;

namespace Nova.Library.Utilities.Functions
{
    public static class Math
    {
        public static int WrappedClamp(int n, int min, int max)
        {
            if (n > max) return 0;
            return n < min ? max : n;
        }

        public static float LerpNegative(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static Vector3 Vector3LerpNegative(Vector3 a, Vector3 b, float t)
        {
            Vector3 c = Vector3.zero;
            c.x = LerpNegative(a.x, b.x, t);
            c.y = LerpNegative(a.y, b.y, t);
            c.z = LerpNegative(a.z, b.z, t);
            return c;
        }

        public static float AngleDifference(float angle1, float angle2)
        {
            float diff = (angle2 - angle1 + 180) % 360 - 180;
            return diff < -180 ? diff + 360 : diff;
        }
        
        public static float Diff(float a, float b)
        {
            return Mathf.Abs(a - b);
        }
    }
}