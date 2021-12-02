using UnityEngine;

namespace Nova.Attachables
{
    public class TimedObjectDestructor : MonoBehaviour
    {
        [SerializeField] public float timeToDestroy;
        [SerializeField] private bool destroyOnAwake;

        private bool destroying;

        private void Awake()
        {
            if (destroyOnAwake)
            {
                InitiateDestroy();
            }
        }
    
        public void InitiateDestroy()
        {
            if (destroying) return;
            Destroy(gameObject, timeToDestroy);
            destroying = true;
        }
    }

}