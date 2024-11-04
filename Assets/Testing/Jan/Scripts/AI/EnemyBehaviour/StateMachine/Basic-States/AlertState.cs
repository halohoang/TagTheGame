﻿using Enemies;
using EnumLibrary;

namespace StateMashine
{
    public class AlertState : BaseState
    {
        public AlertState(NPCBehaviourController behavCtrl, NPCStateMachine enemySM) : base(behavCtrl, enemySM, "AlertState")
        {
           
        }
        public AlertState(MeleeEnemyBehavCtrl meleeEnemyBehav, NPCStateMachine enemySM) : base(meleeEnemyBehav, enemySM, "AlertState")
        {
            
        }
        public AlertState(RangeEnemyBehavCtrl rangeEnemyBehav, NPCStateMachine enemySM) : base(rangeEnemyBehav, enemySM, "AlertState")
        {
            
        }

        public override void EnterState()
        {
            base.EnterState();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyAlertStateSOInstance.ExecuteOnEnterState();
        }

        public override void ExitState()
        {
            base.ExitState();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyAlertStateSOInstance.ExecuteOnExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyAlertStateSOInstance.Execute﻿FrameUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyAlertStateSOInstance.ExecutePhysicsUpdate();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyAlertStateSOInstance.ExecuteOnAnim﻿ationTriggerEvent(animTriggerType);
        }
    }
}