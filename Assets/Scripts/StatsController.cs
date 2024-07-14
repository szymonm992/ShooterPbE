using UnityEngine;
using System;
using Elympics;

namespace ShooterPbE
{
    public class StatsController : ElympicsMonoBehaviour, IInitializable
    {
        public event Action<float, float> HealthValueChanged = null;

        public bool IsDead => health.Value <= 0;
        public int PlayerId => playerId;

        [SerializeField] private float maxHealth = 100.0f;
        [SerializeField] private DeathController deathController = null;
        [SerializeField] private int playerId = 0;

        private ElympicsFloat health = new ElympicsFloat(0);
       
        public void Initialize()
        {
            health.Value = maxHealth;
            health.ValueChanged += OnHealthValueChanged;

            deathController.PlayerRespawned += ResetPlayerStats;
        }

        private void ResetPlayerStats()
        {
            health.Value = maxHealth;
        }

        public void ChangeHealth(float value, int damageOwner)
        {
            if (!Elympics.IsServer || deathController.IsDead.Value)
            {
                return;
            }

            health.Value += value;

            if (health.Value <= 0.0f)
            {
                deathController.ProcessPlayersDeath(damageOwner);
            }
        }

        private void OnHealthValueChanged(float lastValue, float newValue)
        {
            HealthValueChanged?.Invoke(newValue, maxHealth);
        }
    }
}
