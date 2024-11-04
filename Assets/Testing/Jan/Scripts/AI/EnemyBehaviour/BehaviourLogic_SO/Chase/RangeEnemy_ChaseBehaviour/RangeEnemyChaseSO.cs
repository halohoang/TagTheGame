using Enemies;
using EnumLibrary;
using UnityEngine;


namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RangeEnemy_Chase", menuName = "Scriptable Objects/Enemy Logic/Chase Logic/RangeEnemy Chase")]
    public class RangeEnemyChaseSO : BaseEnemyChaseSO
    {
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

            // Set proper Animation
            _behaviourCtrl.Animator.SetBool("Patrol", true);
        }

        public override void ExecuteOnExitState()
        {
            base.ExecuteOnExitState();

            // Set proper Animation
            _behaviourCtrl.Animator.SetBool("Patrol", false);
        }

        public override void ExecuteFrameUpdate()
        {
            // set facing direection via calling 'base.baseFrameUpdate()'
            base.ExecuteFrameUpdate();                       

            // Set Movement-Destination for NavMeshAgent
            _behaviourCtrl.NavAgent.SetDestination(_behaviourCtrl.LastKnowntargetPos);
            #region debuggers little helper
            Debug.Log($"<color=orange>{_behaviourCtrl.gameObject.name}</color>: Last Known Player Position is: ('<color=lime>{_behaviourCtrl.LastKnowntargetPos}</color>') " +
                $"| Own Position is: ('<color=lime>{_behaviourCtrl.gameObject.transform.position}</color>')");
            #endregion

            // 1) Transition check (if player is detected -> switch to attack state (shooting))
            if (_behaviourCtrl.IsTargetDetected)
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.AttackState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>Attack (Shooting)</color>' should have been happend now!");
                return;
            }
            // 1.1) if agent reached last known position of player and player can't be detected anymore -> switch to idle/alert state            
            else if ((Vector2)_behaviourCtrl.gameObject.transform.position == (Vector2)_behaviourCtrl.LastKnowntargetPos && !_behaviourCtrl.IsTargetDetected)
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.IdleState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
                return;
            }
            else if ((Vector2)_behaviourCtrl.gameObject.transform.position != (Vector2)_behaviourCtrl.LastKnowntargetPos && !_behaviourCtrl.IsTargetDetected)
            {
                _behaviourCtrl.NavAgent.SetDestination(_behaviourCtrl.LastKnowntargetPos);
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
    }
}