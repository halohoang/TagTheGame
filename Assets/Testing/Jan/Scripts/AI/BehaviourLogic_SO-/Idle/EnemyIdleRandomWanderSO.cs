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
        [SerializeField, Range(1.0f, 5.0f)] private float _distanceTowardsWall = 2.0f;

        private Rigidbody2D _thisEnemyRB2D;
        private Vector3 _walkTargedPos;
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

            _thisEnemyRB2D = _baseEnemyBehaviour.gameObject.GetComponent<Rigidbody2D>();

            _isMoving = true;

            _walkTargedPos = GetRndMoveDirection();
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
            Vector2 direction = (_walkTargedPos - _baseEnemyBehaviour.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _thisEnemyRB2D.rotation = angle;
            #region altern rotation for facing direction
            //// a alternative way to manage the facing direction by applying the rotation to the transform instead of to the rigidbody
            //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
            //_baseEnemyBehaviour.transform.rotation = quart;
            #endregion

            // actual walking
            _baseEnemyBehaviour.NavAgent.SetDestination(_walkTargedPos);

            float rndWalktime = Random.Range(_minRandomWalkingTime, _maxRandomWalkingTime);

            if (_timer > rndWalktime)
            {
                _walkTargedPos = GetRndMoveDirection();
                _timer = 0.0f;
            }

            if (_isMovingTOCloseToObstacle)
                _walkTargedPos *= -1.5f;            
        }

        public override void ExecutePhysicsUpdateLogic()
        {
            base.ExecutePhysicsUpdateLogic();

            Vector3 raycastOrigin = _baseEnemyBehaviour.transform.position + (_walkTargedPos - _baseEnemyBehaviour.transform.position);

            // todo: (!) maybe exchange following code by more preformance friendly solution; JM (02.11.2023)
            // change Movementdirection if moving to close towards wall
            if (Physics2D.Raycast(_baseEnemyBehaviour.transform.position, _walkTargedPos - _baseEnemyBehaviour.transform.position, _distanceTowardsWall, LayerMask.GetMask("Wall")))
            {
                _isMovingTOCloseToObstacle = true;
            }
            else if (Physics2D.Raycast(_baseEnemyBehaviour.transform.position, _walkTargedPos - _baseEnemyBehaviour.transform.position, _distanceTowardsWall, LayerMask.GetMask("Door")))
            {
                _isMovingTOCloseToObstacle = true;
            }
            else if (Physics2D.Raycast(raycastOrigin.normalized, _walkTargedPos - _baseEnemyBehaviour.transform.position, _distanceTowardsWall, LayerMask.GetMask("Enemy")))
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