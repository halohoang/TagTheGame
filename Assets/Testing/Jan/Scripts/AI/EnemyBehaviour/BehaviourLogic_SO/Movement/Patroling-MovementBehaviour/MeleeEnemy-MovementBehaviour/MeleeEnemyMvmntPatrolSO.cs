﻿using Enemies;
using EnumLibrary;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MeleeEnemy_Mvmnt_Patrol", menuName = "Scriptable Objects/Enemy Logic/Movement Logic/MeleeEnemy Patrol")]
    public class MeleeEnemyMvmntPatrolSO : BaseEnemyMovementSO
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
        [SerializeField] private float _patrolingSpeed = 1.0f;
        #region Tooltip
        [Tooltip("The minimum time the Enemy shall wander before choosing a new wander direction, note this will be set by random between min and max value")]
        #endregion
        [SerializeField, Range(0.0f, 10.0f)] private float _minTimeWaitingAtWaypoint = 2.0f;
        #region Tooltip
        [Tooltip("The maximum time the Enemy shall wander before choosing a new wander direction, note this will be set by random between min and max value")]
        #endregion
        [SerializeField, Range(0.0f, 10.0f)] private float _maxTimeWaitingAtWaypoint = 6.0f;

        #region ObstacleCheck
        //#region Tooltip
        //[Tooltip("Defines the how far the EnemyObject shall check for obstacles in front of it")]
        //#endregion
        //[SerializeField, Range(1.0f, 5.0f)] private float _distanceToCheckForObstacles = 2.0f;
        //#region Tooltip
        //[Tooltip("Defines the min Distance that needts to be clear of obstacles for chosing this direction for avoiding detected obstaacles. (needs to be higher than 'Distance to check for obstacles')")]
        //#endregion
        //[SerializeField, Range(1.0f, 5.0f)] private float _distanceForAvoidCheck = 5.0f;

        //#region Tooltip
        //[Tooltip("The Objects the Enemy that shall be recognized as obstacles by the EnemyObject")]
        //#endregion
        //[SerializeField] private LayerMask _obstacleMask;
        //[SerializeField] private Vector3[] _directionsToCheckToAvoidObstacle;
        #endregion

        private GameObject _currentTargetWaypointObj;
        private Rigidbody2D _thisEnemyRB2D;
        private Vector3 _walkTargedPos;
        private Vector3 _nextWaypoint;
        private Vector3 _previousWaypoint;
        private Vector3 _lookdirectionWhileWaitingForTimerEnd;
        //private Vector3 _currentObstacleAvoidanceVector;
        private float _timer = 0.0f;
        private float _rndWaitAtWaypointTime;        
        private bool _isMoving;
        private bool _isWaitingForWaypointTimerEnd;
        //private bool _isMovingTOCloseToObstacle = false;

        // prperties
        public GameObject CurrentTargetWaypoint { get => _currentTargetWaypointObj; private set => _currentTargetWaypointObj = value; }
        public float Timer { get => _timer; private set => _timer = value; }
        public bool IsMoving { get => _isMoving; private set => _isMoving = value; }
        //public bool IsMovingToCloseToObstacle { get => _isMovingTOCloseToObstacle; private set => _isMovingTOCloseToObstacle = value; }
        public Vector3 WalkTargetPos { get => _walkTargedPos; private set => _walkTargedPos = value; }
        #endregion


        #region Methods
        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------


        #region cusotm Methods
        public override void Initialize(GameObject enemyObj, NPCBehaviourController enemyBehav)
        {
            base.Initialize(enemyObj, enemyBehav);
        }
        #region not yet used constructors
        //public override void Initialize(GameObject enemyObj, MeleeEnemyBehaviour meleeEnemyBehav)
        //{
        //    base.Initialize(enemyObj, meleeEnemyBehav);
        //}
        //public override void Initialize(GameObject enemyObj, RangeEnemyBehaviour rangeEnemyBehav)
        //{
        //    base.Initialize(enemyObj, rangeEnemyBehav);
        //}
        #endregion

        public override void ExecuteOnE﻿nterState() // Todo: find reason why this won't be executed
        {
            Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>': 'ExecuteOnEnterState()' is called");

            base.ExecuteOnE﻿nterState();

            // referencing
            _thisEnemyRB2D = _behaviourCtrl.gameObject.GetComponent<Rigidbody2D>();

            // Setup values
            _isMoving = true;

            // Setup the time the Agent shall walk max in one direction
            _rndWaitAtWaypointTime = Random.Range(_minTimeWaitingAtWaypoint, _maxTimeWaitingAtWaypoint);

            // Setup Walking Direction/Target Pos
            for (int i = 0; i < _behaviourCtrl.WayPoints.Count; i++)    // when already standing at a wayppoint-position, then Set 'WalkTargetPos' to next waypoint
            {
                if (_behaviourCtrl.gameObject.transform.position == _behaviourCtrl.WayPoints[i].transform.position)
                {
                    _nextWaypoint = _behaviourCtrl.WayPoints[i++].transform.position;
                    _currentTargetWaypointObj = _behaviourCtrl.WayPoints[i++];
                    break;
                }
                else
                {
                    _nextWaypoint = _behaviourCtrl.WayPoints[1].transform.position;
                    _currentTargetWaypointObj = _behaviourCtrl.WayPoints[1];
                    break;
                }
            }            
            WalkTargetPos = _nextWaypoint;

            // Setup NavMeshAgent-Properties
            _behaviourCtrl.NavAgent.speed = _patrolingSpeed;

            // Setup walking animation
            _behaviourCtrl.Animator.SetBool("Engage", true);
        }

        public override void ExecuteOnExitState()
        {
            base.ExecuteOnExitState();

            _isMoving = false;

            // setup walking animation
            _behaviourCtrl.Animator.SetBool("Engage", false);
        }

        public override void ExecuteFrameUpdate()
        {
            Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>': 'ExecuteOnFrameUpdate()' is called");

            base.ExecuteFrameUpdate();

            WalkingConditionCheck();
            SetFacingDirection();

            // execute actual walking according to previous checks and settings
            _behaviourCtrl.NavAgent.isStopped = false;
            _behaviourCtrl.NavAgent.SetDestination(WalkTargetPos);

            _behaviourCtrl.SetIsCollidingWithObject(false);    // reset bool is collidion with other enemy so at the end of an update cycly so the AI actually has a chance to wolk another direction
        }

        public override void ExecutePhysicsUpdate()
        {
            base.ExecutePhysicsUpdate();

            #region ObstacleCheck
            //// Check for obstacle collision course
            //if (Physics2D.Raycast(_behaviourCtrl.transform.position, WalkTargetPos - _behaviourCtrl.transform.position, _distanceToCheckForObstacles, _obstacleMask))
            //{
            //    IsMovingToCloseToObstacle = true;
            //    #region debuggers litte helper
            //    Debug.DrawRay(_behaviourCtrl.transform.position, WalkTargetPos - _behaviourCtrl.transform.position, Color.cyan, 1.5f);
            //    Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>' is moving to close to Obstacle: '<color=cyan>{IsMovingToCloseToObstacle}</color>'");
            //    #endregion

            //    CheckForObstacleAvoidanceVector();
            //}
            //else
            //    IsMovingToCloseToObstacle = false;
            #endregion
        }

        public override void ExecuteOnAnim﻿﻿ationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerTyoe)
        {
            base.ExecuteOnAnim﻿﻿ationTriggerEvent(animTriggerTyoe);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }

        /// <summary>
        /// Checks if '<see cref="_timer"/>' is running or not and if the maximum walk range was reached while '<see cref="_timer"/>' is still running or if Agent 
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
            else if (_behaviourCtrl.transform.position == WalkTargetPos) // if waypoit is reached -> wait
            {
                // 1. Setup Timer
                Timer += Time.deltaTime;

                // 2. Halt movement for as long as timer runs
                _isWaitingForWaypointTimerEnd = true;
                _behaviourCtrl.Animator.SetBool("Engage", false);

                // 3. check if Timer is out of time and set values respectively
                SetNewWalkTargetPosOnTimerEnd();
            }

            // if last waypoint is reached
            if (_currentTargetWaypointObj == _behaviourCtrl.WayPoints[_behaviourCtrl.WayPoints.Count] && _behaviourCtrl.gameObject.transform.position == WalkTargetPos)
            {
                // think about logic for going through waypoints backwards; JM (10.11.24)
            }

        #region Old Code from RndWalk-SO
        // Todo: continue work here

        //    else if (Timer > _rndWaitAtWaypointTime)               // is Timmer out of Time
        //    {
        //        _isWaitingForWaypointTimerEnd = false;

        //        // Reset Timer and rnd WalkTime
        //        Timer = 0.0f;
        //        _rndWaitAtWaypointTime = Random.Range(_minTimeWaitingAtWaypoint, _maxTimeWaitingAtWaypoint);

        //        // reset Walking Direction/TargetPos
        //        //WalkTargetPos = GetRndMoveDirection();

        //        // Setup Walking Animation
        //        _behaviourCtrl.Animator.SetBool("Engage", true);

        //        Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>': rnd-Walking-Timer ended and was set to 0.0f again; New MovingDirection was calculated and set");
        //    }
        //    else if (Timer <= _rndWaitAtWaypointTime && (Vector2)_behaviourCtrl.transform.position == (Vector2)WalkTargetPos)      // if Timer is still running but Walking-Taget-Position is reached
        //    {
        //        if (!_isWaitingForWaypointTimerEnd)    // set random lookdirection if it is the first time entering this query
        //            _lookdirectionWhileWaitingForTimerEnd = Random.insideUnitCircle;

        //        _isWaitingForWaypointTimerEnd = true;
        //        _behaviourCtrl.NavAgent.isStopped = true;
        //        _behaviourCtrl.Animator.SetBool("Engage", false);

        //        Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>': rnd-Walking-Timer still running; rnd-Walking-Range was reached");
        //    }
        //    else if (IsMovingToCloseToObstacle)     // if Agent is walking to close towards an Obstacle, change walkin direction to the oposite
        //    {
        //        WalkTargetPos = _currentObstacleAvoidanceVector;
        //        Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>': Walking Direction was Changed due to walking to close towards obstacle; new direction: {_currentObstacleAvoidanceVector}");
        //        OnObstacleAvoidance?.Invoke(_currentObstacleAvoidanceVector);
        //        IsMovingToCloseToObstacle = false;
        //    }
        //}
        #endregion
        }

        /// <summary>
        /// Checks if <see cref="Timer"/> runs out of time and respectively sets the <see cref="WalkTargetPos"/> to the next waypoint (<see cref="_nextWaypoint"/>). Also sets
        /// <see cref="_previousWaypoint"/> and <see cref="_currentTargetWaypointObj"/> respectively to new values.
        /// </summary>
        private void SetNewWalkTargetPosOnTimerEnd()
        {
            if (Timer > _rndWaitAtWaypointTime) // is Timer out of time
            {
                // reset Timer & new waiting Time
                Timer = 0.0f;
                _rndWaitAtWaypointTime = Random.Range(_minTimeWaitingAtWaypoint, _maxTimeWaitingAtWaypoint);

                // reset Walking Direction/TargetPos
                _previousWaypoint = WalkTargetPos;

                for (int i = 0; i < _behaviourCtrl.WayPoints.Count; i++)    // set _nexWaypoint and _currentTargetWaypoint to new values on Timer End
                {
                    if (_currentTargetWaypointObj == _behaviourCtrl.WayPoints[i])
                    {
                        _nextWaypoint = _behaviourCtrl.WayPoints[i++].transform.position;
                        _currentTargetWaypointObj = _behaviourCtrl.WayPoints[i++];
                        break;
                    }
                }
                WalkTargetPos = _nextWaypoint;

                Debug.Log($"'<color=orange>{_behaviourCtrl.gameObject.name}</color>': waiting-on-waypoint-timer ended and was set to 0.0f again; " +
                    $"next Waypoint('<color=orange>{_currentTargetWaypointObj.name}</color>') is set as WalkTargetPos");
            }
        }

        /// <summary>
        /// Sets the facing direction of the Agent-Object according to its movement direction or randomly when 'standing' and 'waiting' til '<see cref="_timer"/>' ends
        /// when max walk-range was already reached
        /// </summary>
        private void SetFacingDirection()
        {
            // setting facing direction
            if (_isWaitingForWaypointTimerEnd)
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

        ///// <summary>
        ///// Checks all '<see cref="_directionsToCheckToAvoidObstacle"/>' for beeing clear of Obstacles inside the '<see cref="_distanceToCheckForObstacles"/>'. If so the
        ///// '<see cref="_currentObstacleAvoidanceVector"/>' will be set accordingly.
        ///// </summary>
        //private void CheckForObstacleAvoidanceVector()
        //{
        //    for (int i = 0; i < _directionsToCheckToAvoidObstacle.Length; i++)
        //    {
        //        RaycastHit hit;
        //        Vector3 currentDirectionToCheck = _behaviourCtrl.transform.TransformDirection(_directionsToCheckToAvoidObstacle[i].normalized);

        //        // if no obstacle was detected in the checked direction -> set this direction to the desired avoidance direction
        //        if (!Physics.Raycast(_behaviourCtrl.transform.position, currentDirectionToCheck, out hit, _distanceForAvoidCheck, _obstacleMask))
        //            _currentObstacleAvoidanceVector = currentDirectionToCheck.normalized;
        //    }
        //}
        #endregion

        #endregion
    }
}