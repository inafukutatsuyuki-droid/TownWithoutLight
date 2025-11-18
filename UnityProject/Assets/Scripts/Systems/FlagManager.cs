using System.Collections.Generic;
using UnityEngine;
using TownWithoutLight.Interaction;

namespace TownWithoutLight.Systems
{
    /// <summary>
    /// Tracks simple string-based progression flags such as collected items or puzzle states.
    /// </summary>
    public class FlagManager : MonoBehaviour
    {
        public static FlagManager Instance { get; private set; }

        private readonly HashSet<string> _flags = new HashSet<string>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            RefreshSceneItems();
        }

        public bool HasFlag(string flagId)
        {
            if (string.IsNullOrEmpty(flagId))
            {
                return false;
            }

            return _flags.Contains(flagId);
        }

        public void SetFlag(string flagId, bool value)
        {
            if (string.IsNullOrEmpty(flagId))
            {
                return;
            }

            if (value)
            {
                _flags.Add(flagId);
            }
            else
            {
                _flags.Remove(flagId);
            }

            RefreshSceneItems();
        }

        public string[] GetAllFlags()
        {
            string[] result = new string[_flags.Count];
            _flags.CopyTo(result);
            return result;
        }

        public void LoadFlags(IEnumerable<string> flags)
        {
            _flags.Clear();
            if (flags == null)
            {
                return;
            }

            foreach (string flag in flags)
            {
                if (!string.IsNullOrEmpty(flag))
                {
                    _flags.Add(flag);
                }
            }

            RefreshSceneItems();
        }

        public void RefreshSceneItems()
        {
            var items = FindObjectsOfType<CollectibleItem>(true);
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (!item.CanInteract)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
}
