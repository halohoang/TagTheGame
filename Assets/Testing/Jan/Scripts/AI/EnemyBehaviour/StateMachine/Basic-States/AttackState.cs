using Enemies;
using EnumLibrary;
using Player;
using UnityEngine;

namespace StateMashine
{
    public class AttackState : BaseState
    {
        private PlayerStats _playerStatsScript;

        public AttackState(NPCBehaviourController behavCtrl, NPCStateMachine enemyStaMa) : base(behavCtrl, enemyStaMa)
        {
            StateName = "AttackState";
        }
        public AttackState(MeleeEnemyBehaviour meleeEnemyBehav, NPCStateMachine enemySM) : base(meleeEnemyBehav, enemySM)
        {
            StateName = "AttackState";
        }
        public AttackState(RangeEnemyBehaviour rangeEnemyBehav, NPCStateMachine enemySM) : base(rangeEnemyBehav, enemySM)
        {
            StateName = "AttackState";
        }

        public override void EnterState()
        {
            base.EnterState();

            // caling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyAttackStateSOInstance.ExecuteEnterLogic();

            #region OldCode
            //// setup NavMeshAgent Properties
            ////_enemyBehaviour.NavAgent.speed = _enemyBehaviour.ChasingSpeed;

            //// set proper animation
            //_enemyBehaviour.Animator.SetBool("Attack", true);

            //// set PlayerGameObject reference
            //_playerHealthScript = _enemyBehaviour.PlayerObject.GetComponent<PlayerHealth>();
            #endregion
        }

        public override void ExitState()
        {
            base.ExitState();

            // caling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyAttackStateSOInstance.ExecuteExitLogic();
            #region OldCode
            //// setup NavMeshAgent Properties
            ////_enemyBehaviour.NavAgent.speed = _enemyBehaviour.MovementSpeed;

            //// set proper animation
            //_enemyBehaviour.Animator.SetBool("Attack", false);
            #endregion
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // caling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyAttackStateSOInstance.ExecuteFrameUpdateLogic();

            #region OldCode
            //if (_enemyBehaviour.IsInAttackRange)
            //{
            //    // dealing Damage
            //    _playerHealthScript.GetDamage();
            //}
            //else
            //{
            //    _enemyBehaviour.StateMachine.Transition(_enemyBehaviour.ChaseState);
            //    Debug.Log($"State-Transition from '<color=orange>MeleeAttack</color>' to '<color=orange>Chase</color>' should have been happend now!");
            //}
            #endregion
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            // caling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyAttackStateSOInstance.ExecutePhysicsUpdateLogic();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);

            // caling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyAttackStateSOInstance.ExecuteAnimationTriggerEventLogic(animTriggerType);
        }
    }
}