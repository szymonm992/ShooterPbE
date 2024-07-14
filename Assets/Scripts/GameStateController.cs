using UnityEngine;
using Elympics;
using Cysharp.Threading.Tasks;

namespace ShooterPbE
{
    public class GameStateController : ElympicsMonoBehaviour, IInitializable, IUpdatable
    {
        [SerializeField] private GameInitializer gameInitializer = null;
        [SerializeField] private PlayersManager playersProvider;

        public ElympicsInt CurrentGameState { get; } = new ((int)GameState.Beginning);

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
                Elympics.EndGame();
            }
        }
    }
}
