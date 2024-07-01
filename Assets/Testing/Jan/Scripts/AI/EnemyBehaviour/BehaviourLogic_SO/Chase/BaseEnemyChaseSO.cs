using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    public class BaseEnemyChaseSO : ScriptableObject
    {
        protected NPCBehaviourController _behaviourCtrl;
        //protected MeleeEnemyBehaviour _meleeEnemyBehaviour;
        //protected RangeEnemyBehaviour _rangeEnemyBehaviour;
        protected Transform _transform;
        protected GameObject _gameObject;

        protected Transform _playerTransform;
        private Rigidbody2D _thisEnemyRB2D;

        public virtual void Initialize(GameObject enemyObj, NPCBehaviourController enemyBehav)
        {
            this._gameObject = enemyObj;
            this._transform = enemyObj.transform;
            this._behaviourCtrl = enemyBehav;

            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        //public virtual void Initialize(GameObject enemyObj, MeleeEnemyBehaviour meleeEnemyBehav)
        //{
        //    this._gameObject = enemyObj;
        //    this._transform = enemyObj.transform;
        //    this._meleeEnemyBehaviour = meleeEnemyBehav;

        //    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //}
        //public virtual void Initialize(GameObject enemyObj, RangeEnemyBehaviour rangeEnemyBehav)
        //{
        //    this._gameObject = enemyObj;
        //    this._transform = enemyObj.transform;
        //    this._rangeEnemyBehaviour = rangeEnemyBehav;

        //    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //}

        public virtual void ExecuteEnterLogic() 
        {
            // get references
            _thisEnemyRB2D = _behaviourCtrl.gameObject.GetComponent<Rigidbody2D>();

            // setup NavMeshAgent Properties
            _behaviourCtrl.NavAgent.speed = _behaviourCtrl.ChasingSpeed;            
        }

        public virtual void ExecuteExitLogic()
        {
            // setup NavMeshAgent Properties
            _behaviourCtrl.NavAgent.speed = _behaviourCtrl.MovementSpeed;            

            ResetValues();
        }

        public virtual void ExecuteFrameUpdateLogic()
        {            
            // facing Player Position
            Vector2 direction = (_behaviourCtrl.PlayerObject.transform.position - _behaviourCtrl.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _thisEnemyRB2D.rotation = angle;
            #region altern rotation for facing direction
            //// a alternative way to manage the facing direction by applying the rotation to the transform instead of to the rigidbody
            //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
            //_baseEnemyBehaviour.transform.rotation = quart;
            #endregion                        
        }

        public virtual void ExecutePhysicsUpdateLogic() { }
        public virtual void ExecuteAnimationTriggerEventLogic(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }
        public virtual void ResetValues() { }
    }
}