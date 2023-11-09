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

        public override void ExecuteEnterLogic()
        {
            base.ExecuteEnterLogic();
        }

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();
        }

        public override void ExecuteFrameUpdateLogic()
        {
            base.ExecuteFrameUpdateLogic();

            // Transition-Condition-Check (if Player is in AttackRange -> switch to Attack-State)          
            if (_baseEnemyBehaviour.IsInAttackRange)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.AttackState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>MeleeAttack</color>' should have been happend now!");
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