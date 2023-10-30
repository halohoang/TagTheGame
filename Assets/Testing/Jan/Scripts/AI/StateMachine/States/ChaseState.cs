using Enemies;
using EnumLibrary;
using UnityEngine;

namespace StateMashine
{
    public class ChaseState : BaseState
    {
        public ChaseState(BaseEnemyBehaviour enemy, EnemyStateMachine enemySM) : base(enemy, enemySM)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (!_enemyBehaviour.IsPlayerDetected)
            {
                _enemyBehaviour.StateMachine.Transition(_enemyBehaviour.IdleState);
                Debug.Log($"State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);
        }
    }
}