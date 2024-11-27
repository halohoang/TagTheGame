using Enemies;
using EnumLibrary;

namespace StateMashine
{
    /// <summary>
    /// In the simpl IdleState currently nothing will happen, since the basic Idle is just standing around. For later versions of the game it might be worth thinking about
    /// some other Idle-Behaviour like randomly looking around via the 'Random.InsideUnitCircle()' or so; JM (27.10.2023)
    /// </summary>
    public class InvestigationState : BaseState
    {
        public InvestigationState(NPCBehaviourController behavCtrl, NPCStateMachine enemySM) : base(behavCtrl, enemySM, "InvestigationState")
        {

        }
        public InvestigationState(MeleeEnemyBehavCtrl meleeEnemyBehav, NPCStateMachine enemySM) : base(meleeEnemyBehav, enemySM, "InvestigationState")
        {

        }
        public InvestigationState(RangeEnemyBehavCtrl rangeEnemyBehav, NPCStateMachine enemySM) : base(rangeEnemyBehav, enemySM, "InvestigationState")
        {

        }

        public override void EnterState()
        {
            base.EnterState();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyInvestigationStateSOInstance.ExecuteOnE﻿nterState();
        }

        public override void ExitState()
        {
            base.ExitState();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyInvestigationStateSOInstance.ExecuteOnExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyInvestigationStateSOInstance.ExecuteFrameUpdate();

            #region OldCode
            //// Switch State from Idle to ChaseState when Player is Detected
            //if (_enemyBehaviour.IsPlayerDetected)
            //{
            //    _enemyBehaviour.StateMachine.Transition(_enemyBehaviour.ChaseState);
            //    Debug.Log($"State-Transition from '<color=orange>Idle</color>' to '<color=orange>Chase</color>' should have been happend now!");
            //}
            #endregion
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyInvestigationStateSOInstance.ExecutePhysicsUpdate();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);

            // calling the actual Behaviour of the State-ScriptableObjects
            _behaviourCtrl.BaseEnemyInvestigationStateSOInstance.ExecuteOnAnim﻿﻿ationTriggerEvent(animTriggerType);
        }
    }
}