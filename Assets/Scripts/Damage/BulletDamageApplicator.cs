using Elympics;
using UnityEngine;

namespace ShooterPbE.Damage
{
    public class BulletDamageApplicator : ElympicsMonoBehaviour
    {
        [SerializeField] private float damage = 1.0f;
        [SerializeField] private ElympicsMonoBehaviour owner;

        public void ApplyDamageTo(StatsController statsController)
        {
            if (statsController == null)
            {
                return;
            }

            statsController.ChangeHealth(-damage, (int)owner.PredictableFor);
        }
    }
}