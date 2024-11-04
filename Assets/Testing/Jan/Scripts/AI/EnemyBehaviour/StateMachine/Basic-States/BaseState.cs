using UnityEngine;
using Enemies;
using EnumLibrary;

namespace StateMashine
{
    public class BaseState
    {
        protected NPCStateMachine _enemyStateMachine;
        protected NPCBehaviourController _behaviourCtrl;
        private string _stateName;

        public string StateName { get => _stateName; protected set => _stateName = value; }

        //protected MeleeEnemyBehaviour _meleeEnemyBehav;
        //protected RangeEnemyBehaviour _rangeEnemyBehav;

        public BaseState(NPCBehaviourController enemyBehav, NPCStateMachine enemyStaMa, string stateName)
        {
            // setup Variables (using 'this'-Keyword just for visible clarification and simpler understanding)
            this._behaviourCtrl = enemyBehav;
            this._enemyStateMachine = enemyStaMa;
            this._stateName = stateName;
        }

        //public BaseState(MeleeEnemyBehaviour meleeEnemyBehaviour, EnemyStateMachine enemyStaMa)
        //{
        //    // setup Variables (using 'this'-Keyword just for visible clarification and simpler understanding)
        //    this._meleeEnemyBehav = meleeEnemyBehaviour;
        //    this._enemyStateMachine = enemyStaMa;
        //}

        //public BaseState(RangeEnemyBehaviour rangeEnemyBehav, EnemyStateMachine enemyStaMa)
        //{
        //    // setup Variables (using 'this'-Keyword just for visible clarification and simpler understanding)
        //    this._rangeEnemyBehav = rangeEnemyBehav;
        //    this._enemyStateMachine = enemyStaMa;
        //}

        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void FrameUpdate() { }
        public virtual void PhysicsUpdate() { }
        public virtual void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType) { }
    }
}