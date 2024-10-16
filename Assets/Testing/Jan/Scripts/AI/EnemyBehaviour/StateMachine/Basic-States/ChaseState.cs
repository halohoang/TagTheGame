using Enemies;
using EnumLibrary;

namespace StateMashine
{
    public class ChaseState : BaseState
    {
        public ChaseState(NPCBehaviourController behavCtrl, NPCStateMachine enemyStaMa) : base(behavCtrl, enemyStaMa)
        {
            StateName = "ChaseState";
        }
        public ChaseState(MeleeEnemyBehavCtrl meleeEnemyBehav, NPCStateMachine enemySM) : base(meleeEnemyBehav, enemySM)
        {
            StateName = "ChaseState";
        }
        public ChaseState(RangeEnemyBehavCtrl rangeEnemyBehav, NPCStateMachine enemySM) : base(rangeEnemyBehav, enemySM)
        {
            StateName = "ChasetState";
        }

        public override void EnterState()
        {
            base.EnterState();

            // caling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyChaseStateSOInstance.ExecuteEnterLogic();
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
            _behaviourCtrl.BaseEnemyChaseStateSOInstance.ExecuteExitLogic();

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
            _behaviourCtrl.BaseEnemyChaseStateSOInstance.ExecuteFrameUpdateLogic();

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
            _behaviourCtrl.BaseEnemyChaseStateSOInstance.ExecutePhysicsUpdateLogic();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);

            // caling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyChaseStateSOInstance.ExecuteAnimationTriggerEventLogic(animTriggerType);
        }
    }
}