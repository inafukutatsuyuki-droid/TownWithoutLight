using UnityEngine;
using UnityEngine.UI;
using TownWithoutLight.Interaction;

namespace TownWithoutLight.UI
{
    /// <summary>
    /// Displays the contextual interaction prompt when the player looks at usable objects.
    /// </summary>
    public class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField] private Interactor interactor;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Text promptLabel;
        [SerializeField] private string format = "[E] {0}";

        private void Awake()
        {
            SetVisible(false);
        }

        private void OnEnable()
        {
            if (interactor == null)
            {
                interactor = FindObjectOfType<Interactor>();
            }

            if (interactor != null)
            {
                interactor.OnFocused += HandleFocused;
                interactor.OnFocusLost += HandleFocusLost;
            }
        }

        private void OnDisable()
        {
            if (interactor != null)
            {
                interactor.OnFocused -= HandleFocused;
                interactor.OnFocusLost -= HandleFocusLost;
            }
        }

        private void HandleFocused(IInteractable interactable)
        {
            if (promptLabel != null && interactable != null)
            {
                promptLabel.text = string.Format(format, interactable.GetDescription());
            }

            SetVisible(true);
        }

        private void HandleFocusLost()
        {
            SetVisible(false);
        }

        private void SetVisible(bool visible)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = visible ? 1f : 0f;
                canvasGroup.blocksRaycasts = visible;
                canvasGroup.interactable = visible;
            }
        }
    }
}
