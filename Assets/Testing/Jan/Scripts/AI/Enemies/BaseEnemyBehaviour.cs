using UnityEngine;
using EnumLibrary;
using StateMashine;
using UnityEngine.AI;
using NaughtyAttributes;
using Interactables;
using ScriptableObjects;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseEnemyBehaviour : MonoBehaviour
    {
        // ---------- Fields ----------
        [Header("Needed References")]
        [SerializeField, ReadOnly] private NavMeshAgent _navAgent;
        [SerializeField, ReadOnly] private Animator _animator;
        [SerializeField, ReadOnly] private GameObject _playerObject;
        [SerializeField, ReadOnly] private ConditionPlayerDetectionCheck _condPlayerDetectionCheck;
        [SerializeField, ReadOnly] private ConditionIsInMeleeAttackRangeCheck _condMeleeAttackCheck;    // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)
        [Space(5)]

        [Header("References to Behaviour ScriptableObjects")]
        [SerializeField] private BaseEnemyIdleSO _baseEnemyIdleStateSO;
        [SerializeField] private BaseEnemyAlertSO _baseEnemyAlertStateSO;
        [SerializeField] private BaseEnemyChaseSO _baseEnemyChaseStateSO;
        [SerializeField] private BaseEnemyAttackSO _baseEnemyAttackStateSO;

        public BaseEnemyIdleSO BaseEnemyIdleStateSOInstance { get; set; }
        public BaseEnemyAlertSO BaseEnemyAlertStateSOInstance { get; set; }
        public BaseEnemyChaseSO BaseEnemyChaseStateSOInstance { get; set; }
        public BaseEnemyAttackSO BaseEnemyAttackStateSOInstance { get; set; }
        [Space(5)]

        [Header("Behaviour-related Settings")]
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _chasingSpeed;
        //[SerializeField] private Vector3[] _directionsToCheckToAvoidObstacle;
        [Space(5)]

        [Header("Monitoring of importang values")]
        [SerializeField, ReadOnly] private bool _isPlayerDetected;
        [SerializeField, ReadOnly] private bool _isSomethingAlarmingHappening;
        [SerializeField, ReadOnly] private bool _isInAttackRange;                   // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)
        [SerializeField, ReadOnly] private bool _isCollidingWithObstacle;
        [SerializeField, ReadOnly] private float _noiseRangeOfAlarmingEvent;
        [SerializeField, ReadOnly] private Vector3 _positionOfAlarmingEvent;
        [SerializeField, ReadOnly] private Vector2 _collisionObjectPos;


        // StateMachine-Related Variables
        private EnemyStateMachine _stateMachine;
        private IdleState _idleState;
        private AlertState _alertState;
        private ChaseState _chaseState;
        private AttackState _attackState;


        // --- Properties ---
        // References
        public NavMeshAgent NavAgent { get => _navAgent; private set => _navAgent = value; }
        public Animator Animator { get => _animator; private set => _animator = value; }
        public GameObject PlayerObject { get => _playerObject; protected set => _playerObject = value; }
        public ConditionPlayerDetectionCheck PlayerDetectionCheck { get => _condPlayerDetectionCheck; private set => _condPlayerDetectionCheck = value; }

        // Behaviourrelated-Settings
        public float MovementSpeed { get => _movementSpeed; private set => _movementSpeed = value; }
        public float ChasingSpeed { get => _chasingSpeed; private set => _chasingSpeed = value; }
        //public Vector3[] DirectionsToCheckToAvoidObstacle { get => _directionsToCheckToAvoidObstacle; private set => _directionsToCheckToAvoidObstacle = value; }

        public bool IsPlayerDetected { get => _isPlayerDetected; private set => _isPlayerDetected = value; }
        public bool IsSomethingAlarmingHappening { get => _isSomethingAlarmingHappening; private set => _isSomethingAlarmingHappening = value; }
        public bool IsCollidingWithOtherEnemy { get => _isCollidingWithObstacle; private set => _isCollidingWithObstacle = value; }
        public float NoiseRangeOfAlarmingEvent { get => _noiseRangeOfAlarmingEvent; set => _noiseRangeOfAlarmingEvent = value; }
        public Vector3 PositionOfAlarmingEvent { get => _positionOfAlarmingEvent; set => _positionOfAlarmingEvent = value; }
        public Vector2 CollisionObjectPos { get => _collisionObjectPos; private set => _collisionObjectPos = value; }

        // StateMachine-Related
        public EnemyStateMachine StateMachine { get => _stateMachine; set => _stateMachine = value; }
        public IdleState IdleState { get => _idleState; set => _idleState = value; }
        public AlertState AlertState { get => _alertState; set => _alertState = value; }
        public ChaseState ChaseState { get => _chaseState; set => _chaseState = value; }


        // put that later inside the 'MeleeEnemyBehaviour.cs': ; JM (31.10.2023)
        public ConditionIsInMeleeAttackRangeCheck CondMeleeAttackCheck { get => _condMeleeAttackCheck; private set => _condMeleeAttackCheck = value; }
        public AttackState AttackState { get => _attackState; set => _attackState = value; }
        public bool IsInAttackRange { get => _isInAttackRange; private set => _isInAttackRange = value; }

        // ---------- Methods ----------
        protected void Awake()
        {
            // Autoreferencing
            _navAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _condPlayerDetectionCheck = GetComponent<ConditionPlayerDetectionCheck>();

            CondMeleeAttackCheck = GetComponent<ConditionIsInMeleeAttackRangeCheck>(); // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)

            // Instantiating Copys of the Behaviour-CriptableObjects (so every Enemy has its own beahviour and not all Enemies will share the same reference of an behaviour)
            BaseEnemyIdleStateSOInstance = Instantiate(_baseEnemyIdleStateSO);
            BaseEnemyAlertStateSOInstance = Instantiate(_baseEnemyAlertStateSO);
            BaseEnemyChaseStateSOInstance = Instantiate(_baseEnemyChaseStateSO);
            BaseEnemyAttackStateSOInstance = Instantiate(_baseEnemyAttackStateSO);

            // Variable Initialization
            StateMachine = new EnemyStateMachine();
            IdleState = new IdleState(this, StateMachine);
            AlertState = new AlertState(this, StateMachine);
            ChaseState = new ChaseState(this, StateMachine);
            AttackState = new AttackState(this, StateMachine);
        }

        protected void OnEnable()
        {
            // subscribing to Events
            Interactable.OnDoorKickIn += SetAlarmingEventValues;
            PlayerShoot.OnPlayerShoot += SetAlarmingEventValues;
            _condPlayerDetectionCheck.OnPlayerDetection += SetIsPlayerDetected;

            _condMeleeAttackCheck.OnMeleeAttack += SetIsInAttackRangePlayer;        // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)
        }

        protected void OnDisable()
        {
            // unsubscribing from Events
            Interactable.OnDoorKickIn -= SetAlarmingEventValues;
            PlayerShoot.OnPlayerShoot -= SetAlarmingEventValues;
            _condPlayerDetectionCheck.OnPlayerDetection -= SetIsPlayerDetected;

            _condMeleeAttackCheck.OnMeleeAttack -= SetIsInAttackRangePlayer;        // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)
        }

        // Start is called before the first frame update
        void Start()
        {
            // fixing buggy rotation and transform that happens due to NavMeshAgent on EnemyObjects
            _navAgent.updateRotation = false;
            _navAgent.updateUpAxis = false;

            // initialize the SO-Instances with proper values
            BaseEnemyIdleStateSOInstance.Initialize(this.gameObject, this);
            BaseEnemyAlertStateSOInstance.Initialize(this.gameObject, this);
            BaseEnemyChaseStateSOInstance.Initialize(this.gameObject, this);
            BaseEnemyAttackStateSOInstance.Initialize(this.gameObject, this);

            // initialize Statemachine with initial State
            StateMachine.Initialize(IdleState);
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }
       
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                IsCollidingWithOtherEnemy = true;
                NavAgent.isStopped = true;
                CollisionObjectPos = collision.transform.position;

                Debug.Log($"'<color=lime>{gameObject.name}</color>': collided with a wall (wall position: {CollisionObjectPos});");
            }
        }

        // Update is called once per frame
        void Update()
        {
            StateMachine.CurrentState.FrameUpdate();
        }

        internal void SetIsSomethingAlarmingHappening(bool isSomethinAlarmingHappening)
        {
            IsSomethingAlarmingHappening = isSomethinAlarmingHappening;
        }

        internal void SetIsCollidingWithWall(bool isCollidingWithWall)
        {
            IsCollidingWithOtherEnemy = isCollidingWithWall;
        }

        private void SetIsPlayerDetected(bool isPlayerDetected, GameObject playerObj)
        {
            IsPlayerDetected = isPlayerDetected;
            PlayerObject = playerObj;
        }

        private void SetAlarmingEventValues(bool isSomethinAlarmingHappening, Vector3 positionOfAlarmingEvent, float noiseRangeOfAlarmingEvent)
        {
            // for every Enemy that actually is in the noise range of the AlertEvent set the appropriate values
            Collider2D[] enemieColliders = Physics2D.OverlapCircleAll(positionOfAlarmingEvent, noiseRangeOfAlarmingEvent, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < enemieColliders.Length; i++)
            {
                enemieColliders[i].GetComponent<BaseEnemyBehaviour>().IsSomethingAlarmingHappening = isSomethinAlarmingHappening;
                enemieColliders[i].GetComponent<BaseEnemyBehaviour>().PositionOfAlarmingEvent = positionOfAlarmingEvent;
                enemieColliders[i].GetComponent<BaseEnemyBehaviour>().NoiseRangeOfAlarmingEvent = noiseRangeOfAlarmingEvent;
            }
        }

        /// <summary>
        /// put that later inside the 'MeleeEnemyBehaviour.cs; JM (31.10.2023)
        /// </summary>
        /// <param name="isAttackingPlayer"></param>
        /// <param name="playerObj"></param>
        private void SetIsInAttackRangePlayer(bool isAttackingPlayer, GameObject playerObj)
        {
            IsInAttackRange = isAttackingPlayer;
            PlayerObject = playerObj;
        }

        private void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            // todo: maybe fill with logic for playing specific Animations on specific AnimationEvents if needed; JM (27.10.2023)
            StateMachine.CurrentState.AnimationTriggerEvent(animTriggerType);
        }
    }
}