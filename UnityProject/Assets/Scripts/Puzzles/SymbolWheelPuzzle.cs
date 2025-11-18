using TownWithoutLight.Interaction;
using UnityEngine;

namespace TownWithoutLight.Puzzles
{
    /// <summary>
    /// Rotates a set of wheels until each reaches the target symbol index.
    /// </summary>
    public class SymbolWheelPuzzle : PuzzleBase, IInteractable
    {
        [SerializeField] private string description = "シンボルを合わせる";
        [SerializeField] private int symbolCount = 4;
        [SerializeField] private int[] targetSymbols = { 1, 2, 3 };

        private int[] _currentSymbols;
        private int _activeWheelIndex;

        private void Awake()
        {
            if (targetSymbols == null || targetSymbols.Length == 0)
            {
                targetSymbols = new[] { 0 };
            }

            _currentSymbols = new int[targetSymbols.Length];
        }

        public string GetDescription() => description;

        public bool CanInteract => !IsSolved;

        public void Interact(Interactor interactor)
        {
            if (IsSolved)
            {
                return;
            }

            _currentSymbols[_activeWheelIndex] = (_currentSymbols[_activeWheelIndex] + 1) % Mathf.Max(1, symbolCount);
            _activeWheelIndex = (_activeWheelIndex + 1) % _currentSymbols.Length;

            if (MatchesTarget())
            {
                SolvePuzzle();
            }
        }

        private bool MatchesTarget()
        {
            for (int i = 0; i < targetSymbols.Length; i++)
            {
                if (_currentSymbols[i] != targetSymbols[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
