using UnityEngine;

namespace StateMashine
{
    public class EnemyStateMachine
    {
        private BaseState _currentState;

        public BaseState CurrentState { get => _currentState; set => _currentState = value; }

        /// <summary>
        /// Setting an initial State of the StateMachine to start with
        /// </summary>
        /// <param name="initialState"></param>
        public void Initialize(BaseState initialState)
        {
            CurrentState = initialState;
            CurrentState.EnterState();
        }

        /// <summary>
        /// Switching from one state to another state
        /// </summary>
        /// <param name="nextState"></param>
        public void Transition(BaseState nextState)
        {
            CurrentState.ExitState();
            CurrentState = nextState;
            CurrentState.EnterState();
        }
    }
}