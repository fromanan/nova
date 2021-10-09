using UnityEngine;

namespace Nova.Library.Utilities
{
    public static class Player
    {
        public const string PLAYER_TAG = "Player";
        
        public static bool CheckIfPlayer(Transform other, bool isRoot = true)
        {
            return isRoot ? other.root.CompareTag(PLAYER_TAG) : other.CompareTag(PLAYER_TAG);
        }

        public static bool IsPlayer(Collider other, bool isRoot = true)
        {
            return CheckIfPlayer(other.transform, isRoot);
        }

        public static bool IsPlayer(Collision other, bool isRoot = true)
        {
            return CheckIfPlayer(other.transform, isRoot);
        }
    }
}


