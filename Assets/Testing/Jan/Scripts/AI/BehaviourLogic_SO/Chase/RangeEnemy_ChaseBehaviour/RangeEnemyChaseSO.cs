using Enemies;
using EnumLibrary;
using UnityEngine;


namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RangeEnemy_Chase", menuName = "Scriptable Objects/Enemy Logic/Chase Logic/RangeEnemy Chase")]
    public class RangeEnemyChaseSO : BaseEnemyChaseSO
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
            _baseEnemyBehaviour.Animator.SetBool("Patrol", true);
        }

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();

            // Set proper Animation
            _baseEnemyBehaviour.Animator.SetBool("Patrol", false);
        }

        public override void ExecuteFrameUpdateLogic()
        {
            // set facing direection via calling 'base.baseFrameUpdate()'
            base.ExecuteFrameUpdateLogic();                       

            // Set Movement-Destination for NavMeshAgent
            _baseEnemyBehaviour.NavAgent.SetDestination(_baseEnemyBehaviour.LastKnownPlayerPos);

            // 1) Transition check (if player is detected -> switch to attack state (shooting))
            if (_baseEnemyBehaviour.IsPlayerDetected)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.AttackState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>Attack (Shooting)</color>' should have been happend now!");
            }
            // 1.1) if agent reached last known position of player and player can't be detected anymore -> switch to idle/alert state            
            else if (_baseEnemyBehaviour.gameObject.transform.position == _baseEnemyBehaviour.LastKnownPlayerPos && !_baseEnemyBehaviour.IsPlayerDead)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.IdleState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
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