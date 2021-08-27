using UnityEngine;

namespace Nova.Library.Utilities
{
    public static class PlayerFunctions
    {
        public static bool CheckIfPlayer(Transform other)
        {
            if (other.root.childCount > 1 && other.root.GetChild(1).childCount > 1)
                return other.root.GetChild(1).GetChild(0).CompareTag("Player");
            if (other.root.childCount > 1 && other.root.GetChild(1).childCount > 1)
                return other.root.GetChild(1).GetChild(0).CompareTag("Player");
            return false;
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


