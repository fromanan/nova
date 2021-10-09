using Nova.Library.Derivables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Nova.Library.Interactions
{
    public class Interact : PlayerTriggerBehavior
    {
        [Header("Interaction Events")]
        [SerializeField] private UnityEvent OnEnterInteractionRange = new UnityEvent();
        [SerializeField] private UnityEvent OnExitInteractionRange = new UnityEvent();
        [SerializeField] private UnityEvent OnStartInteraction = new UnityEvent();
        [SerializeField] private UnityEvent OnStopInteraction = new UnityEvent();
        
        [Header("GUI Interactions")]
        [SerializeField] private InputAction InteractAction;
        [SerializeField] private InputAction ExitAction;

        public bool Interactible { get; private set; }
        
        public bool PlayerInRange { get; private set; }
        
        public bool Interacting { get; private set; }

        private void OnEnable()
        {
            InteractAction.performed += OnInteract;
            ExitAction.performed += OnExit;
        }

        private void OnDisable()
        {
            InteractAction.performed -= OnInteract;
            ExitAction.performed -= OnExit;
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (Interactible && PlayerInRange) StartInteraction();
        }

        private void OnExit(InputAction.CallbackContext context)
        {
            if (Interacting) StopInteraction();
        }

        protected virtual void StartInteraction()
        {
            OnStartInteraction.Invoke();
            Interacting = true;
        }

        protected virtual void StopInteraction()
        {
            OnStopInteraction.Invoke();
            Interacting = false;
        }

        protected override void TriggerEnterBehavior(Transform player, Collider other)
        {
            OnEnterInteractionRange.Invoke();
            PlayerInRange = true;
        }

        protected override void TriggerExitBehavior(Transform player, Collider other)
        {
            OnExitInteractionRange.Invoke();
            PlayerInRange = false;
        }

        public void UnblockInteractions()
        {
            Interactible = true;
        }

        public void BlockInteractions()
        {
            Interactible = false;
        }
    }
}