using Elympics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace ShooterPbE.GUI
{
    public class MainMenuController : MonoBehaviour
    {
        public static string MATCHMAKING_QUEUE = "1v1";

        [SerializeField] private Button startGameButton;
        [SerializeField] private CanvasGroup loadingScreenGroup;

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
            ToggleLoadingScreen(true);
            PlayOnline();
        }

        [UsedImplicitly]
        public async void PlayOnline()
        {
            var (region, latency) = await ClosestRegionFinder.GetClosestRegion();
            ElympicsLobbyClient.Instance.PlayOnlineInRegion(region, null, null, MATCHMAKING_QUEUE);

            Debug.Log($"Joined matchmaking in region: {region} with ping {latency}");
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
