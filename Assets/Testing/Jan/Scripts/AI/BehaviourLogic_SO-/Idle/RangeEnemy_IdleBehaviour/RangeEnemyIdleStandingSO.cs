using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RangeEnemy_Idle_Standing", menuName = "Scriptable Objects/Enemy Logic/Idle Logic/RangeEnemy Standing (Just Standing Idle)")]
    public class RangeEnemyIdleStandingSO : BaseEnemyIdleSO
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

            // Transitionchecks 
            // Switch State from Idle to AttackState (Shooting) when Player is Detected
            if (_baseEnemyBehaviour.IsPlayerDetected)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.AttackState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Attack (Shooting/RangeAttack)</color>' should have been happend now!");
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