using System;
using UnityEngine;

namespace TownWithoutLight.Interaction
{
    /// <summary>
    /// Performs a forward raycast every frame to detect <see cref="IInteractable"/> instances and trigger them via the E key.
    /// </summary>
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private LayerMask interactMask = ~0;

        public event Action<IInteractable> OnFocused;
        public event Action OnFocusLost;
        public event Action<IInteractable> OnInteracted;

        private IInteractable _currentTarget;

        public IInteractable CurrentTarget => _currentTarget;

        private void Reset()
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            UpdateFocus();

            if (_currentTarget != null && _currentTarget.CanInteract && Input.GetKeyDown(KeyCode.E))
            {
                _currentTarget.Interact(this);
                OnInteracted?.Invoke(_currentTarget);
            }
        }

        private void UpdateFocus()
        {
            IInteractable newTarget = null;
            if (playerCamera != null)
            {
                Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask))
                {
                    newTarget = hit.collider.GetComponentInParent<IInteractable>();
                }
            }

            if (newTarget == _currentTarget)
            {
                return;
            }

            if (_currentTarget != null)
            {
                OnFocusLost?.Invoke();
            }

            _currentTarget = newTarget;

            if (_currentTarget != null)
            {
                OnFocused?.Invoke(_currentTarget);
            }
        }
    }
}
