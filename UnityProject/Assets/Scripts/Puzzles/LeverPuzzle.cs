using UnityEngine;
using TownWithoutLight.Interaction;

namespace TownWithoutLight.Puzzles
{
    /// <summary>
    /// Simple lever puzzle that requires the levers to match a target on/off pattern.
    /// Each interaction toggles the next lever to emulate the player walking between handles.
    /// </summary>
    public class LeverPuzzle : PuzzleBase, IInteractable
    {
        [SerializeField] private string description = "レバーを操作する";
        [SerializeField] private bool[] targetPattern = { true, false, true };

        private bool[] _currentPattern;
        private int _activeLeverIndex;

        private void Awake()
        {
            if (targetPattern == null || targetPattern.Length == 0)
            {
                targetPattern = new[] { true };
            }

            _currentPattern = new bool[targetPattern.Length];
        }

        public string GetDescription() => description;

        public bool CanInteract => !IsSolved;

        public void Interact(TownWithoutLight.Interaction.Interactor interactor)
        {
            if (IsSolved)
            {
                return;
            }

            _currentPattern[_activeLeverIndex] = !_currentPattern[_activeLeverIndex];
            _activeLeverIndex = (_activeLeverIndex + 1) % _currentPattern.Length;

            if (IsCorrect())
            {
                SolvePuzzle();
            }
        }

        private bool IsCorrect()
        {
            for (int i = 0; i < targetPattern.Length; i++)
            {
                if (_currentPattern[i] != targetPattern[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
