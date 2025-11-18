using UnityEngine;
using UnityEngine.Events;
using TownWithoutLight.Player;
using TownWithoutLight.Systems;

namespace TownWithoutLight.UI
{
    /// <summary>
    /// Handles pause/unpause behaviour and exposes hooks for resume/quit buttons.
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private PlayerLook playerLook;
        [SerializeField] private UnityEvent onResumed;

        private bool _initialized;

        private void Awake()
        {
            if (root != null)
            {
                root.SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            if (GameStateManager.Instance == null)
            {
                return;
            }

            if (GameStateManager.Instance.CurrentState == GameState.Paused)
            {
                ResumeGame();
            }
            else if (GameStateManager.Instance.CurrentState == GameState.Exploring)
            {
                PauseGame();
            }
        }

        public void PauseGame()
        {
            GameStateManager.Instance?.SetState(GameState.Paused);
            if (root != null)
            {
                root.SetActive(true);
            }

            EnsurePlayerLook();
            playerLook?.ApplyCursorState(false);
        }

        public void ResumeGame()
        {
            GameStateManager.Instance?.SetState(GameState.Exploring);
            if (root != null)
            {
                root.SetActive(false);
            }

            EnsurePlayerLook();
            playerLook?.ApplyCursorState(true);
            onResumed?.Invoke();
        }

        private void EnsurePlayerLook()
        {
            if (_initialized)
            {
                return;
            }

            if (playerLook == null)
            {
                playerLook = FindObjectOfType<PlayerLook>();
            }

            _initialized = true;
        }
    }
}
