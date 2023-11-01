using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    public class BaseEnemyChaseSO : ScriptableObject
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

        public virtual void ExecuteEnterLogic() 
        {
            // setup NavMeshAgent Properties
            _baseEnemyBehaviour.NavAgent.speed = _baseEnemyBehaviour.ChasingSpeed;

            // Set proper Animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", true);
        }

        public virtual void ExecuteExitLogic()
        {
            // setup NavMeshAgent Properties
            _baseEnemyBehaviour.NavAgent.speed = _baseEnemyBehaviour.MovementSpeed;

            // Set proper Animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", false);

            ResetValues();
        }

        public virtual void ExecuteFrameUpdateLogic()
        {
            // Set Movement-Destination for NavMeshAgent
            _baseEnemyBehaviour.NavAgent.SetDestination(_baseEnemyBehaviour.PlayerObject.transform.position);

            // facing Player Position
            _baseEnemyBehaviour.gameObject.transform.right = _baseEnemyBehaviour.PlayerObject.transform.position - _baseEnemyBehaviour.gameObject.transform.position;

            // Transition-Condition-Check (if not Player is detected anymore -> switch to IdleState again)
            if (!_baseEnemyBehaviour.IsPlayerDetected)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.IdleState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Chase</color>' to '<color=orange>Idle</color>' should have been happend now!");
            }
        }

        public virtual void ExecutePhysicsUpdateLogic() { }
        public virtual void ExecuteAnimationTriggerEventLogic(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }
        public virtual void ResetValues() { }
    }
}