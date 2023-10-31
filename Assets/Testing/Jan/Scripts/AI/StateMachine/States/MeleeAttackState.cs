using Enemies;
using EnumLibrary;
using UnityEngine;

namespace StateMashine
{
    public class MeleeAttackState : BaseState
    {
        private PlayerHealth _playerHealthScript;

        public MeleeAttackState(BaseEnemyBehaviour enemy, EnemyStateMachine enemySM) : base(enemy, enemySM)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            // setup NavMeshAgent Properties
            //_enemyBehaviour.NavAgent.speed = _enemyBehaviour.ChasingSpeed;

            // set proper animation
            _enemyBehaviour.Animator.SetBool("Attack", true);

            // set PlayerGameObject reference
            _playerHealthScript = _enemyBehaviour.PlayerObject.GetComponent<PlayerHealth>();            
        }

        public override void ExitState()
        {
            base.ExitState();

            // setup NavMeshAgent Properties
            //_enemyBehaviour.NavAgent.speed = _enemyBehaviour.MovementSpeed;
            
            // set proper animation
            _enemyBehaviour.Animator.SetBool("Attack", false);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (_enemyBehaviour.IsInAttackRange)
            {
                // dealing Damage
                _playerHealthScript.GetDamage();
            }
            else
            {
                _enemyBehaviour.StateMachine.Transition(_enemyBehaviour.ChaseState);
                Debug.Log($"State-Transition from '<color=orange>MeleeAttack</color>' to '<color=orange>Chase</color>' should have been happend now!");

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