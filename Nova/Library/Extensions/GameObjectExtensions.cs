using UnityEngine;

namespace Nova.Library.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool HasComponent<T>(this GameObject gameObject)
        {
            return gameObject.GetComponent<T>() == null;
        }
    }
}