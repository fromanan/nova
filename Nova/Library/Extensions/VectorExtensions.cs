using UnityEngine;

namespace Nova.Library.Extensions
{
    public static class VectorExtensions
    {
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
    }
}