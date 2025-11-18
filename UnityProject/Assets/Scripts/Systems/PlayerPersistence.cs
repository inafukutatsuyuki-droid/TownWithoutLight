using UnityEngine;
using TownWithoutLight.Player;

namespace TownWithoutLight.Systems
{
    /// <summary>
    /// Simple component that serializes player position + rotation using the <see cref="SaveSystem"/>.
    /// </summary>
    public class PlayerPersistence : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Transform playerRoot;

        private void Reset()
        {
            playerController = FindObjectOfType<PlayerController>();
            playerRoot = playerController != null ? playerController.transform : null;
        }

        public void Save()
        {
            if (playerRoot == null)
            {
                return;
            }

            var data = new GameSaveData
            {
                playerPosition = playerRoot.position,
                playerRotation = playerRoot.rotation,
                checkpointId = string.Empty
            };

            SaveSystem.Save(data);
        }

        public void Load()
        {
            if (playerRoot == null)
            {
                return;
            }

            if (SaveSystem.TryLoad(out var data))
            {
                playerRoot.position = data.playerPosition;
                playerRoot.rotation = data.playerRotation;
            }
        }
    }
}
