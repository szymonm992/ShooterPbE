using UnityEngine;
using ShooterPbE.Bullets;
using Elympics;

namespace ShooterPbE.Weapons
{
    public class MachineGun : WeaponBase
    {
        [SerializeField] private Transform bulletSpawnPoint = null;
        [SerializeField] private BulletBase prefab = null;

        protected override void PerformWeaponShoot(int playerId)
        {
            var bullet = CreateBullet(playerId);
            bullet.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<BulletBase>().Launch(bulletSpawnPoint.transform.forward);
        }

        protected GameObject CreateBullet(int playerId)
        {
            var bullet = ElympicsInstantiate(prefab.gameObject.name, ElympicsPlayer.FromIndex(playerId));
            bullet.GetComponent<BulletBase>().SetOwner(Owner.gameObject.transform.root.gameObject.GetComponent<ElympicsBehaviour>());

            return bullet;
        }
    }
}
