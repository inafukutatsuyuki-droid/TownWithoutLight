using UnityEngine;
using UnityEngine.Events;
using TownWithoutLight.Systems;

namespace TownWithoutLight.Interaction
{
    /// <summary>
    /// Generic pick-up item that toggles a progression flag when collected.
    /// </summary>
    public class CollectibleItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private string itemId;
        [SerializeField] private string description = "拾う";
        [SerializeField] private bool hideAfterPickup = true;
        [SerializeField] private UnityEvent onCollected;

        private void Start()
        {
            if (!CanInteract && hideAfterPickup)
            {
                gameObject.SetActive(false);
            }
        }

        public string GetDescription() => description;

        public bool CanInteract
        {
            get
            {
                if (FlagManager.Instance == null)
                {
                    return true;
                }

                return !FlagManager.Instance.HasFlag(itemId);
            }
        }

        public void Interact(Interactor interactor)
        {
            if (!CanInteract)
            {
                return;
            }

            FlagManager.Instance?.SetFlag(itemId, true);
            onCollected?.Invoke();

            if (hideAfterPickup)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
