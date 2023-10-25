using UnityEngine;

namespace StateMashine
{
    public class Transition
    {
        private BaseCondition _condition;
        private BaseState _targetState;

        public BaseCondition Condition { get => _condition; set => _condition = value; }
        public BaseState TargetState { get => _targetState; set => _targetState = value; }
    }
}