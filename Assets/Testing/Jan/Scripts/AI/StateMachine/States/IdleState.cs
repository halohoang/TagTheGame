using Enemies;
using EnumLibrary;
using UnityEngine;

namespace StateMashine
{
    /// <summary>
    /// In the simpl IdleState currently nothing will happen, since the basic Idle is just standing around. For later versions of the game it might be worth thinking about
    /// some other Idle-Behaviour like randomly looking around via the 'Random.InsideUnitCircle()' or so; JM (27.10.2023)
    /// </summary>
    public class IdleState : BaseState
    {
        public IdleState(BaseEnemyBehaviour enemy, EnemyStateMachine enemySM) : base(enemy, enemySM)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);
        }
    }
}