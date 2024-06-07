using Enemies;
using EnumLibrary;

namespace StateMashine
{
    /// <summary>
    /// In the simpl IdleState currently nothing will happen, since the basic Idle is just standing around. For later versions of the game it might be worth thinking about
    /// some other Idle-Behaviour like randomly looking around via the 'Random.InsideUnitCircle()' or so; JM (27.10.2023)
    /// </summary>
    public class IdleState : BaseState
    {
        public IdleState(BaseEnemyBehaviour enemyBehav, EnemyStateMachine enemySM) : base(enemyBehav, enemySM)
        {
        }
        public IdleState(MeleeEnemyBehaviour meleeEnemyBehav, EnemyStateMachine enemySM) : base (meleeEnemyBehav, enemySM) 
        { 
        }
        public IdleState(RangeEnemyBehaviour rangeEnemyBehav, EnemyStateMachine enemySM) : base(rangeEnemyBehav, enemySM)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            // caling the actual Behaviour of the State-ScriptableObjects
            _enemyBehaviour.BaseEnemyIdleStateSOInstance.ExecuteEnterLogic();
        }

        public override void ExitState()
        {
            base.ExitState();

            // caling the actual Behaviour of the State-ScriptableObjects
            _enemyBehaviour.BaseEnemyIdleStateSOInstance.ExecuteExitLogic();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // caling the actual Behaviour of the State-ScriptableObjects
            _enemyBehaviour.BaseEnemyIdleStateSOInstance.ExecuteFrameUpdateLogic();

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

            // caling the actual Behaviour of the State-ScriptableObjects
            _enemyBehaviour.BaseEnemyIdleStateSOInstance.ExecutePhysicsUpdateLogic();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);

            // caling the actual Behaviour of the State-ScriptableObjects
            _enemyBehaviour.BaseEnemyIdleStateSOInstance.ExecuteAnimationTriggerEventLogic(animTriggerType);
        }
    }
}