using UnityEngine;

namespace Nova.Library.Derivables
{
    public abstract class PlayerCollisionBehavior : MonoBehaviour
    {
        [SerializeField] private bool playerIsRoot = true;
    
        private void OnCollisionEnter(Collision other)
        {
            Transform player = playerIsRoot ? other.transform.root : other.transform;
            //Debug.Log($"Collision with {other.Format()})");
            if (!player.CompareTag("Player")) return;
            CollisionEnterBehavior(player, other);
        }

        protected virtual void CollisionEnterBehavior(Transform player, Collision other) { }
    
        private void OnCollisionExit(Collision other)
        {
            Transform player = playerIsRoot ? other.transform.root : other.transform;
            //Debug.Log($"Collision with {other.Format()})");
            if (!player.CompareTag("Player")) return;
            CollisionExitBehavior(player, other);
        }

        protected virtual void CollisionExitBehavior(Transform player, Collision other) { }
    
        private void OnCollisionStay(Collision other)
        {
            Transform player = playerIsRoot ? other.transform.root : other.transform;
            //Debug.Log($"Collision with {other.Format()})");
            if (!player.CompareTag("Player")) return;
            CollisionStayBehavior(player, other);
        }

        protected virtual void CollisionStayBehavior(Transform player, Collision other) { }
    }

}