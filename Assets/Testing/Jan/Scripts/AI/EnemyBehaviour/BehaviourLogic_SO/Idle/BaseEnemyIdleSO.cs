using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{    
    public class BaseEnemyIdleSO : ScriptableObject
    {
        protected NPCBehaviourController _baseEnemyBehaviour;
        protected MeleeEnemyBehaviour _meleeEnemyBehaviour;
        protected RangeEnemyBehaviour _rangeEnemyBehaviour;
        protected Transform _transform;
        protected GameObject _gameObject;

        protected Transform _playerTransform;

        public virtual void Initialize(GameObject enemyObj, NPCBehaviourController enemyBehav)
        {
            this._gameObject = enemyObj;
            this._transform = enemyObj.transform;
            this._baseEnemyBehaviour = enemyBehav;

            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        public virtual void Initialize(GameObject enemyObj, MeleeEnemyBehaviour meleeEnemyBehav)
        {
            this._gameObject = enemyObj;
            this._transform = enemyObj.transform;
            this._meleeEnemyBehaviour = meleeEnemyBehav;

            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        public virtual void Initialize(GameObject enemyObj, RangeEnemyBehaviour rangeEnemyBehav)
        {
            this._gameObject = enemyObj;
            this._transform = enemyObj.transform;
            this._rangeEnemyBehaviour = rangeEnemyBehav;

            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public virtual void ExecuteEnterLogic() { }
        public virtual void ExecuteExitLogic()
        {
            ResetValues();
        }

        public virtual void ExecuteFrameUpdateLogic()
        {
            // Transitionchecks 
            // Switch State from Idle to AlertState when something alarming is happening (e.g. door kick in, player shoots etc.) and Agent is in noise range
            if (_baseEnemyBehaviour.IsSomethingAlarmingHappening)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.AlertState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Alert</color>' should have been happend now!");
            }
        }

        public virtual void ExecutePhysicsUpdateLogic() { }
        public virtual void ExecuteAnimationTriggerEventLogic(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }
        public virtual void ResetValues() { }
    }
}