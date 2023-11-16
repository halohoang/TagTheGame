using Enemies;
using EnumLibrary;
using UnityEngine;


namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MeleeEnemy_Chase", menuName = "Scriptable Objects/Enemy Logic/Chase Logic/MeleeEnemy Chase")]
    public class MeleeEnemyChaseSO : BaseEnemyChaseSO
    {
        public override void Initialize(GameObject enemyObj, BaseEnemyBehaviour enemyBehav)
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
            _baseEnemyBehaviour.Animator.SetBool("Engage", true);
        }

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();

            // Set proper Animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", false);
        }

        public override void ExecuteFrameUpdateLogic()
        {
            // Transition-Condition-Check (if not Player is detected anymore -> switch to IdleState again)
            if (!_baseEnemyBehaviour.IsPlayerDetected)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.IdleState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
                return;
            }
            // 1.1) if agent reached last known position of player and player can't be detected anymore -> switch to idle/alert state            
            else if ((Vector2)_baseEnemyBehaviour.gameObject.transform.position == (Vector2)_baseEnemyBehaviour.LastKnownPlayerPos && !_baseEnemyBehaviour.IsPlayerDetected)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.IdleState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
                return;
            }
            else if ((Vector2)_baseEnemyBehaviour.gameObject.transform.position != (Vector2)_baseEnemyBehaviour.LastKnownPlayerPos && !_baseEnemyBehaviour.IsPlayerDetected)
            {
                _baseEnemyBehaviour.NavAgent.SetDestination(_baseEnemyBehaviour.LastKnownPlayerPos);
            }

            // set facing direection via calling 'base.baseFrameUpdate()'
            base.ExecuteFrameUpdateLogic();
            
            // Set Movement-Destination for NavMeshAgent
            _baseEnemyBehaviour.NavAgent.SetDestination(_baseEnemyBehaviour.PlayerObject.transform.position);

            // Transition-Condition-Check (if Player is in AttackRange -> switch to Attack-State)          
            if (_baseEnemyBehaviour.IsInAttackRange)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.AttackState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>MeleeAttack</color>' should have been happend now!");
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