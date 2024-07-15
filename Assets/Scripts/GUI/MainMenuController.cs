using Elympics;
using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace ShooterPbE.GUI
{
    public class MainMenuController : MonoBehaviour
    {
        public static string MATCHMAKING_QUEUE = "1v1";

        [SerializeField] private Button startGameButton;
        [SerializeField] private CanvasGroup loadingScreenGroup;
        [SerializeField] private TextMeshProUGUI errorLabel;

        [UsedImplicitly]
        public async void PlayOnline()
        {
            var (region, latency) = await ClosestRegionFinder.GetClosestRegion();
            ElympicsLobbyClient.Instance.PlayOnlineInRegion(region, null, null, MATCHMAKING_QUEUE);

            Debug.Log($"Joined matchmaking in region: {region} with ping {latency}");
        }

        private void Awake()
        {
            DebugManager.instance.enableRuntimeUI = false;
        }

        private void Start()
        {
            if (ElympicsLobbyClient.Instance == null)
            {
                return;
            }

            startGameButton.onClick.AddListener(StartButtonClicked);
            TogglePlayButton(ElympicsLobbyClient.Instance.IsAuthenticated);
            ElympicsLobbyClient.Instance.AuthenticationSucceeded += (_) => TogglePlayButton(true);
            ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed += OnMatchmakingFailed;
        }

        private void OnDestroy()
        {
            if (ElympicsLobbyClient.Instance == null)
            {
                return;
            }

            ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed -= OnMatchmakingFailed;
            startGameButton.onClick.RemoveAllListeners();
        }

        private void OnMatchmakingFailed((string error, Guid _) result) => SetErrorTextCaption(result.error);

        private void StartButtonClicked()
        {
            SetErrorTextCaption(string.Empty);
            TogglePlayButton(false);
            ToggleLoadingScreen(true);
            PlayOnline();
        }

        private void SetErrorTextCaption(string caption)
        {
            errorLabel.text = caption;
        }

        private void TogglePlayButton(bool value)
        {
            startGameButton.interactable = value;
        }

        private void ToggleLoadingScreen(bool value)
        {
            loadingScreenGroup.alpha = value ? 1.0f : 0.0f;
        }
    }
}
