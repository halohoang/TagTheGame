using Enemies;
using EnumLibrary;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    public class BaseEnemyInvestigationStateSO : ScriptableObject
    {
        #region Events
        //--------------------------------
        // - - - - -  E V E N T S  - - - - 
        //--------------------------------

        public static event UnityAction<Vector3> OnObstacleAvoidance;
        #endregion

        #region Variables
        //--------------------------------------
        // - - - - -  V A R I A B L E S  - - - - 
        //--------------------------------------

        [Header("Behaviour Settings")]
        [SerializeField] private float _wanderSpeed = 1.0f;
        #region Tooltip
        [Tooltip("The minimum time the Enemy shall wander before choosing a new wander direction, note this will be set by random between min and max value")]
        #endregion
        [SerializeField, Range(0.0f, 10.0f)] private float _minRandomWalkingTime = 2.0f;
        #region Tooltip
        [Tooltip("The maximum time the Enemy shall wander before choosing a new wander direction, note this will be set by random between min and max value")]
        #endregion
        [SerializeField, Range(0.0f, 10.0f)] private float _maxRandomWalkingTime = 6.0f;
        #region Tooltip
        [Tooltip("The minimum range the Enemy shall be able to wander, note this will be set by random between min and max value")]
        #endregion
        [SerializeField, Range(0.0f, 20.0f)] private float _minRandomWalkingRange = 1.0f;
        #region Tooltip
        [Tooltip("The maximum range the Enemy shall be able to wander, note this will be set by random between min and max value")]
        #endregion
        [SerializeField, Range(0.0f, 20.0f)] private float _maxRandomWalkingRange = 5.0f;
        #region Tooltip
        [Tooltip("The minimum range the Enemy shall investigate, note this will be set by random between min and max value")]
        #endregion
        [SerializeField, Range(0.0f, 20.0f)] private float _minInvestigationTime = 5.0f;
        #region Tooltip
        [Tooltip("The maximum range the Enemy shall investigate, note this will be set by random between min and max value")]
        #endregion
        [SerializeField, Range(0.0f, 20.0f)] private float _maxInvestigationTime = 10.0f;
        #region Tooltip
        [Tooltip("Defines the how far the EnemyObject shall check for obstacles in front of it")]
        #endregion
        [SerializeField, Range(1.0f, 5.0f)] private float _distanceToCheckForObstacles = 2.0f;
        #region Tooltip
        [Tooltip("Defines the min Distance that needts to be clear of obstacles for chosing this direction for avoiding detected obstaacles. (needs to be higher than 'Distance to check for obstacles')")]
        #endregion
        [SerializeField, Range(1.0f, 5.0f)] private float _distanceForAvoidCheck = 5.0f;
        #region Tooltip
        [Tooltip("The Objects the Enemy that shall be recognized as obstacles by the EnemyObject")]
        #endregion
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private Vector3[] _directionsToCheckToAvoidObstacle;

        protected NPCBehaviourController _behaviourCtrl;
        //protected MeleeEnemyBehaviour _meleeEnemyBehaviour;
        //protected RangeEnemyBehaviour _rangeEnemyBehaviour;
        protected Transform _transform;
        protected GameObject _gameObject;
        protected Transform _playerTransform;
        private Rigidbody2D _thisEnemyRB2D;
        private Vector3 _previousEventPosition;
        private Vector3 _walkTargedPos;
        private Vector3 _lookdirectionWhileWaitingForTimerEnd;
        private Vector3 _currentObstacleAvoidanceVector;
        private float _investigationTime;
        private float _investTimer;
        private float _rndWalktime;
        private float _rndWalkTimer;
        private float _rndWalkRange;
        private bool _isMoving;
        private bool _movedToLastKnownTargetPos;
        private bool _isWaitingForWalkTimerEnd;
        private bool _isMovingTOCloseToObstacle = false;


        public float RndWalkTimer { get => _rndWalkTimer; set => _rndWalkTimer = value; }
        public float RndWalkRange { get => _rndWalkRange; private set => _rndWalkRange = value; }
        public bool IsMoving { get => _isMoving; private set => _isMoving = value; }
        public bool IsMovingToCloseToObstacle { get => _isMovingTOCloseToObstacle; private set => _isMovingTOCloseToObstacle = value; }

        public Vector3 WalkTargetPos { get => _walkTargedPos; private set => _walkTargedPos = value; }
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
            _investigationTime = Random.Range(_minInvestigationTime, _maxInvestigationTime);

            // Setup Walking Direction/Target Pos
            WalkTargetPos = _behaviourCtrl.LastKnowntargetPos;

            // Setup NavMeshAgent-Properties
            _behaviourCtrl.NavAgent.speed = _wanderSpeed;

            // Setup walking animation
            _behaviourCtrl.Animator.SetBool("Engage", true);
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
                //Todo: Message to UI-Managr for BehaviourFeedback
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.ChaseState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Chase</color>' should have been happend now!");
                return;
            }
            else if (!_movedToLastKnownTargetPos)   // move to last known target position if not moved there yet
            {
                Debug.Log($"<color=orange>{_behaviourCtrl.gameObject.name}</color>: InvestigationState; " +
                    $"This NPC-Object did not reach last known target position yet and should still moove towards it");
                return;
            }
            else // start investigation behaviour
            {
                _movedToLastKnownTargetPos = true;

                //Todo: Message to UI-Managr for BehaviourFeedback

                // set timer for investigation-behaviour
                _investTimer += Time.deltaTime;

                // set timer wor random walking time
                _rndWalkTimer += Time.deltaTime;

                // logic for investigation behaviour
                WalkingConditionCheck();
                SetFacingDirection();

                // execute actual walking according to previous checks and settings
                _behaviourCtrl.NavAgent.isStopped = false;
                _behaviourCtrl.NavAgent.SetDestination(WalkTargetPos);

                _behaviourCtrl.SetIsCollidingWithObject(false);    // reset bool is collidion with other enemy so at the end of an update cycly so the AI actually has a chance to wolk another direction

                // if timer runs out
                if (_investTimer >= _investigationTime)
                {
                    // set was investigating for other state to know it changed from investigation state
                    _behaviourCtrl.SetWasInvestigating(true);

                    // actual state transition
                    if (_behaviourCtrl.IsStandingIdle)
                        _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.IdleState);
                    else
                        _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.MovementState);
                    // after timer runs out send event to inform, that state transition is coming from investigation State and transit to back to normal behaviour -> NPC walks to his last Idle-Position or last Waypoint
                    return;
                }
            }
        }

        public virtual void ExecutePhysicsUpdate()
        {
            // Obstacle check by rycasting
            if (Physics2D.Raycast(_behaviourCtrl.transform.position, WalkTargetPos - _behaviourCtrl.transform.position, _distanceToCheckForObstacles, _obstacleMask))
            {
                IsMovingToCloseToObstacle = true;
                #region debuggers litte helper
                Debug.DrawRay(_behaviourCtrl.transform.position, WalkTargetPos - _behaviourCtrl.transform.position, Color.cyan, 1.5f);
                Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>' is moving to close to Obstacle: '<color=cyan>{IsMovingToCloseToObstacle}</color>'");
                #endregion

                CheckForObstacleAvoidanceVector();
            }
            else
                IsMovingToCloseToObstacle = false;
        }

        public virtual void ExecuteOnAnim﻿ationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }

        public virtual void ResetValues()
        {
            _behaviourCtrl.SetIsSomethingAlarmingHappening(false);  // reset value to false on leaving state
            _movedToLastKnownTargetPos = false;
        }

        /// <summary>
        /// Checks if '<see cref="_investTimer"/>' is running or not and if the maximum walk range was reached while '<see cref="_investTimer"/>' is still running or if Agent 
        /// is moving to close towards an Obatacle-Object. According to Status of the Checks the walk-target-pos and or the animations etc. will be set accordingly.
        /// Also implements logic for movement direction changes if collision with other enemy-agent-object was detected.
        /// </summary>
        private void WalkingConditionCheck()
        {
            // controll structures regarding walking behaviour
            if (_behaviourCtrl.IsCollidingWithObject)
            {
                // stop movement for this cycle
                _behaviourCtrl.NavAgent.isStopped = true;
                _behaviourCtrl.Animator.SetBool("Engage", false);
                _walkTargedPos = _behaviourCtrl.gameObject.transform.position;

                Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>': since EnemyObj collided with´an obstacle, movement was stoped currently. new movementdirection will be calculated");
            }
            else if (RndWalkTimer > _rndWalktime) // is Timmer out of Time
            {
                _isWaitingForWalkTimerEnd = false;

                // Reset Timer and rnd WalkTime
                RndWalkTimer = 0.0f;
                _rndWalktime = Random.Range(_minRandomWalkingTime, _maxRandomWalkingTime);

                // reset Walking Direction/TargetPos
                WalkTargetPos = GetRndMoveDirection();

                // Setup Walking Animation
                _behaviourCtrl.Animator.SetBool("Engage", true);

                Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>': rnd-Walking-Timer ended and was set to 0.0f again; New MovingDirection was calculated and set");
            }
            else if (RndWalkTimer <= _rndWalktime && (Vector2)_behaviourCtrl.transform.position == (Vector2)WalkTargetPos)      // if Timer is still running but Walking-Taget-Position is reached
            {
                if (!_isWaitingForWalkTimerEnd)    // set random lookdirection if it is the first time entering this query
                    _lookdirectionWhileWaitingForTimerEnd = Random.insideUnitCircle;

                _isWaitingForWalkTimerEnd = true;
                _behaviourCtrl.NavAgent.isStopped = true;
                _behaviourCtrl.Animator.SetBool("Engage", false);

                Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>': rnd-Walking-Timer still running; rnd-Walking-Range was reached");
            }
            else if (IsMovingToCloseToObstacle)     // if Agent is walking to close towards an Obstacle, change walkin direction to the oposite
            {
                WalkTargetPos = _currentObstacleAvoidanceVector;
                Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>': Walking Direction was Changed due to walking to close towards obstacle; new direction: {_currentObstacleAvoidanceVector}");
                OnObstacleAvoidance?.Invoke(_currentObstacleAvoidanceVector);
                IsMovingToCloseToObstacle = false;
            }
        }

        /// <summary>
        /// Returns a random direction for the Enemy-Agent to move to by using the '<see cref="_minRandomWalkingRange"/>' and '<see cref="_maxRandomWalkingRange"/>' as origin
        /// for the 
        /// </summary>
        /// <returns></returns>
        private Vector3 GetRndMoveDirection()
        {
            RndWalkRange = Random.Range(_minRandomWalkingRange, _maxRandomWalkingRange);
            return _behaviourCtrl.transform.position + (Vector3)Random.insideUnitCircle * RndWalkRange;
        }

        /// <summary>
        /// Sets the facing direction of the Agent-Object according to its movement direction or randomly when 'standing' and 'waiting' til '<see cref="_investTimer"/>' ends
        /// when max walk-range was already reached
        /// </summary>
        private void SetFacingDirection()
        {
            // setting facing direction
            if (_isWaitingForWalkTimerEnd)
            {
                // setting facing to random when walktimer is still running but walking range was already reached
                Vector2 direction = _lookdirectionWhileWaitingForTimerEnd.normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _thisEnemyRB2D.rotation = angle;
                #region altern rotation for facing direction
                //// a alternative way to manage the facing direction by applying the rotation to the transform instead of to the rigidbody
                //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
                //_baseEnemyBehaviour.transform.rotation = quart;
                #endregion
            }
            else
            {
                // setting facing to walk direction if walking timer has ended and was setup anew
                Vector2 direction = (WalkTargetPos - _behaviourCtrl.transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _thisEnemyRB2D.rotation = angle;
                #region altern rotation for facing direction
                //// a alternative way to manage the facing direction by applying the rotation to the transform instead of to the rigidbody
                //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
                //_baseEnemyBehaviour.transform.rotation = quart;
                #endregion
            }
        }

        /// <summary>
        /// Checks all '<see cref="_directionsToCheckToAvoidObstacle"/>' for beeing clear of Obstacles inside the '<see cref="_distanceToCheckForObstacles"/>'. If so the
        /// '<see cref="_currentObstacleAvoidanceVector"/>' will be set accordingly.
        /// </summary>
        private void CheckForObstacleAvoidanceVector()
        {
            for (int i = 0; i < _directionsToCheckToAvoidObstacle.Length; i++)
            {
                RaycastHit hit;
                Vector3 currentDirectionToCheck = _behaviourCtrl.transform.TransformDirection(_directionsToCheckToAvoidObstacle[i].normalized);

                // if no obstacle was detected in the checked direction -> set this direction to the desired avoidance direction
                if (!Physics.Raycast(_behaviourCtrl.transform.position, currentDirectionToCheck, out hit, _distanceForAvoidCheck, _obstacleMask))
                    _currentObstacleAvoidanceVector = currentDirectionToCheck.normalized;
            }
        }
        #endregion
    }
}