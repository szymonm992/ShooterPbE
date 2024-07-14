using System;
using Elympics;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ShooterPbE
{
    public class PlayersManager : ElympicsMonoBehaviour, IInitializable
    {
        public event Action IsReadyChanged;

        public bool IsServer => Elympics.IsServer;
        public StatsController ClientPlayer { get; private set; } = null;
        public StatsController[] AllPlayersInScene { get; private set; } = null;
        public ElympicsInt WinnerPlayerId { get; } = new ElympicsInt(-1);
        public bool IsReady { get; private set; } = false;

        [SerializeField] private CustomServerHandler serverHandler;

        public async void Initialize()
        {
            await UniTask.WaitUntil(() => serverHandler.IsGameReady);

            FindAllPlayersInScene();
            FindClientPlayerInScene();
            SubscribeToDeathControllers();

            IsReady = true;
            IsReadyChanged?.Invoke();
        }

        public StatsController GetPlayerById(int id)
        {
            return AllPlayersInScene.FirstOrDefault(x => x.PlayerId == id);
        }

        private void FindAllPlayersInScene()
        {
            AllPlayersInScene = FindObjectsOfType<StatsController>().OrderBy(x => x.PlayerId).ToArray();
        }

        private void FindClientPlayerInScene()
        {
            foreach (var player in AllPlayersInScene)
            {
                if ((int)Elympics.Player == player.PlayerId)
                {
                    ClientPlayer = player;
                    return;
                }
            }

            ClientPlayer = !Elympics.IsServer ? AllPlayersInScene[0] : null;
        }

        private void ProcessPlayerDeath(int _, int killer)
        {
            if (Elympics.IsServer)
            {
                WinnerPlayerId.Value = killer;
            }
        }

        private void SubscribeToDeathControllers()
        {
            foreach (var stats in AllPlayersInScene)
            {
                if (stats.TryGetComponent(out DeathController deathController))
                {
                    deathController.HasBeenKilled += ProcessPlayerDeath;
                }
            }
        }
    }
}
