using UnityEngine;
using Elympics;
using System;

namespace ShooterPbE.Weapons
{
    public abstract class WeaponBase : ElympicsMonoBehaviour, IInitializable, IUpdatable
    {
        private const float SECONDS_IN_MINUTE = 60f;
        protected bool CanShoot => shootingTimer >= timeBetweenShots;
        public float TimeBetweenShoots => timeBetweenShots;
        public GameObject Owner => this.transform.root.gameObject;

        [SerializeField] protected float shotsPerMinute = 180f;

        protected ElympicsFloat shootingTimer = new ElympicsFloat();
        protected float timeBetweenShots = 0.0f;

        public void Initialize()
        {
            RecalculateFiringRate();
        }

        public virtual void ElympicsUpdate()
        {
            if (!CanShoot)
            {
                shootingTimer.Value += Elympics.TickDuration;
            }
        }

        public void RecalculateFiringRate()
        {
            if (shotsPerMinute > 0)
            {
                timeBetweenShots = SECONDS_IN_MINUTE / shotsPerMinute;
            }
            else
            {
                timeBetweenShots = 0f;
            }
        }

        public void TryShoot(int playerId)
        {
            ShootInternal(playerId);
        }

        protected abstract void PerformWeaponShoot(int playerId);

        private void ShootInternal(int playerId)
        {
            if (CanShoot)
            {
                PerformWeaponShoot(playerId);
                shootingTimer.Value = 0f;
            }
        }
    }
}
