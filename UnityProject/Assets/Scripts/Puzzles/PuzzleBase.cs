using UnityEngine;
using UnityEngine.Events;
using TownWithoutLight.Systems;

namespace TownWithoutLight.Puzzles
{
    /// <summary>
    /// Shared base logic for puzzle components so they can report completion to the <see cref="PuzzleManager"/>.
    /// </summary>
    public abstract class PuzzleBase : MonoBehaviour
    {
        [SerializeField] private string puzzleId;
        [SerializeField] private UnityEvent onSolved;

        public bool IsSolved { get; private set; }

        protected PuzzleManager PuzzleManager => PuzzleManager.Instance;

        protected void SolvePuzzle()
        {
            if (IsSolved)
            {
                return;
            }

            IsSolved = true;
            onSolved?.Invoke();
            if (PuzzleManager != null)
            {
                PuzzleManager.ReportPuzzleSolved(puzzleId);
            }
        }

        public string PuzzleId => puzzleId;

        public virtual void LoadSolvedState(bool solved)
        {
            IsSolved = solved;
            if (solved)
            {
                onSolved?.Invoke();
            }
        }
    }
}
