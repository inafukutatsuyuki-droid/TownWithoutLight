using UnityEngine;
using UnityEngine.SceneManagement;
using TownWithoutLight.Puzzles;

namespace TownWithoutLight.Systems
{
    public enum EndingType
    {
        Hopeful,
        Neutral,
        Tragic
    }

    /// <summary>
    /// Evaluates the current progression state and loads the appropriate ending scene.
    /// </summary>
    public class EndingManager : MonoBehaviour
    {
        [SerializeField] private int hopefulClueThreshold = 4;
        [SerializeField] private int hopefulPuzzleThreshold = 3;
        [SerializeField] private int tragicPuzzleThreshold = 1;
        [SerializeField] private string hopefulScene = "Ending_Hopeful";
        [SerializeField] private string neutralScene = "Ending_Neutral";
        [SerializeField] private string tragicScene = "Ending_Tragic";

        public void TriggerEnding()
        {
            EndingType ending = EvaluateEnding();
            LoadEndingScene(ending);
        }

        private EndingType EvaluateEnding()
        {
            int clueCount = ClueManager.Instance != null ? ClueManager.Instance.ClueCount : 0;
            int solvedPuzzles = PuzzleManager.Instance != null ? PuzzleManager.Instance.SolvedCount : 0;

            if (clueCount >= hopefulClueThreshold && solvedPuzzles >= hopefulPuzzleThreshold)
            {
                return EndingType.Hopeful;
            }

            if (solvedPuzzles <= tragicPuzzleThreshold)
            {
                return EndingType.Tragic;
            }

            return EndingType.Neutral;
        }

        private void LoadEndingScene(EndingType ending)
        {
            GameStateManager.Instance?.SetState(GameState.Ending);

            string sceneName = ending switch
            {
                EndingType.Hopeful => hopefulScene,
                EndingType.Tragic => tragicScene,
                _ => neutralScene
            };

            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
