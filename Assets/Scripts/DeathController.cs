using Elympics;
using UnityEngine;
using System;

namespace ShooterPbE
{
    public class DeathController : ElympicsMonoBehaviour, IUpdatable
    {
        public event Action<StatsController> PlayerRespawnEvent;
        public event Action PlayerRespawned = null;
        public event Action<int, int> HasBeenKilled = null;

        public ElympicsBool IsDead { get; } = new ElympicsBool(false);
        public ElympicsFloat CurrentDeathTime { get; } = new ElympicsFloat(0.0f);

        [SerializeField] private float deathTime = 2.0f;
        [SerializeField] private StatsController statsController;

        public void ProcessPlayersDeath(int damageOwner)
        {
            CurrentDeathTime.Value = deathTime;
            IsDead.Value = true;

            Debug.Log(this.gameObject.name + " has been killed!");

            HasBeenKilled?.Invoke((int)PredictableFor, damageOwner);
        }

        public void ElympicsUpdate()
        {
            if (!IsDead || !Elympics.IsServer)
                return;

            CurrentDeathTime.Value -= Elympics.TickDuration;

            if (CurrentDeathTime.Value <= 0)
            {
                RespawnPlayer();
            }
        }

        private void RespawnPlayer()
        {
            PlayersSpawner.Instance.SpawnPlayer(statsController);
            PlayerRespawned?.Invoke();
            IsDead.Value = false;
        }
    }
}
