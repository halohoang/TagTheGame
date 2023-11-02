using Enemies;
using EnumLibrary;
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
        [Tooltip("Defines the Distance to an Obstacle at wich an EnemyObject shall turn away when previously walking towards the obstacle")]
        [SerializeField, Range(1.5f, 5.0f)] private float _distanceTowardsWall = 2.0f;

        private Vector3 _moveTargedPos;
        private float _timer = 0.0f;
        private bool _isMoving;
        private bool _isMovingTOCloseToObstacle = false;

        public bool IsMoving { get => _isMoving; private set => _isMoving = value; }

        public override void Initialize(GameObject enemyObj, BaseEnemyBehaviour enemyBehav)
        {
            base.Initialize(enemyObj, enemyBehav);
        }

        public override void ExecuteEnterLogic()
        {
            base.ExecuteEnterLogic();

            _isMoving = true;

            _moveTargedPos = GetRndMoveDirection();
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

            _timer += Time.deltaTime;

            // setting facing to walk direction
            _baseEnemyBehaviour.transform.right = _moveTargedPos - _baseEnemyBehaviour.transform.position;

            // actual walking
            _baseEnemyBehaviour.NavAgent.SetDestination(_moveTargedPos);

            float rndWalktime = Random.Range(_minRandomWalkingTime, _maxRandomWalkingTime);

            if (_timer > rndWalktime)
            {
                _moveTargedPos = GetRndMoveDirection();
                _timer = 0.0f;
            }

            if (_isMovingTOCloseToObstacle)
                _moveTargedPos *= -1.5f;            
        }

        public override void ExecutePhysicsUpdateLogic()
        {
            base.ExecutePhysicsUpdateLogic();

            Vector3 raycastOrigin = _baseEnemyBehaviour.transform.position + (_moveTargedPos - _baseEnemyBehaviour.transform.position);

            // todo: (!) maybe exchange following code by more preformance friendly solution; JM (02.11.2023)
            // change Movementdirection if moving to close towards wall
            if (Physics2D.Raycast(_baseEnemyBehaviour.transform.position, _moveTargedPos - _baseEnemyBehaviour.transform.position, _distanceTowardsWall, LayerMask.GetMask("Wall")))
            {
                _isMovingTOCloseToObstacle = true;
            }
            else if (Physics2D.Raycast(_baseEnemyBehaviour.transform.position, _moveTargedPos - _baseEnemyBehaviour.transform.position, _distanceTowardsWall, LayerMask.GetMask("Door")))
            {
                _isMovingTOCloseToObstacle = true;
            }
            else if (Physics2D.Raycast(raycastOrigin.normalized, _moveTargedPos - _baseEnemyBehaviour.transform.position, _distanceTowardsWall, LayerMask.GetMask("Enemy")))
            {
                _isMovingTOCloseToObstacle = true;
            }
            else
                _isMovingTOCloseToObstacle = false;

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
            float rndWalkingRange = Random.Range(_minRandomWalkingRange, _maxRandomWalkingRange);
            return _baseEnemyBehaviour.transform.position + (Vector3)Random.insideUnitCircle * rndWalkingRange;
        }
    }
}