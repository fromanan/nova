using UnityEngine;

namespace Nova.Library.Derivables
{
    public abstract class PlayerTriggerBehavior : MonoBehaviour
    {
        [SerializeField] private bool playerIsRoot = true;
    
        private void OnTriggerEnter(Collider other)
        {
            Transform player = playerIsRoot ? other.transform.root : other.transform;
            //Debug.Log($"Collision with {other.Format()})");
            if (!player.CompareTag("Player")) return;
            TriggerEnterBehavior(player, other);
        }

        protected virtual void TriggerEnterBehavior(Transform player, Collider other) { }
    
        private void OnTriggerExit(Collider other)
        {
            Transform player = playerIsRoot ? other.transform.root : other.transform;
            //Debug.Log($"Collision with {other.Format()})");
            if (!player.CompareTag("Player")) return;
            TriggerExitBehavior(player, other);
        }

        protected virtual void TriggerExitBehavior(Transform player, Collider other) { }
    
        private void OnTriggerStay(Collider other)
        {
            Transform player = playerIsRoot ? other.transform.root : other.transform;
            //Debug.Log($"Collision with {other.Format()})");
            if (!player.CompareTag("Player")) return;
            TriggerStayBehavior(player, other);
        }

        protected virtual void TriggerStayBehavior(Transform player, Collider other) { }
    }

}