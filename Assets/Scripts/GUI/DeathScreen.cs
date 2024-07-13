using TMPro;
using UnityEngine;

namespace ShooterPbE.GUI
{
    public class DeathScreen : MonoBehaviour
    {
        [SerializeField] private PlayersProvider playersProvider = null;
        [SerializeField] private CanvasGroup canvasGroup = null;
        [SerializeField] private TextMeshProUGUI deathTimerText = null;

        private void Start()
        {
            if (playersProvider.IsReady)
            {
                SubscribeToDeathController();
            }
            else
            {
                playersProvider.IsReadyChanged += SubscribeToDeathController;
            }
        }

        private void SubscribeToDeathController()
        {
            var clientPlayerDeathController = playersProvider.ClientPlayer.GetComponent<DeathController>();
            clientPlayerDeathController.CurrentDeathTime.ValueChanged += UpdateDeathTimerView;
            clientPlayerDeathController.IsDead.ValueChanged += UpdateDeathScreenView;
        }

        private void UpdateDeathScreenView(bool lastValue, bool newValue)
        {
            canvasGroup.alpha = newValue ? 1.0f : 0.0f;
        }

        private void UpdateDeathTimerView(float lastValue, float newValue)
        {
            deathTimerText.text = Mathf.Ceil(newValue).ToString();
        }
    }
}
