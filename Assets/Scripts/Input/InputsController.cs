using Elympics;
using UnityEngine;
using ShooterPbE.Player;

namespace ShooterPbE.Inputs
{
    public class InputsController : ElympicsMonoBehaviour, IInputHandler, IUpdatable
    {
        [SerializeField] private InputsProvider inputProvider;
        [SerializeField] private PlayerStatistics playerStats;
        [SerializeField] private PlayerMovement playerMovement;

        public void OnInputForBot(IInputWriter inputSerializer)
        {
            SerializeInput(inputSerializer);
        }

        public void OnInputForClient(IInputWriter inputSerializer)
        {
            SerializeInput(inputSerializer);
        }

        public void ElympicsUpdate()
        {
            if (ElympicsBehaviour.TryGetInput(ElympicsPlayer.FromIndex(playerStats.PlayerId), out var inputReader))
            {
                inputReader.Read(out float forwardMovement);
                inputReader.Read(out float rightMovement);
                inputReader.Read(out bool isJumping);

                playerMovement.ProcessMovement(forwardMovement, rightMovement, isJumping);
            }
        }

        private void SerializeInput(IInputWriter inputWriter)
        {
            inputWriter.Write(inputProvider.Movement.x);
            inputWriter.Write(inputProvider.Movement.y);
            inputWriter.Write(inputProvider.Jump);
        }
    }
}