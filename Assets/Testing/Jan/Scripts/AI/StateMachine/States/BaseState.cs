using UnityEngine;
using Enemies;
using EnumLibrary;

namespace StateMashine
{
    public class BaseState
    {
        protected BaseEnemyBehaviour _enemy;
        protected EnemyStateMachine _enemyStateMachine;

        public BaseState(BaseEnemyBehaviour enemy, EnemyStateMachine enemySM)
        {
            // setup Variables (using 'this'-Keyword just for visible clarification and simpler understanding)
            this._enemy = enemy;
            this._enemyStateMachine = enemySM;
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void FrameUpdate() { }
        public virtual void PhysicsUpdate() { }
        public virtual void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType) { }
    }
}