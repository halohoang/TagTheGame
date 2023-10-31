using Enemies;
using EnumLibrary;
using UnityEngine;

namespace StateMashine
{
    public class MeleeEnemyChaseState : ChaseState
    {
        private MeleeEnemyBehaviour _meleeEnemyBehaviour;

        public MeleeEnemyChaseState(MeleeEnemyBehaviour enemyBehav, EnemyStateMachine enemyStaMa) : base(enemyBehav, enemyStaMa)
        {
            this._meleeEnemyBehaviour = enemyBehav;
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

            // Check StateTransition-Conditions
            if (!_meleeEnemyBehaviour.IsPlayerDetected)
            {
                _meleeEnemyBehaviour.StateMachine.Transition(_meleeEnemyBehaviour.IdleState);

                Debug.Log($"State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
            }
            else if (_meleeEnemyBehaviour.IsInAttackRange)
            {
                _meleeEnemyBehaviour.StateMachine.Transition(_meleeEnemyBehaviour.MeleeAttackState);
                Debug.Log($"State-Transition from '<color=orange>Chase</color>' to '<color=orange>MeleeAttack</color>' should have been happend now!");
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