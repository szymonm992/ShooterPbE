using Elympics;
using UnityEngine;
using ShooterPbE.Player;
using ShooterPbE;
using System;

namespace ShooterPbE.Inputs
{
    public class InputsController : ElympicsMonoBehaviour, IInputHandler, IUpdatable, IInitializable
    {
        [SerializeField] private InputsProvider inputProvider;
        [SerializeField] private StatsController playerStats;
        [SerializeField] private PlayerMovementController playerMovement;
        [SerializeField] private PlayerShootingSystem shootingSystem;
        [SerializeField] private CursorController cursorController;
        
        private GameStateController gameStateController;
        private bool canProcessInputs = false;

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
            if (canProcessInputs && !playerStats.IsDead && ElympicsBehaviour.TryGetInput(ElympicsPlayer.FromIndex(playerStats.PlayerId), out var inputReader))
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

        public void Initialize()
        {
            //NOTE: This kidn of weird/ugly constructions can be easily replaced with Dependency Injection, however
            //due to time limits I'm doing it this way
            gameStateController = FindObjectOfType<GameStateController>();
            gameStateController.CurrentGameState.ValueChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            gameStateController.CurrentGameState.ValueChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(int lastValue, int newValue)
        {
            canProcessInputs = (GameState)newValue == GameState.Inprogress;
        }
    }
}