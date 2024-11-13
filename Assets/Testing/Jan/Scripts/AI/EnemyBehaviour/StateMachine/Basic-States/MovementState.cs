using Enemies;
using EnumLibrary;

namespace StateMashine
{
    public class MovementState : BaseState
    {
        public MovementState(NPCBehaviourController behavCtrl, NPCStateMachine enemySM) : base(behavCtrl, enemySM, "MovementState")
        {
            
        }
        public MovementState(MeleeEnemyBehavCtrl meleeEnemyBehav, NPCStateMachine enemySM) : base(meleeEnemyBehav, enemySM, "MovementState")
        {
            
        }
        public MovementState(RangeEnemyBehavCtrl rangeEnemyBehav, NPCStateMachine enemySM) : base(rangeEnemyBehav, enemySM, "MovementState")
        {
           
        }

        public override void EnterState()
        {
            base.EnterState();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyMovementStateSOInstance.ExecuteOnE﻿nterState();
        }

        public override void ExitState()
        {
            base.ExitState();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyMovementStateSOInstance.ExecuteOnExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyMovementStateSOInstance.ExecuteFrameUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyMovementStateSOInstance.ExecutePhysicsUpdate();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyMovementStateSOInstance.ExecuteOnAnim﻿﻿ationTriggerEvent(animTriggerType);
        }
    }
}