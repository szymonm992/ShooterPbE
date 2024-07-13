using UnityEngine;
using Elympics;
using System;

namespace ShooterPbE
{
    public class PlayerScoresManager : ElympicsMonoBehaviour, IInitializable
    {
        public event Action IsReadyChanged;

        public ElympicsInt WinnerPlayerId { get; } = new ElympicsInt(-1);
        public bool IsReady { get; private set; } = false;

        [SerializeField] private PlayersProvider playersProvider = null;
        [SerializeField] private int pointsRequiredToWin = 10;

        private ElympicsArray<ElympicsInt> playerScores = null;
       
        public void Initialize()
        {
            if (playersProvider.IsReady)
            {
                SetupManager();
            }
            else
            {
                playersProvider.IsReadyChanged += SetupManager;
            }
        }

        private void SetupManager()
        {
            PreparePlayerScores();
            SubscribeToDeathControllers();

            IsReady = true;
            IsReadyChanged?.Invoke();
        }

        private void SubscribeToDeathControllers()
        {
            foreach (var stats in playersProvider.AllPlayersInScene)
            {
                if (stats.TryGetComponent(out DeathController deathController))
                {
                    deathController.HasBeenKilled += ProcessPlayerDeath;
                }
            }
        }

        private void ProcessPlayerDeath(int victim, int killer)
        {
            if (victim == killer)
            {
                playerScores.Values[killer].Value--;
            }
            else
            {
                playerScores.Values[killer].Value++;
            }

            if (Elympics.IsServer && playerScores.Values[killer].Value >= pointsRequiredToWin)
            {
                WinnerPlayerId.Value = killer;
            }
        }

        private void PreparePlayerScores()
        {
            var numberOfPlayers = playersProvider.AllPlayersInScene.Length;

            var localPlayerScoresArray = new ElympicsInt[numberOfPlayers];

            for (int i = 0; i < numberOfPlayers; i++)
            {
                localPlayerScoresArray[i] = new ElympicsInt(0);
            }

            playerScores = new (localPlayerScoresArray);
        }
    }
}
