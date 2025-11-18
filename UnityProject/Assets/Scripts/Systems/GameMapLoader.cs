using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TownWithoutLight.Systems
{
    /// <summary>
    /// Loads the playable map scenes in sequence to ensure the world is ready for continuous playthroughs.
    /// </summary>
    public class GameMapLoader : MonoBehaviour
    {
        [SerializeField] private string[] additiveScenes;
        [SerializeField] private bool loadOnStart = true;

        private void Start()
        {
            if (loadOnStart)
            {
                StartCoroutine(LoadScenesRoutine());
            }
        }

        private IEnumerator LoadScenesRoutine()
        {
            for (int i = 0; i < additiveScenes.Length; i++)
            {
                string sceneName = additiveScenes[i];
                if (string.IsNullOrEmpty(sceneName))
                {
                    continue;
                }

                if (!SceneManager.GetSceneByName(sceneName).isLoaded)
                {
                    AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    while (op != null && !op.isDone)
                    {
                        yield return null;
                    }
                }
            }

            GameStateManager.Instance?.SetState(GameState.Exploring);
        }
    }
}
