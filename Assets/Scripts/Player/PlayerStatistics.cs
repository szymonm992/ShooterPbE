using UnityEngine;

namespace ShooterPbE.Player
{
    public class PlayerStatistics : MonoBehaviour
    {
        [Header("Parameters:")]
        [SerializeField] private int playerId = 0;

        public int PlayerId => playerId;
    }
}
