using Enemies;
using EnumLibrary;

namespace StateMashine
{
    public class ChaseState : BaseState
    {
        public ChaseState(NPCBehaviourController behavCtrl, NPCStateMachine enemyStaMa) : base(behavCtrl, enemyStaMa, "ChaseState")
        {
            
        }
        public ChaseState(MeleeEnemyBehavCtrl meleeEnemyBehav, NPCStateMachine enemySM) : base(meleeEnemyBehav, enemySM, "ChaseState")
        {
            
        }
        public ChaseState(RangeEnemyBehavCtrl rangeEnemyBehav, NPCStateMachine enemySM) : base(rangeEnemyBehav, enemySM, "ChaseState")
        {
            
        }

        public override void EnterState()
        {
            base.EnterState();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyChaseStateSOInstance.ExecuteOnEnterState();
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

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyChaseStateSOInstance.ExecuteOnExitState();

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

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyChaseStateSOInstance.ExecuteFrameUpdate();

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

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyChaseStateSOInstance.ExecutePhysicsUpdate();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyChaseStateSOInstance.ExecuteOnAnimationTriggerEvent(animTriggerType);
        }
    }
}