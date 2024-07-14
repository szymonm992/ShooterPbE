using TMPro;
using UnityEngine;

namespace ShooterPbE.GUI
{
    public class InitializationScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countdownToStartMatchText = null;
        [SerializeField] private CanvasGroup screenCanvasGroup = null;
        [SerializeField] private GameStateController gameStateController = null;
        [SerializeField] private GameInitializer gameInitializer = null;

        private void Awake()
        {
            gameInitializer.CurrentTimeToStartMatch.ValueChanged += UpdateTimeToStartMatchDisplay;

            ProcessScreenViewAtStartOfTheGame();
        }

        private void ProcessScreenViewAtStartOfTheGame()
        {
            SetScreenDisplayBasedOnCurrentGameState(-1, gameStateController.CurrentGameState.Value);
            gameStateController.CurrentGameState.ValueChanged += SetScreenDisplayBasedOnCurrentGameState;
        }

        private void UpdateTimeToStartMatchDisplay(float lastValue, float newValue)
        {
            countdownToStartMatchText.text = Mathf.Ceil(newValue).ToString();
        }

        private void SetScreenDisplayBasedOnCurrentGameState(int lastGameState, int newGameState)
        {
            screenCanvasGroup.alpha = (GameState)newGameState == GameState.Beginning ? 1.0f : 0.0f;
        }
    }
}
