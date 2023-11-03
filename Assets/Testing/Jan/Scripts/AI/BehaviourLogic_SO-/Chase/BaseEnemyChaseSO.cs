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
        private Rigidbody2D _thisEnemyRB2D;

        public virtual void Initialize(GameObject enemyObj, BaseEnemyBehaviour enemyBehav)
        {
            this._gameObject = enemyObj;
            this._transform = enemyObj.transform;
            this._baseEnemyBehaviour = enemyBehav;

            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public virtual void ExecuteEnterLogic() 
        {
            // get references
            _thisEnemyRB2D = _baseEnemyBehaviour.gameObject.GetComponent<Rigidbody2D>();

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
            Vector2 direction = (_baseEnemyBehaviour.PlayerObject.transform.position - _baseEnemyBehaviour.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _thisEnemyRB2D.rotation = angle;
            #region altern rotation for facing direction
            //// a alternative way to manage the facing direction by applying the rotation to the transform instead of to the rigidbody
            //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
            //_baseEnemyBehaviour.transform.rotation = quart;
            #endregion

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