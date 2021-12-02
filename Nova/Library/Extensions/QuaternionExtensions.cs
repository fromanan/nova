using UnityEngine;

namespace Nova.Extensions
{
    public static class QuaternionExtensions
    {
        private static Quaternion LockRoll(this Quaternion rotation)
        {
            Vector3 angles = rotation.eulerAngles;
            return Quaternion.Euler(angles.x, angles.y, 0f);
        }
    }
}