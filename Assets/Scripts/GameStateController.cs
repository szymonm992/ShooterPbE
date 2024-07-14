using UnityEngine;
using Elympics;
using Cysharp.Threading.Tasks;
using System;

namespace ShooterPbE
{
    public class GameStateController : ElympicsMonoBehaviour, IInitializable, IUpdatable
    {
        private const float ENDING_GAME_TIME_THRESHOLD = 5f;

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
                EndGameAfterTime(ENDING_GAME_TIME_THRESHOLD).Forget();
            }
        }

        private async UniTask EndGameAfterTime(float seconds)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds), true);
            Elympics.EndGame();
        }
    }
}
