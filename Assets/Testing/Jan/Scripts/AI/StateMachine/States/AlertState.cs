using Enemies;
using EnumLibrary;
using UnityEngine;

namespace StateMashine
{
    public class AlertState : BaseState
    {
        public AlertState(BaseEnemyBehaviour enemy, EnemyStateMachine enemySM) : base(enemy, enemySM)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            _enemyBehaviour.BaseEnemyAlertStateSOInstance.ExecuteEnterLogic();
        }

        public override void ExitState()
        {
            base.ExitState();

            _enemyBehaviour.BaseEnemyAlertStateSOInstance.ExecuteExitLogic();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            _enemyBehaviour.BaseEnemyAlertStateSOInstance.ExecuteFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            _enemyBehaviour.BaseEnemyAlertStateSOInstance.ExecutePhysicsUpdateLogic();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);

            _enemyBehaviour.BaseEnemyAlertStateSOInstance.ExecuteAnimationTriggerEventLogic(animTriggerType);
        }
    }
}