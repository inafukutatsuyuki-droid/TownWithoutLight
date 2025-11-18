using System;
using UnityEngine;
using TownWithoutLight.Puzzles;

namespace TownWithoutLight.Systems
{
    public enum ChapterState
    {
        Prologue,
        Investigation,
        PuzzleRush,
        Escape,
        Ending
    }

    /// <summary>
    /// Drives the high-level chapter progression based on clues and puzzle completion counts.
    /// </summary>
    public class ChapterStateMachine : MonoBehaviour
    {
        [SerializeField] private int investigationClueThreshold = 1;
        [SerializeField] private int puzzleRushSolveThreshold = 2;
        [SerializeField] private int escapeClueThreshold = 4;

        public ChapterState CurrentChapter { get; private set; } = ChapterState.Prologue;

        public event Action<ChapterState> OnChapterChanged;

        private void OnEnable()
        {
            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.OnClueCountChanged += HandleClueCountChanged;
            }

            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.OnSolvedCountChanged += HandlePuzzleCountChanged;
            }
        }

        private void OnDisable()
        {
            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.OnClueCountChanged -= HandleClueCountChanged;
            }

            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.OnSolvedCountChanged -= HandlePuzzleCountChanged;
            }
        }

        public void SetChapter(ChapterState newChapter)
        {
            if (CurrentChapter == newChapter)
            {
                return;
            }

            CurrentChapter = newChapter;
            OnChapterChanged?.Invoke(newChapter);
        }

        private void HandleClueCountChanged(int clues)
        {
            if (CurrentChapter == ChapterState.Prologue && clues >= investigationClueThreshold)
            {
                SetChapter(ChapterState.Investigation);
            }
            else if (CurrentChapter == ChapterState.Investigation && clues >= escapeClueThreshold)
            {
                SetChapter(ChapterState.Escape);
            }
        }

        private void HandlePuzzleCountChanged(int solved)
        {
            if (CurrentChapter == ChapterState.Investigation && solved >= puzzleRushSolveThreshold)
            {
                SetChapter(ChapterState.PuzzleRush);
            }
            else if (CurrentChapter == ChapterState.PuzzleRush && ClueManager.Instance != null && ClueManager.Instance.ClueCount >= escapeClueThreshold)
            {
                SetChapter(ChapterState.Escape);
            }
        }
    }
}
