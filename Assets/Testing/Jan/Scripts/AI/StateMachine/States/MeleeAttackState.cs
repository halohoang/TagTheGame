using Enemies;
using EnumLibrary;
using UnityEngine;

namespace StateMashine
{
    public class MeleeAttackState : BaseState
    {
        private PlayerHealth _playerHealthScript;
        private MeleeEnemyBehaviour _meleeEnemyBehaviour;


        public MeleeAttackState(MeleeEnemyBehaviour enemyBehav, EnemyStateMachine enemyStaMa) : base(enemyBehav, enemyStaMa)
        {
            this._meleeEnemyBehaviour = enemyBehav;
        }

        public override void EnterState()
        {
            base.EnterState();

            // setup NavMeshAgent Properties
            //_enemyBehaviour.NavAgent.speed = _enemyBehaviour.ChasingSpeed;

            // set proper animation
            _meleeEnemyBehaviour.Animator.SetBool("Attack", true);

            // set PlayerGameObject reference
            _playerHealthScript = _meleeEnemyBehaviour.PlayerObject.GetComponent<PlayerHealth>();            
        }

        public override void ExitState()
        {
            base.ExitState();

            // setup NavMeshAgent Properties
            //_enemyBehaviour.NavAgent.speed = _enemyBehaviour.MovementSpeed;

            // set proper animation
            _meleeEnemyBehaviour.Animator.SetBool("Attack", false);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (_meleeEnemyBehaviour.IsInAttackRange)
            {
                // dealing Damage
                _playerHealthScript.GetDamage();
            }
            else
            {
                _meleeEnemyBehaviour.StateMachine.Transition(_meleeEnemyBehaviour.ChaseState);
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