using Enemies;
using EnumLibrary;

namespace StateMashine
{
    public class AlertState : BaseState
    {
        public AlertState(NPCBehaviourController behavCtrl, NPCStateMachine enemySM) : base(behavCtrl, enemySM)
        {
            StateName = "AlertState";
        }
        public AlertState(MeleeEnemyBehaviour meleeEnemyBehav, NPCStateMachine enemySM) : base(meleeEnemyBehav, enemySM)
        {
            StateName = "AlertState";
        }
        public AlertState(RangeEnemyBehaviour rangeEnemyBehav, NPCStateMachine enemySM) : base(rangeEnemyBehav, enemySM)
        {
            StateName = "AlertState";
        }

        public override void EnterState()
        {
            base.EnterState();

            _behaviourCtrl.BaseEnemyAlertStateSOInstance.ExecuteEnterLogic();
        }

        public override void ExitState()
        {
            base.ExitState();

            _behaviourCtrl.BaseEnemyAlertStateSOInstance.ExecuteExitLogic();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            _behaviourCtrl.BaseEnemyAlertStateSOInstance.ExecuteFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            _behaviourCtrl.BaseEnemyAlertStateSOInstance.ExecutePhysicsUpdateLogic();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);

            _behaviourCtrl.BaseEnemyAlertStateSOInstance.ExecuteAnimationTriggerEventLogic(animTriggerType);
        }
    }
}