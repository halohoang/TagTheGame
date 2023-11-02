using UnityEngine;
using Enemies;
using EnumLibrary;

namespace StateMashine
{
    public class BaseState
    {
        protected BaseEnemyBehaviour _enemyBehaviour;
        protected EnemyStateMachine _enemyStateMachine;

        public BaseState(BaseEnemyBehaviour enemyBehav, EnemyStateMachine enemyStaMa)
        {
            // setup Variables (using 'this'-Keyword just for visible clarification and simpler understanding)
            this._enemyBehaviour = enemyBehav;
            this._enemyStateMachine = enemyStaMa;
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void FrameUpdate() { }
        public virtual void PhysicsUpdate() { }
        public virtual void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType) { }
    }
}