using System;
using UnityEngine;

namespace TownWithoutLight.Interaction
{
    /// <summary>
    /// Basic E-key investigation point that triggers a UnityEvent and optional narrative text.
    /// </summary>
    public class InvestigationPoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private string description = "調べる";
        [SerializeField] private bool singleUse = true;

        public event Action OnInvestigated;

        private bool _hasBeenUsed;

        public string GetDescription() => description;

        public bool CanInteract => !singleUse || !_hasBeenUsed;

        public void Interact(Interactor interactor)
        {
            if (!CanInteract)
            {
                return;
            }

            _hasBeenUsed = true;
            OnInvestigated?.Invoke();
        }
    }
}
