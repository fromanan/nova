using UnityEngine;

namespace Nova.Library.Extensions
{
    public static class ColliderExtensions
    {
        public static T GetComponent<T>(this Collision other)
        {
            return other.gameObject.GetComponent<T>();
        }
    
        public static T GetComponent<T>(this Collider other)
        {
            return other.gameObject.GetComponent<T>();
        }

        public static bool CompareTag(this Collision other, string tag)
        {
            return other.gameObject.CompareTag(tag);
        }
    
        public static bool CompareTag(this Collider other, string tag)
        {
            return other.gameObject.CompareTag(tag);
        }
    }
}