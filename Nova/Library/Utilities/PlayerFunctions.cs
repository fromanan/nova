using UnityEngine;

namespace Nova.Library.Utilities
{
    public static class PlayerFunctions
    {
        public static bool CheckIfPlayer(Transform other)
        {
            return other.root.CompareTag("Player");
        }

        public static bool IsPlayer(Collider other)
        {
            return CheckIfPlayer(other.transform);
        }

        public static bool IsPlayer(Collision other)
        {
            return CheckIfPlayer(other.transform);
        }
    }
}


