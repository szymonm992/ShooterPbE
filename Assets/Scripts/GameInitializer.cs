using Elympics;
using UnityEngine;
using System;

namespace ShooterPbE
{
    public class GameInitializer : ElympicsMonoBehaviour, IUpdatable
    {
        private event Action OnMatchInitializedAssignedCallback;

        public ElympicsFloat CurrentTimeToStartMatch { get; } = new (0.0f);

        [SerializeField] private float timeToStartMatch = 5.0f;

        private ElympicsBool gameInitializationEnabled = new (false);

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
