using Enemies;
using EnumLibrary;
using UnityEngine;


namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MeleeEnemy_Chase", menuName = "Scriptable Objects/Enemy Logic/Chase Logic/MeleeEnemy Chase")]
    public class MeleeEnemyChaseSO : BaseEnemyChaseSO
    {
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

        public override void ExecuteEnterLogic()
        {
            base.ExecuteEnterLogic();

            // Set proper Animation
            _behaviourCtrl.Animator.SetBool("Engage", true);
        }

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();

            // Set proper Animation
            _behaviourCtrl.Animator.SetBool("Engage", false);
        }

        public override void ExecuteFrameUpdateLogic()
        {
            #region new Solution (03.07.24)

            // 1. Is target detected?
            if (_behaviourCtrl.IsTargetDetected)
            {
                // set facing direection via calling 'base.baseFrameUpdate()'
                base.ExecuteFrameUpdateLogic();

                // Set Movement-Destination for NavMeshAgent
                _behaviourCtrl.NavAgent.SetDestination(_behaviourCtrl.TargetObject.transform.position);

                // Attack-State-Transitioncheck
                AttackStateTransitionCheck();
            }
            else if (!_behaviourCtrl.IsTargetDetected)  // If Player is not detected -> go to last known player Position
            {
                _behaviourCtrl.NavAgent.SetDestination(_behaviourCtrl.LastKnowntargetPos);
                Debug.Log($"<color=orange> Melee-AI-Behav: </color> Target is not detectad anymore -> '<color=silver>{_behaviourCtrl.gameObject.name}</color>' is moving towards last known posiition of target ('<color=silver>{_behaviourCtrl.LastKnowntargetPos}</color>').");
            }

            // 2. If Player reached last known target postition and target is still not detected -> switch to Idle/alert state            
            if (!_behaviourCtrl.IsTargetDetected && (Vector2)_behaviourCtrl.gameObject.transform.position == (Vector2)_behaviourCtrl.LastKnowntargetPos)
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.IdleState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
            }

            #endregion

            #region Old Solution (before Jul. 2024)
            //// Transition-Condition-Check (if no Player is detected anymore -> switch to IdleState again)
            //if (!_behaviourCtrl.IsTargetDetected)
            //{
            //    _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.IdleState);
            //    Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
            //    return;
            //}
            //// 1.1) if agent reached last known position of player and player can't be detected anymore -> switch to idle/alert state            
            //else if (!_behaviourCtrl.IsTargetDetected && (Vector2)_behaviourCtrl.gameObject.transform.position == (Vector2)_behaviourCtrl.LastKnowntargetPos)
            //{
            //    _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.IdleState);
            //    Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
            //    return;
            //}
            //else if (!_behaviourCtrl.IsTargetDetected && (Vector2)_behaviourCtrl.gameObject.transform.position != (Vector2)_behaviourCtrl.LastKnowntargetPos)
            //{
            //    _behaviourCtrl.NavAgent.SetDestination(_behaviourCtrl.LastKnowntargetPos);
            //}

            //if (_behaviourCtrl.IsTargetDetected)
            //{
            //    // set facing direection via calling 'base.baseFrameUpdate()'
            //    base.ExecuteFrameUpdateLogic();

            //    // Set Movement-Destination for NavMeshAgent
            //    _behaviourCtrl.NavAgent.SetDestination(_behaviourCtrl.TargetObject.transform.position);
            //}

            //// Transition-Condition-Check (if Player is in AttackRange -> switch to Attack-State)          
            //if (_behaviourCtrl.IsInAttackRange)
            //{
            //    _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.AttackState);
            //    Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>MeleeAttack</color>' should have been happend now!");
            //    return;
            //}
            #endregion
        }

        /// <summary>
        /// Checks if this NPC is in attack range to its target object, if so a state transition to the respective attack state will be executed.
        /// </summary>
        private void AttackStateTransitionCheck()
        {
            // Transition-Condition-Check (if Player is in AttackRange -> switch to Attack-State)          
            if (_behaviourCtrl.IsInAttackRange)
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.AttackState);
                Debug.Log($"<color=orange> AI-Melee-Behav: </color> {_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>MeleeAttack</color>' should have been happend now!");
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
        #endregion
    }
}