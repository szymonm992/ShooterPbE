using Elympics;
using UnityEngine;
using System;

namespace ShooterPbE
{
    public class GameInitializer : ElympicsMonoBehaviour, IUpdatable
    {
        private Action OnMatchInitializedAssignedCallback;

        public ElympicsFloat CurrentTimeToStartMatch { get; } = new ElympicsFloat(0.0f);

        [SerializeField] private float timeToStartMatch = 5.0f;

        private ElympicsBool gameInitializationEnabled = new ElympicsBool(false);

        public void InitializeMatch(Action OnMatchInitializedCallback)
        {
            OnMatchInitializedAssignedCallback = OnMatchInitializedCallback;

            CurrentTimeToStartMatch.Value = timeToStartMatch;
            gameInitializationEnabled.Value = true;
        }

        public void ElympicsUpdate()
        {
            if (gameInitializationEnabled.Value)
            {
                CurrentTimeToStartMatch.Value -= Elympics.TickDuration;

                if (CurrentTimeToStartMatch.Value <= 0.0f)
                {
                    OnMatchInitializedAssignedCallback?.Invoke();

                    gameInitializationEnabled.Value = false;
                }
            }
        }
    }
}
