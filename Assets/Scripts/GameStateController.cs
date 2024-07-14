using UnityEngine;
using Elympics;
using Cysharp.Threading.Tasks;
using System;

namespace ShooterPbE
{
    public class GameStateController : ElympicsMonoBehaviour, IInitializable, IUpdatable
    {
        public int GAME_ENDING_TIME_THRESHOLD = 10000;

        public ElympicsInt CurrentGameState { get; } = new((int)GameState.Beginning);

        [SerializeField] private GameInitializer gameInitializer = null;
        [SerializeField] private PlayersManager playersProvider;

        public async void Initialize()
        {
            await UniTask.WaitUntil(() => playersProvider.IsReady);
            gameInitializer.InitializeMatch(() => ChangeGameState(GameState.Inprogress));
        }

        private void ChangeGameState(GameState newGameState)
        {
            CurrentGameState.Value = (int)newGameState;
        }

        public void ElympicsUpdate()
        {
            if (playersProvider.WinnerPlayerId.Value >= 0 && (GameState)CurrentGameState.Value == GameState.Inprogress)
            {
                ChangeGameState(GameState.Ending);
                EndGameDelay().Forget();
            }
        }

        private async UniTask EndGameDelay()
        {
            await UniTask.Delay(GAME_ENDING_TIME_THRESHOLD);
            Elympics.EndGame();
        }
    }
}
