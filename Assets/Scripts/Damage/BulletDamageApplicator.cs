using Elympics;
using UnityEngine;

namespace ShooterPbE.Damage
{
    public class BulletDamageApplicator : ElympicsMonoBehaviour
    {
        [SerializeField] private float damage = 1.0f;

        public void Detonate(StatsController statsController)
        {
            if (statsController == null)
            {
                return;
            }

            statsController.ChangeHealth(-damage);
        }
    }
}