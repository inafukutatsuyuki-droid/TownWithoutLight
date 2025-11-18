using System;
using UnityEngine;

namespace TownWithoutLight.Systems
{
    public enum GameState
    {
        Title,
        Exploring,
        Paused,
        Cutscene,
        Puzzle,
        Ending
    }

    /// <summary>
    /// Lightweight singleton-style state machine that notifies listeners when the global gameplay state changes.
    /// </summary>
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Title;

        public event Action<GameState> OnStateChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetState(GameState newState)
        {
            if (newState == CurrentState)
            {
                return;
            }

            CurrentState = newState;
            Time.timeScale = CurrentState == GameState.Paused ? 0f : 1f;
            OnStateChanged?.Invoke(newState);
        }

        public void TogglePause()
        {
            if (CurrentState == GameState.Paused)
            {
                SetState(GameState.Exploring);
            }
            else if (CurrentState == GameState.Exploring)
            {
                SetState(GameState.Paused);
            }
        }
    }
}
