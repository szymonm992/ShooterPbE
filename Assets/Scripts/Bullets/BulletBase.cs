using UnityEngine;
using Elympics;
using ShooterPbE.Damage;

namespace ShooterPbE.Bullets
{
    public abstract class BulletBase : ElympicsMonoBehaviour, IUpdatable
    {
        private const float COLLISION_DETECTION_DISTANCE = 1f;

        public float LifeTime => lifeTime;

        [Header("Parameters:")]
        [SerializeField] protected float speed = 15.0f;
        [SerializeField] protected float lifeTime = 5.0f;

        [Header("References:")]
        [SerializeField] protected new Rigidbody rigidbody = null;
        [SerializeField] protected BulletDamageApplicator damageApplicator = null;
        [SerializeField] protected LayerMask shellCollisionMask;

        protected ElympicsGameObject owner = new ElympicsGameObject();
        private ElympicsFloat deathTimer = new ElympicsFloat(0.0f);

        private StatsController currentStatisticsComponent = null;

        public virtual void ElympicsUpdate()
        {
            deathTimer.Value += Elympics.TickDuration;

            if (deathTimer.Value >= lifeTime)
            {
                DestroyProjectile();
            }
            else
            {
                DetectCollision();
            }
        }

        public void SetOwner(ElympicsBehaviour owner)
        {
            this.owner.Value = owner;
        }

        public void Launch(Vector3 direction)
        {
            ApplyBulletMovement(direction);
        }

        protected virtual void DealDamage()
        {
            damageApplicator.ApplyDamageTo(currentStatisticsComponent);
            deathTimer.Value = lifeTime;
        }

        private void ApplyBulletMovement(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
            }
            else
            {
                rigidbody.velocity = Vector3.zero;
            }
        }

        private void DetectCollision()
        {
            if (Physics.Raycast(transform.position, transform.forward, out var rayHit, COLLISION_DETECTION_DISTANCE, shellCollisionMask))
            {
                ApplyBulletMovement(Vector3.zero);

                if (rayHit.collider.transform.root.TryGetComponent(out currentStatisticsComponent))
                {
                    DealDamage();
                }
                else
                {
                    DestroyProjectile();
                }
            }
        }

        private void DestroyProjectile()
        {
            ElympicsDestroy(this.gameObject);
        }
    }
}
