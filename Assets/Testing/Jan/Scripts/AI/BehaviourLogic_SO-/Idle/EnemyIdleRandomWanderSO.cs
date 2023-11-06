using Enemies;
using EnumLibrary;
using System.Collections.Generic;
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

        private Rigidbody2D _thisEnemyRB2D;
        private Vector3 _walkTargedPos;
        private Vector2 _lastPositionOringin;
        private float _timer = 0.0f;
        private float _rndWalktime;
        private float _rndWalkRange;
        private bool _isMoving;
        private bool _isMovingTOCloseToObstacle = false;

        public float Timer { get => _timer; private set => _timer = value; }
        public float RndWalkRange { get => _rndWalkRange; private set => _rndWalkRange = value; }
        public bool IsMoving { get => _isMoving; private set => _isMoving = value; }
        public bool IsMovingTOCloseToObstacle { get => _isMovingTOCloseToObstacle; private set => _isMovingTOCloseToObstacle = value; }
        public Vector3 WalkTargedPos { get => _walkTargedPos; private set => _walkTargedPos = value; }

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
            WalkTargedPos = GetRndMoveDirection();

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

            //// controll structures regarding walking behaviour
            //if (_baseEnemyBehaviour.IsCollidingWithWall)
            //{
            //    // Set new walking direction to the opposite of the direction to the CollisionObject and move EnemyObj towards new direction
            //    WalkTargedPos = -(_baseEnemyBehaviour.CollisionObjectPos - (Vector2)_baseEnemyBehaviour.transform.position * RndWalkRange);
            //    _baseEnemyBehaviour.NavAgent.SetDestination(WalkTargedPos);
            //    Debug.Log($"'<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>': since EnemyObj collided with a Wall another MovementDirection was chosen " +
            //        $"and Movement was continued");

            //    _baseEnemyBehaviour.SetIsCollidingWithWall(false);
            //}

            if (Timer > _rndWalktime)               // is Timmer out of Time
            {
                // Reset Timer and rnd WalkTime
                Timer = 0.0f;
                _rndWalktime = Random.Range(_minRandomWalkingTime, _maxRandomWalkingTime);

                // reset Walking Direction/TargetPos
                WalkTargedPos = GetRndMoveDirection();

                // Setup Walking Animation
                _baseEnemyBehaviour.Animator.SetBool("Engage", true);

                Debug.Log($"'<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>': rnd-Walking-Timer ended and was set to 0.0f again; New MovingDirection was calculated and set");
            }
            else if (Timer <= _rndWalktime && Vector2.Distance(_lastPositionOringin, WalkTargedPos) >= RndWalkRange)    // todo: (!!!) continue here with Debugging; JM (06.11.23) 
            {
                _baseEnemyBehaviour.NavAgent.isStopped = true;
                _baseEnemyBehaviour.Animator.SetBool("Engage", false);

                Debug.Log($"'<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>': rnd-Walking-Timer still running; rnd-Walking-Range was reached");
            }
            else if (IsMovingTOCloseToObstacle)
            {
                WalkTargedPos = -(WalkTargedPos - _baseEnemyBehaviour.transform.position);
                Debug.Log($"'<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>': Walking Direction was Changed due to walking to close towards obstacle");
                IsMovingTOCloseToObstacle = false;
            }

            // setting facing to walk direction
            Vector2 direction = (WalkTargedPos - _baseEnemyBehaviour.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _thisEnemyRB2D.rotation = angle;
            #region altern rotation for facing direction
            //// a alternative way to manage the facing direction by applying the rotation to the transform instead of to the rigidbody
            //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
            //_baseEnemyBehaviour.transform.rotation = quart;
            #endregion

            // execute actual walking
            _baseEnemyBehaviour.NavAgent.isStopped = false;
            _baseEnemyBehaviour.NavAgent.SetDestination(WalkTargedPos);
        }

        public override void ExecutePhysicsUpdateLogic()
        {
            base.ExecutePhysicsUpdateLogic();

            if (Physics2D.Raycast(_baseEnemyBehaviour.transform.position, WalkTargedPos - _baseEnemyBehaviour.transform.position, _distanceToCheckForObstacles, _obstacleMask))
            {
                IsMovingTOCloseToObstacle = true;
                Debug.DrawRay(_baseEnemyBehaviour.transform.position, WalkTargedPos - _baseEnemyBehaviour.transform.position, Color.cyan, 1.5f);
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
            _lastPositionOringin = _baseEnemyBehaviour.transform.position;      // caching position before calculating new walkdirection/-range
            RndWalkRange = Random.Range(_minRandomWalkingRange, _maxRandomWalkingRange);
            return _baseEnemyBehaviour.transform.position + (Vector3)Random.insideUnitCircle * RndWalkRange;
        }
    }
}