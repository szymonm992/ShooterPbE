using UnityEngine;
using Elympics;

namespace ShooterPbE
{
    public class GameStateController : ElympicsMonoBehaviour, IInitializable
    {
        [SerializeField] private GameInitializer gameInitializer = null;
        [SerializeField] private PlayerScoresManager playerScoresManager = null;

        public ElympicsInt CurrentGameState { get; } = new ElympicsInt((int)GameState.Beginning);

        public void Initialize()
        {
            gameInitializer.InitializeMatch(() => ChangeGameState(GameState.Inprogress));
        }

        private void ChangeGameState(GameState newGameState)
        {
            CurrentGameState.Value = (int)newGameState;
        }

        public void ElympicsUpdate()
        {
            if (playerScoresManager.WinnerPlayerId.Value >= 0 && (GameState)CurrentGameState.Value == GameState.Inprogress)
            {
                ChangeGameState(GameState.Ending);
            }
        }
    }
}
