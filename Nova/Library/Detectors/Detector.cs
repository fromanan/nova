using Nova.Derivables;
using UnityEngine;
using UnityEngine.Events;

namespace Nova.Detectors
{
    public abstract class Detector : PlayerTriggerBehavior
    {
        [Header("Detection Events")]
        [SerializeField] protected UnityEvent OnPlayerEnter = new UnityEvent();
        [SerializeField] protected UnityEvent OnPlayerExit = new UnityEvent();
        [SerializeField] protected UnityEvent OnPlayerStay = new UnityEvent();
        
        [Header("Tag to Detect")]
        [SerializeField] protected string tagToDetect = "Player";
        public bool ObjectDetected { get; private set; }

        public GameObject DetectedObject { get; private set; }

        protected override void TriggerExitBehavior(Transform player, Collider other)
        {
            ObjectDetected = false;
            DetectedObject = null;
            OnPlayerExit.Invoke();
        }

        protected override void TriggerEnterBehavior(Transform player, Collider other)
        {
            ObjectDetected = true;
            DetectedObject = other.gameObject;
            OnPlayerEnter.Invoke();
        }

        protected override void TriggerStayBehavior(Transform player, Collider other)
        {
            ObjectDetected = true;
            DetectedObject = other.gameObject;
            OnPlayerStay.Invoke();
        }
    }
}