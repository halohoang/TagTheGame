using ArchivedSinceDeprecated;
using EnumLibrary;
using NaughtyAttributes;
using Player;
using Perception;
using ScriptableObjects;
using StateMashine;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent), typeof(NPCPerception))]
    [DisallowMultipleComponent]
    public class NPCBehaviourController : MonoBehaviour
    {
        #region Variables
        //--------------------------------------
        // - - - - -  V A R I A B L E S  - - - - 
        //--------------------------------------

        [Header("Needed References")]
        [SerializeField, ReadOnly] private NavMeshAgent _navAgent;
        [SerializeField, ReadOnly] private Animator _animator;
        [SerializeField, ReadOnly] private GameObject _targetObject;
        [SerializeField, ReadOnly] private ConditionPlayerDetectionCheck _condPlayerDetectionCheck;     // Todo: obsolete with new perception System, remove as soon as completely changed to new system
        [SerializeField, ReadOnly] private ConditionIsInMeleeAttackRangeCheck _condMeleeAttackCheck;    // Todo: obsolete with new perception System, remove as soon as completely changed to new system
        [Space(5)]

        [Header("References to Behaviour ScriptableObjects")] // Todo: think about bool -> _isPatrolingEnemy that enables/disables patroling state; JM (04.11.24)
        [SerializeField] private BaseEnemyIdleSO _baseIdleStateSO;
        [SerializeField] private BaseEnemyMovementSO _baseMovemenStateSO;
        [SerializeField] private BaseEnemyAlertSO _baseAlertStateSO;
        [SerializeField] private BaseEnemyChaseSO _baseChaseStateSO;
        [SerializeField] private BaseEnemyAttackSO _baseAttackStateSO;

        // properties for Base behaviour scriptable objects to store copies of the respective above referenced ones
        internal bool IsStandingIdle { get => _isStandingIdleNPC; private set => _isStandingIdleNPC = value; }
        public BaseEnemyIdleSO BaseEnemyIdleStateSOInstance { get; set; }
        public BaseEnemyMovementSO BaseEnemyMovementStateSOInstance { get; set; }
        public BaseEnemyAlertSO BaseEnemyAlertStateSOInstance { get; set; }
        public BaseEnemyChaseSO BaseEnemyChaseStateSOInstance { get; set; }
        public BaseEnemyAttackSO BaseEnemyAttackStateSOInstance { get; set; }
        [Space(5)]

        [Header("Waypoints for patroling NPC"), ShowIf("_isThisPatrolingNPC")]
        [SerializeField] private List<GameObject> _wayPoints;
        [Space(5)]

        [Header("Behaviour-related Settings")]
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _chasingSpeed;
        //[SerializeField] private Vector3[] _directionsToCheckToAvoidObstacle;
        [Space(5)]

        [Header("Monitoring Values")]
        [SerializeField, ReadOnly] private bool _isStandingIdleNPC;
        [SerializeField, ReadOnly] private bool _isThisPatrolingNPC;
        [SerializeField, ReadOnly] private bool _isThisRandomWanderNPC;
        [SerializeField, ReadOnly] private bool _isThisNPCDead;
        [SerializeField, ReadOnly] private bool _isTargetDead;
        [SerializeField, ReadOnly] private bool _isTargetDetected;
        [SerializeField, ReadOnly] private bool _isSomethingAlarmingHappening;
        [SerializeField, ReadOnly] private bool _isInAttackRange;                   // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)
        [SerializeField, ReadOnly] private bool _isCollidingWithObject;
        [SerializeField, ReadOnly] private float _noiseRangeOfAlarmingEvent;
        [SerializeField, ReadOnly] private Vector3 _positionOfAlarmingEvent;
        [SerializeField, ReadOnly] private Vector2 _collisionObjectPos;
        [SerializeField, ReadOnly] private Vector3 _lastKnownTargetPos;
        [SerializeField, ReadOnly] private string _currentActiveBehaviourState;


        // StateMachine-Related Variables
        private NPCStateMachine _stateMachine;
        private IdleState _idleState;
        private MovementState _movementState;
        private AlertState _alertState;
        private ChaseState _chaseState;
        private AttackState _attackState;


        // - - - Properties - - -         

        // References
        public NavMeshAgent NavAgent { get => _navAgent; private set => _navAgent = value; }
        public Animator Animator { get => _animator; private set => _animator = value; }
        public GameObject TargetObject { get => _targetObject; protected set => _targetObject = value; }
        public ConditionPlayerDetectionCheck PlayerDetectionCheck { get => _condPlayerDetectionCheck; private set => _condPlayerDetectionCheck = value; }

        // Behaviourrelated-Settings
        public float MovementSpeed { get => _movementSpeed; private set => _movementSpeed = value; }
        public float ChasingSpeed { get => _chasingSpeed; private set => _chasingSpeed = value; }
        public List<GameObject> WayPoints { get => _wayPoints; private set => _wayPoints = value; }
        //public Vector3[] DirectionsToCheckToAvoidObstacle { get => _directionsToCheckToAvoidObstacle; private set => _directionsToCheckToAvoidObstacle = value; }

        public bool IsTargetDead { get => _isTargetDead; private set => _isTargetDead = value; }
        public bool IsTargetDetected { get => _isTargetDetected; private set => _isTargetDetected = value; }
        public bool IsSomethingAlarmingHappening { get => _isSomethingAlarmingHappening; private set => _isSomethingAlarmingHappening = value; }
        public bool IsCollidingWithObject { get => _isCollidingWithObject; private set => _isCollidingWithObject = value; }
        public float NoiseRangeOfAlarmingEvent { get => _noiseRangeOfAlarmingEvent; private set => _noiseRangeOfAlarmingEvent = value; }
        public Vector3 PositionOfAlarmingEvent { get => _positionOfAlarmingEvent; private set => _positionOfAlarmingEvent = value; }
        public Vector2 CollisionObjectPos { get => _collisionObjectPos; private set => _collisionObjectPos = value; }
        public Vector3 LastKnowntargetPos { get => _lastKnownTargetPos; private set => _lastKnownTargetPos = value; }

        // StateMachine-Related
        public NPCStateMachine StateMachine { get => _stateMachine; set => _stateMachine = value; }
        public IdleState IdleState { get => _idleState; set => _idleState = value; }
        public MovementState MovementState { get => _movementState; set => _movementState = value; }
        public AlertState AlertState { get => _alertState; set => _alertState = value; }
        public ChaseState ChaseState { get => _chaseState; set => _chaseState = value; }
        public AttackState AttackState { get => _attackState; set => _attackState = value; }


        // put that later inside the 'MeleeEnemyBehaviour.cs': ; JM (31.10.2023)
        public ConditionIsInMeleeAttackRangeCheck CondMeleeAttackCheck { get => _condMeleeAttackCheck; private set => _condMeleeAttackCheck = value; }
        public bool IsInAttackRange { get => _isInAttackRange; private set => _isInAttackRange = value; }
        #endregion


        #region Methods
        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------

        #region Unity Methods
        // Unity provided Methods
        // 
        private void OnValidate()
        {
            // Check if there is a reference Set to the _baseMovementStateSO and set bollians respectively
            if (_baseMovemenStateSO == null)
            {
                _isThisPatrolingNPC = false;
                _isThisRandomWanderNPC = false;
                _isStandingIdleNPC = true;
            }
            else if (_baseMovemenStateSO is MeleeEnemyMvmntPatrolSO)
            {
                _isThisPatrolingNPC = true;
                _isThisRandomWanderNPC = false;
            }
            else if (_baseMovemenStateSO is MeleeEnemyMvmntRandomWanderSO || _baseMovemenStateSO is RangeEnemyMvmntRandomWanderSO)
            {
                _isThisPatrolingNPC = false;
                _isThisRandomWanderNPC = true;
            }
        }

        protected void Awake()
        {
            // Autoreferencing
            _navAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _condPlayerDetectionCheck = GetComponent<ConditionPlayerDetectionCheck>();

            CondMeleeAttackCheck = GetComponent<ConditionIsInMeleeAttackRangeCheck>(); // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)

            // Instantiating Copys of the Behaviour-ScriptableObjects (so every Enemy has its own beahviour and not all Enemies will share the same reference of an behaviour)
            BaseEnemyIdleStateSOInstance = Instantiate(_baseIdleStateSO);
            BaseEnemyMovementStateSOInstance = Instantiate(_baseMovemenStateSO);
            BaseEnemyAlertStateSOInstance = Instantiate(_baseAlertStateSO);
            BaseEnemyChaseStateSOInstance = Instantiate(_baseChaseStateSO);
            BaseEnemyAttackStateSOInstance = Instantiate(_baseAttackStateSO);

            // Variable Initialization
            StateMachine = new NPCStateMachine();
            IdleState = new IdleState(this, StateMachine);
            MovementState = new MovementState(this, StateMachine);
            AlertState = new AlertState(this, StateMachine);
            ChaseState = new ChaseState(this, StateMachine);
            AttackState = new AttackState(this, StateMachine);
        }

        protected void OnEnable()
        {
            // subscribing to Events
            #region old System
            //PlayerHealth.OnPlayerDeath += SetIsPlayerDead;
            ////Interactable_Door.OnDoorKickIn += SetAlarmingEventValues;
            //PlayerShoot.OnPlayerShoot += SetAlarmingEventValues;
            //_condPlayerDetectionCheck.OnPlayerDetection += SetIsPlayerDetected;
            //_condMeleeAttackCheck.OnMeleeAttack += SetIsInAttackRangePlayer;        // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)
            #endregion

            #region new System (26.06.)
            // new System (26.06.24)
            PlayerStats.OnPlayerDeath += SetIsTargetDead;
            EnemyStats.OnEnemyDeathEvent += SetIsThisNPCDead;

            NPCPerception.OnTargetDetection += SetIsTargetDetected;
            NPCPerception.OnSomethingAlarmingIsHappening += SetAlarmingEventValues;
            NPCPerception.OnCollidingWithOtherObject += SetIsCollidingWithOtherEnemy;
            NPCPerception.OnInMeleeAttackRange += SetIsInAttackRangePlayer;

            //StateMachine related
            NPCStateMachine.OnStateTransition += SetCurrentActiveState;
            #endregion
        }

        protected void OnDisable()
        {
            // unsubscribing from Events
            #region old system
            //PlayerHealth.OnPlayerDeath -= SetIsPlayerDead;
            ////Interactable_Door.OnDoorKickIn -= SetAlarmingEventValues;
            //PlayerShoot.OnPlayerShoot -= SetAlarmingEventValues;
            //_condPlayerDetectionCheck.OnPlayerDetection -= SetIsPlayerDetected;
            //_condMeleeAttackCheck.OnMeleeAttack -= SetIsInAttackRangePlayer;        // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)
            #endregion

            #region new system (26.06.)
            // new System (26.06.24)
            PlayerStats.OnPlayerDeath -= SetIsTargetDead;
            EnemyStats.OnEnemyDeathEvent -= SetIsThisNPCDead;

            NPCPerception.OnTargetDetection -= SetIsTargetDetected;
            NPCPerception.OnSomethingAlarmingIsHappening -= SetAlarmingEventValues;
            NPCPerception.OnCollidingWithOtherObject -= SetIsCollidingWithOtherEnemy;
            NPCPerception.OnInMeleeAttackRange -= SetIsInAttackRangePlayer;

            //StateMachine related
            NPCStateMachine.OnStateTransition -= SetCurrentActiveState;
            #endregion
        }

        private void Start()
        {
            // fixing buggy rotation and transform that happens due to NavMeshAgent on EnemyObjects
            _navAgent.updateRotation = false;
            _navAgent.updateUpAxis = false;

            // initialize the SO-Instances with proper values
            BaseEnemyIdleStateSOInstance.Initialize(this.gameObject, this);
            BaseEnemyMovementStateSOInstance.Initialize(this.gameObject, this);
            BaseEnemyAlertStateSOInstance.Initialize(this.gameObject, this);
            BaseEnemyChaseStateSOInstance.Initialize(this.gameObject, this);
            BaseEnemyAttackStateSOInstance.Initialize(this.gameObject, this);
            // todo: JM; rework Architecture since the Initializiation like that will allways put the specific EnemyBehaviourCtrl.cs into a NPCBahaviourController (because of Polymorphism) regardless if MeleeEnemyBehaviour or RangeEnemyBehaviour. Accordingly simple Overloading the Initialization() to take Melee/RangeEnemyBehaviour woun't work since nonetheless the first Overload(NPCBahaviourController will be used simply becaus it's possible) therefore outsourcing the MeleeAttack/chase Logic to MeleeEnemyBehaviour can't be called in the specific BehaviourScriptable Objects. -> find a Solution for this Problem(!). until then stick to the existing appraoch by handling all Attack/Chase-Logic and Queries (if 'isInAttackRange' etc) in the NPCBahaviourController.cs even if it's no nice architecture and makes the MeleeEnemyBehaviour/RangeEnemyBehaviour.cs actually useless a the moment; (JM 10.11.2023)            

            // Setup behaviour-state-settings
            // additional secure check if there is a reference Set to the _baseMovementStateSO and set bollians respectively
            if (_baseMovemenStateSO == null)
            {
                _isThisPatrolingNPC = false;
                _isThisRandomWanderNPC = false;
                _isStandingIdleNPC = true;
            }
            else if (_baseMovemenStateSO is MeleeEnemyMvmntPatrolSO)
            {
                _isThisPatrolingNPC = true;
                _isThisRandomWanderNPC = false;
            }
            else if (_baseMovemenStateSO is MeleeEnemyMvmntRandomWanderSO || _baseMovemenStateSO is RangeEnemyMvmntRandomWanderSO)
            {
                _isThisPatrolingNPC = false;
                _isThisRandomWanderNPC = true;
            }

            // initialize Statemachine with initial State
            if (_isThisRandomWanderNPC || _isThisPatrolingNPC)
                StateMachine.Initialize(MovementState);
            else
                StateMachine.Initialize(IdleState);
        }

        private void FixedUpdate()
        {
            if (_isThisNPCDead)
                return;

            // reset to Idle State if Player was killed
            if (_isTargetDead && StateMachine.CurrentState != IdleState)
                StateMachine.Transition(IdleState);

            StateMachine.CurrentState.PhysicsUpdate();
        }

        #region Testing
        // todo: move this logic once perception-system is fully implemented and out of testphase; JM (18.07.24)
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _isCollidingWithObject = true;
                //NavAgent.isStopped = true;
                //CollisionObjectPos = collision.transform.position;

                Debug.Log($"'<color=lime>{gameObject.name}</color>': collided with '{collision.gameObject.name}' (wall position: '{_collisionObjectPos}');");
            }
            else
                _isCollidingWithObject = false;
        }
        #endregion

        private void Update()
        {
            if (_isThisNPCDead)
                return;

            // reset to Idle State if Player was killed
            if (_isTargetDead && StateMachine.CurrentState != IdleState)
                StateMachine.Transition(IdleState);

            StateMachine.CurrentState.FrameUpdate();
        }
        #endregion

        #region Custom Methods
        // Custom Methods        

        internal void SetIsCollidingWithOtherEnemy(bool isCollidingWithWall, GameObject otherObj)
        {
            _isCollidingWithObject = isCollidingWithWall;
            _collisionObjectPos = otherObj.transform.position;
        }

        internal void SetIsCollidingWithObject(bool isCollidingWithObject)
        {
            _isCollidingWithObject = isCollidingWithObject;
        }

        /// <summary>
        /// Sets the Value for '<see cref="LastKnowntargetPos"/>'.
        /// </summary>
        /// <param name="lastKnownPlayerPosition"></param>
        internal void CacheLastKnownTargetPosition()
        {
            _lastKnownTargetPos = _targetObject.transform.position;
        }

        internal void SetIsSomethingAlarmingHappening(bool isSomethinAlarmingHappening)
        {
            _isSomethingAlarmingHappening = isSomethinAlarmingHappening;
        }

        /// <summary>
        /// Sets the values defining the alarming status of this enemy. (<see cref="IsSomethingAlarmingHappening"/>, <see cref="PositionOfAlarmingEvent"/>)
        /// </summary>
        /// <param name="isSomethinAlarmingHappening"></param>
        /// <param name="positionOfAlarmingEvent"></param>
        internal void SetAlarmingEventValues(bool isSomethinAlarmingHappening, Vector3 positionOfAlarmingEvent)
        {
            _isSomethingAlarmingHappening = isSomethinAlarmingHappening;
            _positionOfAlarmingEvent = positionOfAlarmingEvent;
        }

        /// <summary>
        /// rework this solution since every enemy will execute this and therefore everey enemy will set -> 
        /// </summary>
        /// <param name="isSomethinAlarmingHappening"></param>
        /// <param name="positionOfAlarmingEvent"></param>
        /// <param name="noiseRangeOfAlarmingEvent"></param>
        private void SetAlarmingEventValues(bool isSomethinAlarmingHappening, Vector3 positionOfAlarmingEvent, float noiseRangeOfAlarmingEvent)
        {
            NPCBehaviourController enemBehav = new NPCBehaviourController();

            // for every Enemy that actually is in the noise range of the AlertEvent set the appropriate values
            Collider2D[] enemieColliders = Physics2D.OverlapCircleAll(positionOfAlarmingEvent, noiseRangeOfAlarmingEvent, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < enemieColliders.Length; i++)
            {
                enemBehav = enemieColliders[i].GetComponent<NPCBehaviourController>();
                enemBehav.IsSomethingAlarmingHappening = isSomethinAlarmingHappening;
                enemBehav.PositionOfAlarmingEvent = positionOfAlarmingEvent;
                enemBehav.NoiseRangeOfAlarmingEvent = noiseRangeOfAlarmingEvent;
            }
        }

        private void SetIsTargetDetected(bool isTargetDetected, GameObject targetObj)
        {
            _isTargetDetected = isTargetDetected;
            _targetObject = targetObj;
            CacheLastKnownTargetPosition();
        }

        /// <summary>
        /// Sets the bool <see cref="IsTargetDead"/>
        /// </summary>
        private void SetIsTargetDead(bool isTargetDead)
        {
            _isTargetDead = isTargetDead;
            _isTargetDetected = false;
            _isInAttackRange = false;
        }

        /// <summary>
        /// Sets the bool <see cref="_isThisNPCDead"/> if this gameobject is the object that run out of health.
        /// </summary>
        /// <param name="isDeadStatus"></param>
        /// <param name="deadObject"></param>
        private void SetIsThisNPCDead(bool isDeadStatus, GameObject deadObject)
        {
            if (this.gameObject == deadObject)
                _isThisNPCDead = isDeadStatus;
        }

        /// <summary>
        /// Sets the string <see cref="_currentActiveBehaviourState"/> according to the transmitted parameter. Only for monitoring/debugging reasons.
        /// </summary>
        /// <param name="nameOfCurrentState"></param>
        private void SetCurrentActiveState(string nameOfCurrentState)
        {
            _currentActiveBehaviourState = nameOfCurrentState;
        }

        /// <summary>
        /// Disables all Behaviour relevant scripts running on the 'this.gameObject'
        /// </summary>
        private void DisableAIBehaviour()
        {
            _condPlayerDetectionCheck.enabled = false;
            //_condMeleeAttackCheck.enabled = false;
            this.enabled = false;
        }

        /// <summary>
        /// put that later inside the 'MeleeEnemyBehaviour.cs; JM (31.10.2023)
        /// </summary>
        /// <param name="isAttackingPlayer"></param>
        /// <param name="playerObj"></param>
        private void SetIsInAttackRangePlayer(bool isAttackingPlayer, GameObject playerObj)
        {
            _isInAttackRange = isAttackingPlayer;
            _targetObject = playerObj;
        }

        /// <summary>
        /// Sets the <see cref="IsStandingIdle"/> field for this NPC.
        /// </summary>
        /// <param name="isStandingIdle"></param>
        internal void SetIsStandingIdle(bool isStandingIdle)
        {
            IsStandingIdle = IsStandingIdle;
        }

        private void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            // todo: maybe fill with logic for playing specific Animations on specific AnimationEvents if needed; JM (27.10.2023)
            StateMachine.CurrentState.AnimationTriggerEvent(animTriggerType);
        }
        #endregion

        #endregion
    }
}