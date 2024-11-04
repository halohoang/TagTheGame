using Enemies;
using EnumLibrary;
using Player;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Melee-Attack State", menuName = "Scriptable Objects/Enemy Logic/Attack Logic/Melee Attack")]
    public class MeleeEnemyAttackSO : BaseEnemyAttackSO
    {
        #region Variables
        //--------------------------------------
        // - - - - -  V A R I A B L E S  - - - - 
        //--------------------------------------

        private PlayerStats _playerStatsComp;
        #endregion


        #region Methods
        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------

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

        public override void ExecuteOnEnterState()
        {
            base.ExecuteOnEnterState();

            // setup NavMeshAgent Properties
            //_enemyBehaviour.NavAgent.speed = _enemyBehaviour.ChasingSpeed;

            // set proper animation
            _behaviourCtrl.Animator.SetBool("Attack", true);

            // set PlayerGameObject reference
            _playerStatsComp = _behaviourCtrl.TargetObject.GetComponent<PlayerStats>();
        }

        public override void ExecuteOnExitState()
        {
            base.ExecuteOnExitState();

            // setup NavMeshAgent Properties
            //_enemyBehaviour.NavAgent.speed = _enemyBehaviour.MovementSpeed;

            // set proper animation
            _behaviourCtrl.Animator.SetBool("Attack", false);
        }

        public override void ExecuteFrameUpdate()
        {
            base.ExecuteFrameUpdate();

            if (_behaviourCtrl.IsInAttackRange)
            {
                // dealing Damage
                _playerStatsComp.GetDamage();
                Debug.Log($"<color=orange> AI-Melee-Behav: </color> '<color=FFD700>{_behaviourCtrl.gameObject.name}</color>' attacks its target Object " +
                    $"(<color=white>{_behaviourCtrl.TargetObject.name}</color>) and deals '<color=white>{_playerStatsComp.TakenDamage}</color>'");
            }
            else    // State transition back to chase ctate
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.ChaseState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>MeleeAttack</color>' to '<color=orange>Chase</color>' should have been happend now!");
                return;
            }
        }

        public override void ExecutePhysicsUpdate()
        {
            base.ExecutePhysicsUpdate();
        }

        public override void ExecuteOnAnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerTyoe)
        {
            base.ExecuteOnAnimationTriggerEvent(animTriggerTyoe);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }
        #endregion
    }
}