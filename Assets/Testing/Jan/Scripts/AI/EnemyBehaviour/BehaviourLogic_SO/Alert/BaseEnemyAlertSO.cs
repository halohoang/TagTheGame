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

        #region
        [Tooltip("The time (in sec.) the target shall be detected until the state will chage to chase/atack-state. Standard is 1 sec.")]
        #endregion
        [SerializeField] private float _targetDetectionTime = 1.0f;

        private float _detectionTimer;              // runs for checking how long the target is detected
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

        public virtual void ExecuteOnE﻿nterState()
        {
            //Todo: Message to UI-Managr for BehaviourFeedback
            FaceAgentTowardsAlarmingEvent(_behaviourCtrl.PositionOfAlarmingEvent, _behaviourCtrl.NoiseRangeOfAlarmingEvent);
            // set detection timer to '0'
            _detectionTimer = 0.0f;
        }

        public virtual void ExecuteOnExitState()
        {
            ResetValues();
        }

        public virtual void Execute﻿FrameUpdate()
        {
            // Transition-Condition-Check
            if (_behaviourCtrl.IsTargetDetected)
            {
                _detectionTimer += Time.deltaTime;

                // if timer runs out and target is still detected -> transit to CHaseState
                if (_detectionTimer == _targetDetectionTime)
                {
                    //Todo: Message to UI-Managr for BehaviourFeedback
                    _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.ChaseState);
                    Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Chase</color>' should have been happend now!");
                    return;
                }
            }
            else
            {
                //Todo: Message to UI-Managr for BehaviourFeedback

                // reset detection TImer
                _detectionTimer = 0.0f;
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.InvestigationState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>InvestigationState</color>' should have been happend now!");
                return;
            }

            // todo: if implementing more ALert-Behaviour maybe move following logic to 'EnemyAlertStandingSO'; JM (02.11.2023)
            if (_previousEventPosition != _behaviourCtrl.PositionOfAlarmingEvent)    // if the Position of Larming event changed (new/other alarming event set NPC rotation respectively)
                FaceAgentTowardsAlarmingEvent(_behaviourCtrl.PositionOfAlarmingEvent, _behaviourCtrl.NoiseRangeOfAlarmingEvent);
        }

        public virtual void ExecutePhysicsUpdate() { }
        public virtual void ExecuteOnAnim﻿ationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }

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
