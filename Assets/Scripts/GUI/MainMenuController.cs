using Cysharp.Threading.Tasks;
using Elympics;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShooterPbE.GUI
{
    public class MainMenuController : MonoBehaviour
    {
        public static string MATCHMAKING_QUEUE = "Default";

        [SerializeField] private Button startGameButton;

        private void Start()
        {
            startGameButton.onClick.AddListener(StartButtonClicked);
            TogglePlayButton(ElympicsLobbyClient.Instance.IsAuthenticated);
            ElympicsLobbyClient.Instance.AuthenticationSucceeded += (_) => TogglePlayButton(true);
        }

        private void OnDestroy()
        {
            startGameButton.onClick.RemoveAllListeners();
        }

        private void StartButtonClicked()
        {
            TogglePlayButton(false);
            StartBattle();
        }

        private async void StartBattle()
        {
            var (closestRegion, latency) = await FindClosestRegion();
            ElympicsLobbyClient.Instance.PlayOnlineInRegion(closestRegion, null, null, MATCHMAKING_QUEUE);
            Debug.Log($"Connecting to region {closestRegion} with ping {latency}");
        }

        private void TogglePlayButton(bool value)
        {
            startGameButton.interactable = value;
        }

        private async UniTask<(string region, float latency)> FindClosestRegion()
        {
            Debug.Log("Searching for closest region...");
            var returnValue = await ElympicsCloudPing.ChooseClosestRegion(ElympicsRegions.AllAvailableRegions);
            Debug.Log("Closest region has been cached!");
            return returnValue;
        }
    }
}
