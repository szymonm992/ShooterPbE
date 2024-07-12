using UnityEngine;
using ShooterPbE.Weapons;

namespace ShooterPbE.Player
{
    public class PlayerShootingSystem : MonoBehaviour
    {
        [SerializeField] private WeaponBase currentEquipedWeapon;

        public void UpdateSystem(int playerId, bool isShooting)
        {
            if (isShooting)
            {
                currentEquipedWeapon.TryShoot(playerId);
            } 
        }
    }
}


