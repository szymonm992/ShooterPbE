using UnityEngine;
using Elympics;
using System.Linq;

namespace ShooterPbE
{
    public class PlayersSpawner : ElympicsMonoBehaviour, IInitializable
    {
        [SerializeField] private PlayersManager playersProvider = null;
        [SerializeField] private Transform[] spawnPoints = null;

        private System.Random random = null;

        public void Initialize()
        {
            if (!Elympics.IsServer)
            {
                return;
            }

            random = new System.Random();

            if (playersProvider.IsReady)
            {
                InitialSpawnPlayers();
            }
            else
            {
                playersProvider.IsReadyChanged += InitialSpawnPlayers;
            }
        }

        private void InitialSpawnPlayers()
        {
            var preparedSpawnPoints = GetRandomizedSpawnPoints().Take(playersProvider.AllPlayersInScene.Length).ToArray();

            for (int i = 0; i < playersProvider.AllPlayersInScene.Length; i++)
            {
                playersProvider.AllPlayersInScene[i].transform.position = preparedSpawnPoints[i].position;
            }
        }

        private IOrderedEnumerable<Transform> GetRandomizedSpawnPoints()
        {
            return spawnPoints.OrderBy(x => random.Next());
        }
    }
}
