using Enemies;
using EnumLibrary;
using Player;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Melee-Attack State", menuName = "Scriptable Objects/Enemy Logic/Attack Logic/Melee Attack")]
    public class MeleeEnemyAttackSO : BaseEnemyAttackSO
    {
        private PlayerStats _playerStatsScript;

        public override void Initialize(GameObject enemyObj, NPCBehaviourController enemyBehav)
        {
            base.Initialize(enemyObj, enemyBehav);
        }
        //public override void Initialize(GameObject enemyObj, MeleeEnemyBehaviour meleeEnemyBehav)
        //{
        //    base.Initialize(enemyObj, meleeEnemyBehav);
        //}
        //public override void Initialize(GameObject enemyObj, RangeEnemyBehaviour rangeEnemyBehav)
        //{
        //    base.Initialize(enemyObj, rangeEnemyBehav);
        //}

        public override void ExecuteEnterLogic()
        {
            base.ExecuteEnterLogic();

            // setup NavMeshAgent Properties
            //_enemyBehaviour.NavAgent.speed = _enemyBehaviour.ChasingSpeed;

            // set proper animation
            _behaviourCtrl.Animator.SetBool("Attack", true);

            // set PlayerGameObject reference
            _playerStatsScript = _behaviourCtrl.PlayerObject.GetComponent<PlayerStats>();
        }

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();

            // setup NavMeshAgent Properties
            //_enemyBehaviour.NavAgent.speed = _enemyBehaviour.MovementSpeed;

            // set proper animation
            _behaviourCtrl.Animator.SetBool("Attack", false);
        }

        public override void ExecuteFrameUpdateLogic()
        {
            base.ExecuteFrameUpdateLogic();

            if (_behaviourCtrl.IsInAttackRange)
            {
                // dealing Damage
                _playerStatsScript.GetDamage();
            }
            else
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.ChaseState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>MeleeAttack</color>' to '<color=orange>Chase</color>' should have been happend now!");
                return;
            }
        }

        public override void ExecutePhysicsUpdateLogic()
        {
            base.ExecutePhysicsUpdateLogic();
        }

        public override void ExecuteAnimationTriggerEventLogic(Enum_Lib.EAnimationTriggerType animTriggerTyoe)
        {
            base.ExecuteAnimationTriggerEventLogic(animTriggerTyoe);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }
    }
}