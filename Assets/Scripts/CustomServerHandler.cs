using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ShooterPbE
{
    public class CustomServerHandler : ElympicsMonoBehaviour, IServerHandlerGuid, IUpdatable
    {
        public event Action GameReadyEvent;

        public bool IsGameReady => isGameReady.Value;

        [SerializeField] private float timeForPlayersToConnect = 30f;
        [SerializeField] private float connectingTimeoutCheckDelta = 5f;
        [SerializeField] private bool shouldGameEndAfterAnyDisconnect = false;

        private bool GameStateAlreadyDetermined => isGameReady.Value || gameCancelled;
        private bool NotAllPlayersConnected => connectedPlayers.Count != playersNumber;

        private readonly HashSet<ElympicsPlayer> connectedPlayers = new HashSet<ElympicsPlayer>();
        private readonly ElympicsBool isGameReady = new ElympicsBool(false);

        private int playersNumber;
        private bool gameCancelled = false;
        private bool isReadyLocally = false;

        public void OnServerInit(InitialMatchPlayerDatasGuid initialMatchPlayerDatas)
        {
            if (!this.IsEnabledAndActive)
            {
                return;
            }

            Assert.IsFalse(timeForPlayersToConnect < 0f || connectingTimeoutCheckDelta < 0f || connectingTimeoutCheckDelta > timeForPlayersToConnect);

            playersNumber = initialMatchPlayerDatas.Count;
            Debug.Log($"Game initialized; expecting: {playersNumber} players");

            StartCoroutine(WaitForClientsToConnect());
        }


        private IEnumerator WaitForClientsToConnect()
        {
            DateTime waitForPlayersFinishTime = DateTime.Now + TimeSpan.FromSeconds(timeForPlayersToConnect);

            while (DateTime.Now < waitForPlayersFinishTime && !GameStateAlreadyDetermined)
            {
                Debug.Log("Waiting for all players to connect...");
                yield return new WaitForSeconds(connectingTimeoutCheckDelta);
            }

            if (GameStateAlreadyDetermined)
            {
                yield break;
            }

            EndGameForcefully("Not all players have connected, therefore the game cannot start and so it ends");
        }

        public void OnPlayerDisconnected(ElympicsPlayer player)
        {
            if (!IsEnabledAndActive)
            {
                return;
            }

            Debug.Log($"Player {player} disconnected");
            connectedPlayers.Remove(player);

            if (gameCancelled)
            {
                return;
            }

            if (shouldGameEndAfterAnyDisconnect)
            {
                EndGameForcefully("Therefore, the game has ended");
            }
        }


        public void OnPlayerConnected(ElympicsPlayer player)
        {
            if (!IsEnabledAndActive)
            {
                return;
            }

            Debug.Log($"Player {player} connected");
            connectedPlayers.Add(player);

            if (NotAllPlayersConnected || GameStateAlreadyDetermined)
            {
                return;
            }

            BeginTheGame();
        }

        private void BeginTheGame()
        {
            isGameReady.Value = true;
        }

        private void EndGameForcefully(string message)
        {
            gameCancelled = true;
            Debug.Log($"Forcefull game end with message: {message}");
            Elympics.EndGame();
        }

        public void ElympicsUpdate()
        {
            if (isReadyLocally || !isGameReady.Value)
            {
                return;
            }

            GameReadyEvent?.Invoke();
            isReadyLocally = true;
        }
    }
}
