using UnityEngine;

namespace TownWithoutLight.Interaction
{
    /// <summary>
    /// Interface that exposes the minimum data and hooks needed for the raycast-based interaction flow.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Returns a localized string that can be rendered on the HUD when the player looks at this object.
        /// </summary>
        string GetDescription();

        /// <summary>
        /// Called by the <see cref="Interactor"/> when the player confirms the interaction.
        /// </summary>
        void Interact(Interactor interactor);

        /// <summary>
        /// Indicates whether the object currently accepts interaction.
        /// </summary>
        bool CanInteract { get; }
    }
}
