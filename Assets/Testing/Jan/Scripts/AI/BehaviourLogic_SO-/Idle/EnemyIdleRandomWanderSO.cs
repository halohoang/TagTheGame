using Enemies;
using EnumLibrary;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Enemy-Idle-RadomWander", menuName = "Scriptable Objects/Enemy Logic/Idle Logic/Random Wander")]
    public class EnemyIdleRandomWanderSO : BaseEnemyIdleSO
    {
        [Header("Behaviour Settings")]
        [SerializeField] private float _wanderSpeed = 1.0f;
        [Tooltip("The minimum time the Enemy shall wander before choosing a new wander direction, note this will be set by random between min and max value")]
        [SerializeField, Range(0.0f, 10.0f)] private float _minRandomWalkingTime = 2.0f;
        [Tooltip("The maximum time the Enemy shall wander before choosing a new wander direction, note this will be set by random between min and max value")]
        [SerializeField, Range(0.0f, 10.0f)] private float _maxRandomWalkingTime = 6.0f;
        [Tooltip("The minimum range the Enemy shall be able to wander, note this will be set by random between min and max value")]
        [SerializeField, Range(0.0f, 20.0f)] private float _minRandomWalkingRange = 1.0f;
        [Tooltip("The maximum range the Enemy shall be able to wander, note this will be set by random between min and max value")]
        [SerializeField, Range(0.0f, 20.0f)] private float _maxRandomWalkingRange = 5.0f;
        [Tooltip("Defines the how far the EnemyObject shall check for obstacles in front of it")]
        [SerializeField, Range(1.0f, 5.0f)] private float _distanceToCheckForObstacles = 2.0f;
        [Tooltip("The Objects the Enemy that shall be recognized as obstacles by the EnemyObject")]
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private Vector3[] _directionsToCheckToAvoidObstacle;

        private Rigidbody2D _thisEnemyRB2D;
        private Vector3 _walkTargedPos;
        private float _timer = 0.0f;
        private float _rndWalktime;
        private float _rndWalkRange;
        private Vector3 _lookdirectionWhileWaitingForTimerEnd;
        private bool _isMoving;
        private bool _isWaitingForWalkTimerEnd;
        private bool _isMovingTOCloseToObstacle = false;

        public float Timer { get => _timer; private set => _timer = value; }
        public float RndWalkRange { get => _rndWalkRange; private set => _rndWalkRange = value; }
        public bool IsMoving { get => _isMoving; private set => _isMoving = value; }
        public bool IsMovingTOCloseToObstacle { get => _isMovingTOCloseToObstacle; private set => _isMovingTOCloseToObstacle = value; }
        public Vector3 WalkTargetPos { get => _walkTargedPos; private set => _walkTargedPos = value; }

        public override void Initialize(GameObject enemyObj, BaseEnemyBehaviour enemyBehav)
        {
            base.Initialize(enemyObj, enemyBehav);
        }

        public override void ExecuteEnterLogic()
        {
            base.ExecuteEnterLogic();

            _thisEnemyRB2D = _baseEnemyBehaviour.gameObject.GetComponent<Rigidbody2D>();

            _isMoving = true;

            // Setup the time the Agent shall walk max in one direction
            _rndWalktime = Random.Range(_minRandomWalkingTime, _maxRandomWalkingTime);

            // Setup Walking Direction/Target Pos
            WalkTargetPos = GetRndMoveDirection();

            // Setup NavMeshAgent-Properties
            _baseEnemyBehaviour.NavAgent.speed = _wanderSpeed;

            // setup walking animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", true);
        }

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();

            _isMoving = false;

            // setup walking animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", false);
        }

        public override void ExecuteFrameUpdateLogic()
        {
            base.ExecuteFrameUpdateLogic();

            // Setup Timer
            Timer += Time.deltaTime;

            //------------------------------------------------------------------
            #region Logic for avoiding Obstacles
            // controll structures regarding walking behaviour
            if (_baseEnemyBehaviour.IsCollidingWithWall)
            {
                // Set new walking direction to the opposite of the direction to the CollisionObject and move EnemyObj towards new direction
                WalkTargetPos = -(_baseEnemyBehaviour.CollisionObjectPos - (Vector2)_baseEnemyBehaviour.transform.position * RndWalkRange);
                _baseEnemyBehaviour.NavAgent.SetDestination(WalkTargetPos);
                Debug.Log($"'<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>': since EnemyObj collided with a Wall another MovementDirection was chosen " +
                    $"and Movement was continued");

                _baseEnemyBehaviour.SetIsCollidingWithWall(false);
            }
            #endregion

            //-------------------------------------------------------------------

            #region Query checking Walking Conditions
            if (Timer > _rndWalktime)               // is Timmer out of Time
            {
                _isWaitingForWalkTimerEnd = false;

                // Reset Timer and rnd WalkTime
                Timer = 0.0f;
                _rndWalktime = Random.Range(_minRandomWalkingTime, _maxRandomWalkingTime);

                // reset Walking Direction/TargetPos
                WalkTargetPos = GetRndMoveDirection();

                // Setup Walking Animation
                _baseEnemyBehaviour.Animator.SetBool("Engage", true);

                Debug.Log($"'<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>': rnd-Walking-Timer ended and was set to 0.0f again; New MovingDirection was calculated and set");
            }
            else if (Timer <= _rndWalktime && _baseEnemyBehaviour.transform.position == WalkTargetPos)      // if Timer is still running but Walking-Taget-Position is reached
            {
                if (!_isWaitingForWalkTimerEnd)    // set random lookdirection if it is the first time entering this query
                    _lookdirectionWhileWaitingForTimerEnd = Random.insideUnitCircle;

                _isWaitingForWalkTimerEnd = true;
                _baseEnemyBehaviour.NavAgent.isStopped = true;
                _baseEnemyBehaviour.Animator.SetBool("Engage", false);


                Debug.Log($"'<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>': rnd-Walking-Timer still running; rnd-Walking-Range was reached");
            }
            else if (IsMovingTOCloseToObstacle)     // if Agent is walking to close towards an Obstacle, change walkin direction to the oposite
            {
                WalkTargetPos = -(WalkTargetPos - _baseEnemyBehaviour.transform.position);
                Debug.Log($"'<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>': Walking Direction was Changed due to walking to close towards obstacle");
                IsMovingTOCloseToObstacle = false;
            }
            #endregion

            #region Query setting Facing direction
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
                Vector2 direction = (WalkTargetPos - _baseEnemyBehaviour.transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _thisEnemyRB2D.rotation = angle;
                #region altern rotation for facing direction
                //// a alternative way to manage the facing direction by applying the rotation to the transform instead of to the rigidbody
                //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
                //_baseEnemyBehaviour.transform.rotation = quart;
                #endregion                
            }
            #endregion

            // execute actual walking
            _baseEnemyBehaviour.NavAgent.isStopped = false;
            _baseEnemyBehaviour.NavAgent.SetDestination(WalkTargetPos);
        }

        public override void ExecutePhysicsUpdateLogic()
        {
            base.ExecutePhysicsUpdateLogic();

            if (Physics2D.Raycast(_baseEnemyBehaviour.transform.position, WalkTargetPos - _baseEnemyBehaviour.transform.position, _distanceToCheckForObstacles, _obstacleMask))
            {
                IsMovingTOCloseToObstacle = true;
                Debug.DrawRay(_baseEnemyBehaviour.transform.position, WalkTargetPos - _baseEnemyBehaviour.transform.position, Color.cyan, 1.5f);
                Debug.Log($"'<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>' is moving to close to Obstacle: '<color=cyan>{IsMovingTOCloseToObstacle}</color>'");
            }
            else
                IsMovingTOCloseToObstacle = false;
        }

        public override void ExecuteAnimationTriggerEventLogic(Enum_Lib.EAnimationTriggerType animTriggerTyoe)
        {
            base.ExecuteAnimationTriggerEventLogic(animTriggerTyoe);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }

        private Vector3 GetRndMoveDirection()
        {
            RndWalkRange = Random.Range(_minRandomWalkingRange, _maxRandomWalkingRange);
            return _baseEnemyBehaviour.transform.position + (Vector3)Random.insideUnitCircle * RndWalkRange;
        }


        /*
         ---_ Solution for Avoding Obstacles to orientate to_---
         ----------------------------------------
         [SerializeField] private Vector3[] directionsToCheckWhenAvoidingObstacle;

        private Vector3 CalculateObstacleAvoidanceVector()
        {
            Vector3 obstacleAvoidanceVector = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, assignedFlock.ObstacleDistance, obstacleMask))
            {
                obstacleAvoidanceVector = FindBestDirectionToAvoidObstacle();
            }
            else
            {
                currentObstacleAvoidanceVector = Vector3.zero;
            }

            return obstacleAvoidanceVector;
        }
         

        private Vector3 FindBestDirectionToAvoidObstacle()
        {
            if (currentObstacleAvoidanceVector != Vector3.zero)
            {
                RaycastHit hit;
                if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, assignedFlock.ObstacleDistance, obstacleMask))
                {
                    return currentObstacleAvoidanceVector;
                }
            }
            float maxDistance = int.MinValue;
            Vector3 SelectedDirection = Vector3.zero;
            for (int i = 0; i < directionsToCheckWhenAvoidingObstacle.Length; i++)
            {
                RaycastHit hit;
                Vector3 currentDirection = MyTransform.TransformDirection(directionsToCheckWhenAvoidingObstacle[i].normalized);
                if(Physics.Raycast(MyTransform.position, currentDirection, out hit, assignedFlock.ObstacleDistance, obstacleMask))
                {
                    float currentDistance = (hit.point - MyTransform.position).sqrMagnitude;
                    if(currentDistance > maxDistance)
                    {
                        maxDistance = currentDistance;
                        SelectedDirection = currentDirection;
                    }
                }
                else
                {
                    SelectedDirection = currentDirection;
                    currentObstacleAvoidanceVector = currentDirection.normalized;
                    return SelectedDirection.normalized;
                }
            }
            return SelectedDirection.normalized;
        }
         */
    }
}