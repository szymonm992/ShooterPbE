using UnityEngine;
using System;
using Elympics;

namespace ShooterPbE
{
    public class StatsController : ElympicsMonoBehaviour, IInitializable
    {
        public event Action<float, float> HealthValueChangedEvent;
        public event Action DiedEvent;

        public bool IsDead => currentHealth <= 0;

        [SerializeField] private float maxHealth = 100.0f;

        private ElympicsFloat currentHealth = new ElympicsFloat(0);

        public void Initialize()
        {
            currentHealth.Value = maxHealth;
            currentHealth.ValueChanged += OnHealthValueChanged;
        }

        public void ChangeHealth(float value)
        {
            if (!Elympics.IsServer || IsDead)
            {
                return;
            }
            Debug.Log($"prev: {currentHealth}");
            currentHealth.Value += value;
            Debug.Log($"aft: {currentHealth}");
            if (currentHealth.Value <= 0.0f)
            {
                DiedEvent?.Invoke();
            }
        }

        private void OnHealthValueChanged(float _, float newValue)
        {
            HealthValueChangedEvent?.Invoke(newValue, maxHealth);
        }
    }
}
