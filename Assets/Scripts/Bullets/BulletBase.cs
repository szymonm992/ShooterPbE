using System.Collections;
using UnityEngine;
using Elympics;
//using ShooterPbE.Damage;

namespace ShooterPbE.Bullets
{
    public abstract class BulletBase : ElympicsMonoBehaviour, IUpdatable, IInitializable
    {
        public float LifeTime => lifeTime;

        [Header("Parameters:")]
        [SerializeField] protected float speed = 15.0f;
        [SerializeField] protected float lifeTime = 5.0f;

        [Header("References:")]
        [SerializeField] protected new Rigidbody rigidbody = null;
        //[SerializeField] protected new BulletDamageApplicator damageApplicator = null;

        protected ElympicsGameObject owner = new ElympicsGameObject();
        protected ElympicsBool readyToDealDamage = new ElympicsBool(false);
        protected ElympicsBool markedAsReadyToDestroy = new ElympicsBool(false);
        protected ElympicsBool bulletHit = new ElympicsBool(false);
        protected ElympicsFloat deathTimer = new ElympicsFloat(0.0f);

        private StatsController currentStatisticsComponent = null;

        public virtual void ElympicsUpdate()
        {
            if (readyToDealDamage.Value && !bulletHit)
            {
                DealDamage();
            }

            if (markedAsReadyToDestroy.Value)
            {
                ElympicsDestroy(this.gameObject);
            }

            deathTimer.Value += Elympics.TickDuration;

            if ((!bulletHit && deathTimer >= lifeTime))
            {
                DestroyProjectile();
            }
        }

        public void SetOwner(ElympicsBehaviour owner)
        {
            this.owner.Value = owner;
        }

        public void Initialize()
        {
            StartCoroutine(SelfDestoryTimer(lifeTime));
        }

        public void Launch(Vector3 direction)
        {
            ApplyBulletMovement(direction);
        }

        protected virtual void DealDamage()
        {
            //damageApplicator.Detonate(currentStatisticsComponent);

            bulletHit.Value = true;
            deathTimer.Value = 0.0f;
        }

        private void ApplyBulletMovement(Vector3 direction)
        {
            Debug.DrawRay(transform.position, direction, Color.red);
            rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ApplyBulletMovement(Vector3.zero);

            if (collision.gameObject.TryGetComponent(out currentStatisticsComponent))
            {
                MarkBulletAsReadyToDealDamage();
            }
            else
            {
                DestroyProjectile();
            }
        }

        private IEnumerator SelfDestoryTimer(float time)
        {
            yield return new WaitForSeconds(time);
            DestroyProjectile();
        }

        private void DestroyProjectile()
        {
            markedAsReadyToDestroy.Value = true;
        }

        private void MarkBulletAsReadyToDealDamage()
        {
            readyToDealDamage.Value = true;
        }
    }
}
