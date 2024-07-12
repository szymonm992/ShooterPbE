using Elympics;
using UnityEngine;
using ShooterPbE.Player;

namespace ShooterPbE.Inputs
{
    public class InputsController : ElympicsMonoBehaviour, IInputHandler, IUpdatable
    {
        [SerializeField] private InputsProvider inputProvider;
        [SerializeField] private PlayerStatistics playerStats;
        [SerializeField] private PlayerMovementController playerMovement;
        [SerializeField] private PlayerShootingSystem shootingSystem;
        [SerializeField] private CursorController cursorController;

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

                inputReader.Read(out float cursorPositionX);
                inputReader.Read(out float cursorPositionY);
                inputReader.Read(out float cursorPositionZ);

                inputReader.Read(out bool shoot);
                inputReader.Read(out bool isJumping);

                playerMovement.ProcessMovement(forwardMovement, rightMovement, isJumping, new Vector3(cursorPositionX, cursorPositionY, cursorPositionZ));
                shootingSystem.UpdateSystem(playerStats.PlayerId, shoot);
            }
        }

        private void SerializeInput(IInputWriter inputWriter)
        {
            inputWriter.Write(inputProvider.Movement.x);
            inputWriter.Write(inputProvider.Movement.y);

            inputWriter.Write(inputProvider.WorldCursorPosition.x);
            inputWriter.Write(inputProvider.WorldCursorPosition.y);
            inputWriter.Write(inputProvider.WorldCursorPosition.z);

            inputWriter.Write(inputProvider.Shoot);
            inputWriter.Write(inputProvider.Jump);
        }
    }
}