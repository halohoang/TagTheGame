using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RangeEnemy_Alert_Stading", menuName = "Scriptable Objects/Enemy Logic/Alert Logic/RangeEnemy Alert Standing")]
    public class RangeEnemyAlertStandingSO : BaseEnemyAlertSO
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

            // Transition-Check; If Player is detected change to Attack State (shooting)
            if (_behaviourCtrl.IsTargetDetected)
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.AttackState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Attack (Shooting)</color>' should have been happend now!");
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