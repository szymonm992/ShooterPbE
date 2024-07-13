using TMPro;
using UnityEngine;

namespace ShooterPbE.GUI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameWinnerText = null;
        [SerializeField] private CanvasGroup screenCanvasGroup = null;

        [SerializeField] private GameStateController gameStateController = null;
        [SerializeField] private PlayerScoresManager playerScoresManager = null;
        [SerializeField] private PlayersProvider playersProvider = null;

        private void Awake()
        {
            playerScoresManager.WinnerPlayerId.ValueChanged += SetWinnerInfo;

            gameStateController.CurrentGameState.ValueChanged += SetScreenDisplayBasedOnCurrentGameState;
        }

        private void SetWinnerInfo(int lastValue, int newValue)
        {
            var winnerData = playersProvider.GetPlayerById(newValue);

            gameWinnerText.text = $"Player {winnerData.transform.gameObject.name} won the game!";
        }

        private void SetScreenDisplayBasedOnCurrentGameState(int lastGameState, int newGameState)
        {
            screenCanvasGroup.alpha = (GameState)newGameState == GameState.Ending ? 1.0f : 0.0f;
        }
    }
}
