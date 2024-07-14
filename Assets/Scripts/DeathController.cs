using Elympics;
using UnityEngine;
using System;

namespace ShooterPbE
{
    public class DeathController : ElympicsMonoBehaviour
    {
        public event Action<int, int> HasBeenKilled;

        public ElympicsBool IsDead { get; } = new ElympicsBool(false);

        [SerializeField] private StatsController statsController;

        public void ProcessPlayersDeath(int damageOwner)
        {
            IsDead.Value = true;
            HasBeenKilled?.Invoke((int)PredictableFor, damageOwner);
        }
    }
}
