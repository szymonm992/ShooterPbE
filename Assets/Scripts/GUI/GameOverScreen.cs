using TMPro;
using UnityEngine;

namespace ShooterPbE.GUI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameWinnerText = null;
        [SerializeField] private TextMeshProUGUI titleText = null;
        [SerializeField] private CanvasGroup screenCanvasGroup = null;
        [SerializeField] private GameStateController gameStateController = null;
        [SerializeField] private PlayersManager playersManager = null;

        private void Start()
        {
            if (playersManager.IsServer)
            {
                return;
            }

            playersManager.WinnerPlayerId.ValueChanged += SetWinnerInfo;
            gameStateController.CurrentGameState.ValueChanged += SetScreenDisplayBasedOnCurrentGameState;
        }

        private void OnDestroy()
        {
            if (playersManager.IsServer)
            {
                return;
            }

            playersManager.WinnerPlayerId.ValueChanged -= SetWinnerInfo;
            gameStateController.CurrentGameState.ValueChanged -= SetScreenDisplayBasedOnCurrentGameState;
        }

        private void SetWinnerInfo(int lastValue, int newValue)
        {
            var winnerData = playersManager.GetPlayerById(newValue);

            if (playersManager.ClientPlayer == winnerData)
            {
                titleText.text = "You WIN!";
                gameWinnerText.text = "Congratulations!";
            }
            else
            {
                titleText.text = "You LOSE!";
                gameWinnerText.text = $"Player {winnerData.transform.gameObject.name} has won the game!";
            }
        }

        private void SetScreenDisplayBasedOnCurrentGameState(int lastGameState, int newGameState)
        {
            screenCanvasGroup.alpha = (GameState)newGameState == GameState.Ending ? 1.0f : 0.0f;
        }
    }
}
