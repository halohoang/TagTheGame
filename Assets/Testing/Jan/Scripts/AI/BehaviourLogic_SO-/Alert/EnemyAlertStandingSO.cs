using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Enemy-Alert-Stading", menuName = "Scriptable Objects/Enemy Logic/Alert Logic/Standing Alert")]
    public class EnemyAlertStandingSO : BaseEnemyAlertSO
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