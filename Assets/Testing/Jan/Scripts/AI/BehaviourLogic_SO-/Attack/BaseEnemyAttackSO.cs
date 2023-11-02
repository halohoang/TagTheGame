using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    public class BaseEnemyAttackSO : ScriptableObject
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
        public virtual void ExecuteFrameUpdateLogic() { }
        public virtual void ExecutePhysicsUpdateLogic() { }
        public virtual void ExecuteAnimationTriggerEventLogic(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }
        public virtual void ResetValues() { }
    }
}