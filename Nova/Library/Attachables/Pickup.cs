using System.Collections;
using Nova.Library.Attributes;
using Nova.Library.Derivables;
using Nova.Library.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Nova.Library.Attachables
{
    public class Pickup : PlayerTriggerBehavior
    {
        [Header("Pickup Properties")]
        [SerializeField] private Transform target;
        [SerializeField] private float pickupSpeed = 10f;
        [SerializeField] private new Rigidbody rigidbody;

        [Header("Item Properties")] 
        [SerializeField] private Progress.Item template;
        [SerializeField] private int itemAmount = 1;

        // Animation Tracking
        [Header("Tracking")] 
        [SerializeField] private bool pickupable;
        [SerializeField] private bool rolling;
        [SerializeField] private bool inAir;
        [ReadOnly] [SerializeField] private bool pickingUp;

        [SerializeField] private float rollTime = 0.25f;
        
        private static readonly int Pickup1 = Animator.StringToHash("Pickup");
        private static readonly int Reset = Animator.StringToHash("Reset");
        private static readonly int Dropped = Animator.StringToHash("Dropped");

        private Animator animator;
        private BoxCollider boxCollider;
        
        public bool Pickupable => pickupable;

        private void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            if (pickupable) return;
            boxCollider.isTrigger = false;
            UseGravity(true);
        }

        private IEnumerator StartRolling()
        {
            rolling = true;
            yield return new WaitForSeconds(rollTime);
            rolling = false;
        }

        private void FixedUpdate()
        {
            if (pickingUp)
            {
                transform.FixedSmoothMoveTowards(target, pickupSpeed);
            }

            if (inAir && rigidbody.IsSleeping())
            {
                rigidbody.WakeUp();
            }
        }

        protected override void TriggerEnterBehavior(Transform player, Collider other)
        {
            if (pickupable) AcquireObject();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.CompareTag("Ground") || !inAir) return;
            StartCoroutine(StartRolling());
        }

        private void OnCollisionStay(Collision other)
        {
            if (!other.CompareTag("Ground") || rolling) return;
            UseGravity(false);
        }

        public void MoveToTarget(GameObject target)
        {
            if (inAir) return;
            pickingUp = true;
            animator.SetTrigger(Pickup1);
            animator.ResetTrigger(Reset);
        }

        public void ResetIdle()
        {
            pickingUp = false;
            animator.ResetTrigger(Pickup1);
            animator.SetTrigger(Reset);
        }

        [SerializeField] private UnityEvent OnPickup = new UnityEvent();
        [SerializeField] private UnityEvent OnDrop = new UnityEvent();

        private void AcquireObject()
        {
            OnPickup.Invoke();

            // Destroy Object
            Destroy(gameObject, 1f);
        }

        public void TogglePickup(float time = 0f) => Invoke(nameof(EnablePickup), time);

        private void EnablePickup() => pickupable = true;

        private void UseGravity(bool use)
        {
            rigidbody.useGravity = use;
            inAir = use;
            animator.SetBool(Dropped, use);

            if (!use)
            {
                // Reset All Rotations/Velocities
                ResetDynamics();
                boxCollider.isTrigger = true;
            }
        }

        private void ResetDynamics()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }
}