using UnityEngine;

namespace Nova.Library.Utilities
{
    public static class Angle
    {
        public static float Constrain(float value, Vector2 constraints)
        {
            return Mathf.Clamp(InvertAngle(value), constraints.x, constraints.y);
        }

        public static float InvertAngle(float angle)
        {
            return angle > 180f ? angle - 360f : angle;
        }
    }
}