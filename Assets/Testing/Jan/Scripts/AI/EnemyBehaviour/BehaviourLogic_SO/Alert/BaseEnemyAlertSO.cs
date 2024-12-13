﻿using Enemies;
using EnumLibrary;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    public class BaseEnemyAlertSO : ScriptableObject
    {
        #region Events
        //--------------------------------
        // - - - - -  E V E N T S  - - - - 
        //--------------------------------

        public static UnityAction<string, bool, GameObject> OnAlertStateTransition;
        #endregion

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
        protected GameObject _behaviourFeedbackIcon;
        private Rigidbody2D _thisEnemyRB2D;

        #region Tooltip
        [Tooltip("The time (in sec.) the target shall be detected until the state will chage to chase/atack-state. Standard is 1 sec.")]
        #endregion
        [SerializeField] private float _targetDetectionTime = 1.0f;
        
        private float _detectionTimer;              // runs for checking how long the target is detected
        private Vector3 _previousEventPosition;
        private bool _wasTargedDetected = false;

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
            // auto referencing
            _thisEnemyRB2D = _behaviourCtrl.gameObject.GetComponent<Rigidbody2D>();

            if (_behaviourFeedbackIcon == null)
            {
                for (int i = 0; i < _behaviourCtrl.transform.childCount; i++)
                {
                    if (_behaviourCtrl.transform.GetChild(i).gameObject.CompareTag("UI_variableImage"))
                    {
                        _behaviourFeedbackIcon = _behaviourCtrl.transform.GetChild(i).gameObject;
                    }
                }
            }

            // fire message to UI-Managr for BehaviourFeedback
            OnAlertStateTransition?.Invoke(_behaviourCtrl.StateMachine.CurrentState.StateName, _behaviourCtrl.IsSomethingAlarmingHappening, _behaviourFeedbackIcon);            

            FaceAgentTowardsAlarmingEvent(_behaviourCtrl.PositionOfAlarmingEvent);

            // set detection timer to '0'
            _detectionTimer = 0.0f;
        }

        public virtual void ExecuteOnExitState()
        {
            ResetValues();

            // inform BehavCtrl that Alert-State was executed
            _behaviourCtrl.SetWasAlerted(true);
        }

        public virtual void Execute﻿FrameUpdate()
        {
            // Transition-Condition-Check
            if (_behaviourCtrl.IsTargetDetected)
            {
                _wasTargedDetected = true;

                // caching target-position on detection time
                _behaviourCtrl.CacheLastKnownTargetPosition();

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
            else if (_wasTargedDetected)
            {                
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.InvestigationState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>InvestigationState</color>' should have been happend now!");
                return;
            }

            // todo: if implementing more ALert-Behaviour maybe move following logic to 'EnemyAlertStandingSO'; JM (02.11.2023)
            if (_previousEventPosition != _behaviourCtrl.PositionOfAlarmingEvent)    // if the Position of Larming event changed (new/other alarming event set NPC rotation respectively)
                FaceAgentTowardsAlarmingEvent(_behaviourCtrl.PositionOfAlarmingEvent);
        }

        public virtual void ExecutePhysicsUpdate() { }
        public virtual void ExecuteOnAnim﻿ationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }

        public virtual void ResetValues()
        {
            _wasTargedDetected = false;            
            _detectionTimer = 0.0f;
            _behaviourCtrl.SetIsSomethingAlarmingHappening(false);  // reset value to false on leaving state
            OnAlertStateTransition?.Invoke(_behaviourCtrl.StateMachine.CurrentState.StateName, _behaviourCtrl.IsSomethingAlarmingHappening, _behaviourFeedbackIcon);
        }

        // todo: if implementing more ALert-Behaviour maybe move following logic to 'EnemyAlertStandingSO'; JM (02.11.2023)
        /// <summary>
        /// Stores the Position of the alarming event and sets the facing direction of the npc-object towards the position of the alarming event that is happening (e.g. door kick in or 
        /// player shooting around).
        /// </summary>
        /// <param name="positionOfAlarmingEvent"></param>
        /// <param name="noiseRangeOfAlarmingEvent"></param>
        private void FaceAgentTowardsAlarmingEvent(Vector3 positionOfAlarmingEvent)
        {
            // storing event position
            _previousEventPosition = positionOfAlarmingEvent;

            // setting facing direction towards alarming event
            //_behaviourCtrl.gameObject.transform.right = positionOfAlarmingEvent - _behaviourCtrl.gameObject.transform.position;

            //Set facing direction
            Vector2 direction = (positionOfAlarmingEvent - _behaviourCtrl.gameObject.transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Debug.Log($"<color=magenta>AI-Behav | BaseAlertStateSO | {_behaviourCtrl.gameObject.name}</color>: original rotation of Agent-Object: '<color=yellow>{_behaviourCtrl.transform.eulerAngles}</color>' | in local euler agles: '<color=yellow>{_behaviourCtrl.transform.localEulerAngles}</color>'");

            _behaviourCtrl.gameObject.transform.eulerAngles = new Vector3(0, 0, angle);

            Debug.Log($"<color=magenta>AI-Behav | BaseAlertStateSO | {_behaviourCtrl.gameObject.name}</color>: rotation of Agent-Object after applying claculated rotation angle: '<color=yellow>{_behaviourCtrl.transform.eulerAngles}</color>' | in local euler agles: '<color=yellow>{_behaviourCtrl.transform.localEulerAngles}</color>' | calculated anlge <color=yellow>{angle}</color> ");


            Debug.DrawLine(_behaviourCtrl.transform.position, positionOfAlarmingEvent, Color.yellow, 6.0f);
            Debug.DrawRay(_behaviourCtrl.transform.position, direction, Color.magenta, 3.0f);
            Debug.Log($"<color=magenta>AI-Behav | BaseAlertStateSO | {_behaviourCtrl.gameObject.name}</color>: Pos of This NPC-Agent: '<color=yellow>{_behaviourCtrl.transform.position}" +
                $"</color>' | Local Pos of This NPC-Agent: '<color=yellow>{_behaviourCtrl.transform.localPosition}</color>' | position of alarming Event: '<color=yellow>{positionOfAlarmingEvent}</color>'");

            #region altern rotation for facing direction
            //// a alternative way to manage the facing direction by applying the rotation to the transform instead of to the rigidbody
            //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
            //_behaviourCtrl.transform.rotation = quart;
            #endregion
        }
        #endregion
    }
}
