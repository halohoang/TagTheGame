using Enemies;
using EnumLibrary;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace StateMashine
{
    public class ChaseState : BaseState
    {
        public ChaseState(BaseEnemyBehaviour enemyBehav, EnemyStateMachine enemyStaMa) : base(enemyBehav, enemyStaMa)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            // caling the actual Behaviour of the State-ScriptableObjects
            _enemyBehaviour.BaseEnemyChaseStateSOInstance.ExecuteEnterLogic();
            #region OldCode
            //// setup NavMeshAgent Properties
            //_enemyBehaviour.NavAgent.speed = _enemyBehaviour.ChasingSpeed;

            //// Set proper Animation
            //_enemyBehaviour.Animator.SetBool("Engage", true);
            #endregion
        }

        public override void ExitState()
        {
            base.ExitState();

            // caling the actual Behaviour of the State-ScriptableObjects
            _enemyBehaviour.BaseEnemyChaseStateSOInstance.ExecuteExitLogic();

            #region OldCode
            //// setup NavMeshAgent Properties
            //_enemyBehaviour.NavAgent.speed = _enemyBehaviour.MovementSpeed;

            //// Set proper Animation
            //_enemyBehaviour.Animator.SetBool("Engage", false);
            #endregion
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // caling the actual Behaviour of the State-ScriptableObjects
            _enemyBehaviour.BaseEnemyChaseStateSOInstance.ExecuteFrameUpdateLogic();

            #region OldCode
            //// Set Movement-Destination for NavMeshAgent
            //_enemyBehaviour.NavAgent.SetDestination(_enemyBehaviour.PlayerObject.transform.position);

            //// facing Player Position
            //_enemyBehaviour.gameObject.transform.right = _enemyBehaviour.PlayerObject.transform.position - _enemyBehaviour.gameObject.transform.position;

            //// Check StateTransition-Conditions
            //if (!_enemyBehaviour.IsPlayerDetected)
            //{
            //    _enemyBehaviour.StateMachine.Transition(_enemyBehaviour.IdleState);

            //    Debug.Log($"State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
            //}
            //else if (_enemyBehaviour.IsInAttackRange)
            //{
            //    _enemyBehaviour.StateMachine.Transition(_enemyBehaviour.MeleeAttackState);
            //    Debug.Log($"State-Transition from '<color=orange>Chase</color>' to '<color=orange>MeleeAttack</color>' should have been happend now!");
            //}
            #endregion
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            // caling the actual Behaviour of the State-ScriptableObjects
            _enemyBehaviour.BaseEnemyChaseStateSOInstance.ExecutePhysicsUpdateLogic();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);

            // caling the actual Behaviour of the State-ScriptableObjects
            _enemyBehaviour.BaseEnemyChaseStateSOInstance.ExecuteAnimationTriggerEventLogic(animTriggerType);
        }
    }
}