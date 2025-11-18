using System;
using System.Collections.Generic;
using UnityEngine;

namespace TownWithoutLight.Puzzles
{
    /// <summary>
    /// Central registry that keeps track of solved puzzle identifiers for save/load and progression logic.
    /// </summary>
    public class PuzzleManager : MonoBehaviour
    {
        public static PuzzleManager Instance { get; private set; }

        public event Action<int> OnSolvedCountChanged;

        private readonly HashSet<string> _solvedPuzzles = new HashSet<string>();

        public int SolvedCount => _solvedPuzzles.Count;

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
            RefreshScenePuzzles();
        }

        public void ReportPuzzleSolved(string puzzleId)
        {
            if (string.IsNullOrEmpty(puzzleId) || _solvedPuzzles.Contains(puzzleId))
            {
                return;
            }

            _solvedPuzzles.Add(puzzleId);
            OnSolvedCountChanged?.Invoke(_solvedPuzzles.Count);
        }

        public bool IsPuzzleSolved(string puzzleId)
        {
            if (string.IsNullOrEmpty(puzzleId))
            {
                return false;
            }

            return _solvedPuzzles.Contains(puzzleId);
        }

        public string[] GetSolvedPuzzles()
        {
            string[] result = new string[_solvedPuzzles.Count];
            _solvedPuzzles.CopyTo(result);
            return result;
        }

        public void LoadSolvedPuzzles(IEnumerable<string> puzzleIds)
        {
            _solvedPuzzles.Clear();
            if (puzzleIds == null)
            {
                return;
            }

            foreach (string id in puzzleIds)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    _solvedPuzzles.Add(id);
                }
            }

            OnSolvedCountChanged?.Invoke(_solvedPuzzles.Count);
            RefreshScenePuzzles();
        }

        public void RefreshScenePuzzles()
        {
            var puzzles = FindObjectsOfType<PuzzleBase>(true);
            for (int i = 0; i < puzzles.Length; i++)
            {
                var puzzle = puzzles[i];
                bool solved = IsPuzzleSolved(puzzle.PuzzleId);
                puzzle.LoadSolvedState(solved);
            }
        }
    }
}
