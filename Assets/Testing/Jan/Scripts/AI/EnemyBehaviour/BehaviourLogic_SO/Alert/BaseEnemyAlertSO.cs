using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    public class BaseEnemyAlertSO : ScriptableObject
    {
        #region Variables
        //--------------------------------------
        // - - - - -  V A R I A B L E S  - - - - 
        //--------------------------------------

        protected NPCBehaviourController _behaviourCtrl;
        //protected MeleeEnemyBehaviour _meleeEnemyBehaviour;
        //protected RangeEnemyBehaviour _rangeEnemyBehaviour;
        protected Transform _transform;
        protected GameObject _gameObject;

        protected Transform _playerTransform;

        private Vector3 _previousEventPosition;
        #endregion


        #region Methods
        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------

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
            FaceAgentTowardsAlarmingEvent(_behaviourCtrl.PositionOfAlarmingEvent, _behaviourCtrl.NoiseRangeOfAlarmingEvent);            
        }

        public virtual void ExecuteExitLogic()
        {
            ResetValues();
        }

        public virtual void ExecuteFrameUpdateLogic()
        {
            // Transition-Condition-Check
            if (_behaviourCtrl.IsTargetDetected)
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.ChaseState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Chase</color>' should have been happend now!");
                return;
            }

            // todo: if implementing more ALert-Behaviour maybe move following logic to 'EnemyAlertStandingSO'; JM (02.11.2023)
            if (_previousEventPosition != _behaviourCtrl.PositionOfAlarmingEvent)    // if the Position of Larming event changed (new/other alarming event set NPC rotation respectively)
                FaceAgentTowardsAlarmingEvent(_behaviourCtrl.PositionOfAlarmingEvent, _behaviourCtrl.NoiseRangeOfAlarmingEvent);
        }

        public virtual void ExecutePhysicsUpdateLogic() { }
        public virtual void ExecuteAnimationTriggerEventLogic(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }

        public virtual void ResetValues()
        {
            _behaviourCtrl.SetIsSomethingAlarmingHappening(false);  // reset value to false on leaving state
        }

        // todo: if implementing more ALert-Behaviour maybe move following logic to 'EnemyAlertStandingSO'; JM (02.11.2023)
        /// <summary>
        /// Stores the Position of the alarming event and sets the facing direction of the npc-object towards the position of the alarming event that is happening (e.g. door kick in or 
        /// player shooting around).
        /// </summary>
        /// <param name="positionOfAlarmingEvent"></param>
        /// <param name="noiseRangeOfAlarmingEvent"></param>
        private void FaceAgentTowardsAlarmingEvent(Vector3 positionOfAlarmingEvent, float noiseRangeOfAlarmingEvent)
        {
            // storing event position
            _previousEventPosition = positionOfAlarmingEvent;

            // setting facing direction towards alarming event
            //_behaviourCtrl.gameObject.transform.right = positionOfAlarmingEvent - enemieColliders[i].gameObject.transform.position;
            Vector2 direction = (positionOfAlarmingEvent - _behaviourCtrl.gameObject.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _behaviourCtrl.gameObject.GetComponent<Rigidbody2D>().rotation = angle;
        }
        #endregion
    }
}
