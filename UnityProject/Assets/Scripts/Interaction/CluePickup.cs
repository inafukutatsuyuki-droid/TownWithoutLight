using UnityEngine;
using UnityEngine.Events;
using TownWithoutLight.Systems;

namespace TownWithoutLight.Interaction
{
    /// <summary>
    /// Specialized interactable for registering narrative clues.
    /// </summary>
    public class CluePickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private string clueId;
        [SerializeField] private string description = "手がかりを読む";
        [SerializeField] private UnityEvent onCollected;

        private void OnEnable()
        {
            if (!CanInteract)
            {
                gameObject.SetActive(false);
            }
        }

        public string GetDescription() => description;

        public bool CanInteract => ClueManager.Instance == null || !ClueManager.Instance.HasClue(clueId);

        public void Interact(Interactor interactor)
        {
            if (!CanInteract)
            {
                return;
            }

            ClueManager.Instance?.RegisterClue(clueId);
            onCollected?.Invoke();

            gameObject.SetActive(false);
        }
    }
}
