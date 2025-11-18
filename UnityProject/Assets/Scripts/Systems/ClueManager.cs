using System;
using System.Collections.Generic;
using UnityEngine;
using TownWithoutLight.Interaction;

namespace TownWithoutLight.Systems
{
    /// <summary>
    /// Keeps track of discovered clues and exposes events so that level logic can react in real-time.
    /// </summary>
    public class ClueManager : MonoBehaviour
    {
        public static ClueManager Instance { get; private set; }

        public event Action<int> OnClueCountChanged;

        private readonly HashSet<string> _collectedClues = new HashSet<string>();

        public int ClueCount => _collectedClues.Count;

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

        private void Start()
        {
            RefreshSceneClues();
        }

        public bool HasClue(string clueId)
        {
            if (string.IsNullOrEmpty(clueId))
            {
                return false;
            }

            return _collectedClues.Contains(clueId);
        }

        public void RegisterClue(string clueId)
        {
            if (string.IsNullOrEmpty(clueId) || _collectedClues.Contains(clueId))
            {
                return;
            }

            _collectedClues.Add(clueId);
            OnClueCountChanged?.Invoke(_collectedClues.Count);
            RefreshSceneClues();
        }

        public string[] GetCollectedClues()
        {
            string[] result = new string[_collectedClues.Count];
            _collectedClues.CopyTo(result);
            return result;
        }

        public void LoadClues(IEnumerable<string> clues)
        {
            _collectedClues.Clear();
            if (clues == null)
            {
                return;
            }

            foreach (string clue in clues)
            {
                if (!string.IsNullOrEmpty(clue))
                {
                    _collectedClues.Add(clue);
                }
            }

            OnClueCountChanged?.Invoke(_collectedClues.Count);
            RefreshSceneClues();
        }

        public void RefreshSceneClues()
        {
            var cluePickups = FindObjectsOfType<CluePickup>(true);
            for (int i = 0; i < cluePickups.Length; i++)
            {
                var pickup = cluePickups[i];
                if (!pickup.CanInteract)
                {
                    pickup.gameObject.SetActive(false);
                }
            }
        }
    }
}
