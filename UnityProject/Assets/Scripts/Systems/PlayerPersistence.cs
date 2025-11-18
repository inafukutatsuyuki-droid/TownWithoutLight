using System;
using UnityEngine;
using TownWithoutLight.Player;
using TownWithoutLight.Puzzles;

namespace TownWithoutLight.Systems
{
    /// <summary>
    /// Simple component that serializes player position + rotation using the <see cref="SaveSystem"/> while also persisting progression systems.
    /// </summary>
    public class PlayerPersistence : MonoBehaviour
    {
        [SerializeField] private Flashlight flashlight;
        [SerializeField] private Transform playerRoot;
        [SerializeField] private ChapterStateMachine chapterStateMachine;

        private void Reset()
        {
            var controller = FindObjectOfType<PlayerController>();
            playerRoot = controller != null ? controller.transform : null;
            flashlight = FindObjectOfType<Flashlight>();
            chapterStateMachine = FindObjectOfType<ChapterStateMachine>();
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
                checkpointId = string.Empty,
                collectedFlags = FlagManager.Instance != null ? FlagManager.Instance.GetAllFlags() : Array.Empty<string>(),
                collectedClues = ClueManager.Instance != null ? ClueManager.Instance.GetCollectedClues() : Array.Empty<string>(),
                solvedPuzzles = PuzzleManager.Instance != null ? PuzzleManager.Instance.GetSolvedPuzzles() : Array.Empty<string>(),
                chapterState = chapterStateMachine != null ? chapterStateMachine.CurrentChapter : ChapterState.Prologue,
                flashlightBattery = flashlight != null ? flashlight.BatteryNormalized : 1f,
                flashlightEnabled = flashlight != null && flashlight.IsOn
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

                FlagManager.Instance?.LoadFlags(data.collectedFlags);
                ClueManager.Instance?.LoadClues(data.collectedClues);
                PuzzleManager.Instance?.LoadSolvedPuzzles(data.solvedPuzzles);
                if (chapterStateMachine != null)
                {
                    chapterStateMachine.SetChapter(data.chapterState);
                }

                if (flashlight != null)
                {
                    flashlight.RestoreBattery(data.flashlightBattery);
                    flashlight.SetActive(data.flashlightEnabled);
                }
            }
        }
    }
}
