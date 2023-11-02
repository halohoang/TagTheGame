using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{    
    public class BaseEnemyIdleSO : ScriptableObject
    {
        protected BaseEnemyBehaviour _baseEnemyBehaviour;
        protected Transform _transform;
        protected GameObject _gameObject;

        protected Transform _playerTransform;

        public virtual void Initialize(GameObject enemyObj, BaseEnemyBehaviour enemyBehav)
        {
            this._gameObject = enemyObj;
            this._transform = enemyObj.transform;
            this._baseEnemyBehaviour = enemyBehav;

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
            // Switch State from Idle to AlertState when Player is Detected
            if (_baseEnemyBehaviour.IsSomethingAlarmingHappening)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.AlertState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Alert</color>' should have been happend now!");
            }

            // Switch State from Idle to ChaseState when Player is Detected
            if (_baseEnemyBehaviour.IsPlayerDetected)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.ChaseState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Chase</color>' should have been happend now!");
            }
        }

        public virtual void ExecutePhysicsUpdateLogic() { }
        public virtual void ExecuteAnimationTriggerEventLogic(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }
        public virtual void ResetValues() { }
    }
}