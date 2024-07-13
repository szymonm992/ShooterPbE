using System;
using Elympics;
using System.Linq;

namespace ShooterPbE
{
    public class PlayersProvider : ElympicsMonoBehaviour, IInitializable
    {
        public StatsController ClientPlayer { get; private set; } = null;
        public StatsController[] AllPlayersInScene { get; private set; } = null;

        public bool IsReady { get; private set; } = false;
        public event Action IsReadyChanged = null;

        public void Initialize()
        {
            FindAllPlayersInScene();
            FindClientPlayerInScene();
            IsReady = true;
            IsReadyChanged?.Invoke();
        }

        private void FindAllPlayersInScene()
        {
            this.AllPlayersInScene = FindObjectsOfType<StatsController>().OrderBy(x => x.PlayerId).ToArray();
        }

        private void FindClientPlayerInScene()
        {
            foreach (var player in AllPlayersInScene)
            {
                if ((int)Elympics.Player == player.PlayerId)
                {
                    ClientPlayer = player;
                    return;
                }
            }

            //Fix for server side.
            ClientPlayer = AllPlayersInScene[0];
        }

        public StatsController GetPlayerById(int id)
        {
            return AllPlayersInScene.FirstOrDefault(x => x.PlayerId == id);
        }
    }
}
