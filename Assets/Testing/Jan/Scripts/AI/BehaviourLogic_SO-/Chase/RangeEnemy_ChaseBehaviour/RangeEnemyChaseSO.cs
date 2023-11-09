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

        public override void ExecuteEnterLogic()
        {
            base.ExecuteEnterLogic();
            
            // setup chasing relevat values (e.g. speed)

            // play 'engage' animation
        }

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();

            // stop 'engage' animation
        }

        public override void ExecuteFrameUpdateLogic()
        {
            base.ExecuteFrameUpdateLogic();

            // 1) transition check (if player is detected -> switch to attack state (shooting))
            // 1.1) if agent reached last known position of player and player can't be detected anymore -> switch to idle/alert state            
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