using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TownWithoutLight.Systems;

namespace TownWithoutLight.UI
{
    /// <summary>
    /// Wires the "New Game" and "Continue" buttons to the GameState + SaveSystem flow.
    /// </summary>
    public class TitleScreenController : MonoBehaviour
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private string gameplaySceneName = "MainScene";

        private void Awake()
        {
            newGameButton.onClick.AddListener(StartNewGame);
            continueButton.onClick.AddListener(ContinueGame);
        }

        private void Start()
        {
            continueButton.interactable = SaveSystem.TryLoad(out _);
        }

        private void StartNewGame()
        {
            SaveSystem.Delete();
            LoadGameplayScene();
        }

        private void ContinueGame()
        {
            if (SaveSystem.TryLoad(out _))
            {
                LoadGameplayScene();
            }
        }

        private void LoadGameplayScene()
        {
            GameStateManager.Instance?.SetState(GameState.Exploring);
            SceneManager.LoadScene(gameplaySceneName);
        }
    }
}
