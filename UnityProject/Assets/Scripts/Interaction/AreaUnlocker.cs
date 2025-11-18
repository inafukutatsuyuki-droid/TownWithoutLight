using UnityEngine;
using UnityEngine.Events;
using TownWithoutLight.Systems;

namespace TownWithoutLight.Interaction
{
    /// <summary>
    /// Enables area access after the player has collected a required number of clues.
    /// </summary>
    public class AreaUnlocker : MonoBehaviour
    {
        [SerializeField] private int requiredClues = 3;
        [SerializeField] private GameObject[] blockers;
        [SerializeField] private UnityEvent onUnlocked;

        private bool _isUnlocked;

        private void OnEnable()
        {
            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.OnClueCountChanged += HandleClueCountChanged;
                HandleClueCountChanged(ClueManager.Instance.ClueCount);
            }
        }

        private void OnDisable()
        {
            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.OnClueCountChanged -= HandleClueCountChanged;
            }
        }

        private void HandleClueCountChanged(int count)
        {
            if (_isUnlocked || count < requiredClues)
            {
                return;
            }

            _isUnlocked = true;
            foreach (var blocker in blockers)
            {
                if (blocker != null)
                {
                    blocker.SetActive(false);
                }
            }

            onUnlocked?.Invoke();
        }
    }
}
