using UnityEngine;
using EnumLibrary;
using StateMashine;
using UnityEngine.AI;
using NaughtyAttributes;
using Interactables;
using System;

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
        //[SerializeField, ReadOnly] private ConditionIsInMeleeAttackRangeCheck _condMeleeAttackCheck;    // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)
        [Space(5)]

        [Header("Behaviour-related Settings")]
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _chasingSpeed;

        [Header("Monitoring for Debugging")]
        [SerializeField, ReadOnly] private bool _isPlayerDetected;
        //[SerializeField, ReadOnly] private bool _isInAttackRange;   // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)



        // StateMachine-Related Variables
        private EnemyStateMachine _stateMachine;
        private IdleState _idleState;
        //private AlertState _alertState;
        private ChaseState _chaseState;


        // Properties
        public NavMeshAgent NavAgent { get => _navAgent; private set => _navAgent = value; }
        public Animator Animator { get => _animator; private set => _animator = value; }
        public GameObject PlayerObject { get => _playerObject; protected set => _playerObject = value; }
        public ConditionPlayerDetectionCheck PlayerDetectionCheck { get => _condPlayerDetectionCheck; private set => _condPlayerDetectionCheck = value; }        

        public float MovementSpeed { get => _movementSpeed; private set => _movementSpeed = value; }
        public float ChasingSpeed { get => _chasingSpeed; private set => _chasingSpeed = value; }
        public bool IsPlayerDetected { get => _isPlayerDetected; private set => _isPlayerDetected = value; }        

        public EnemyStateMachine StateMachine { get => _stateMachine; set => _stateMachine = value; }
        public IdleState IdleState { get => _idleState; set => _idleState = value; }
        //public AlertState AlertState { get => _alertState; set => _alertState = value; }
        public ChaseState ChaseState { get => _chaseState; set => _chaseState = value; }        


        // put that later inside the 'MeleeEnemyBehaviour.cs': ; JM (31.10.2023)
        //public ConditionIsInMeleeAttackRangeCheck CondMeleeAttackCheck { get => _condMeleeAttackCheck; private set => _condMeleeAttackCheck = value; }
        //public bool IsInAttackRange { get => _isInAttackRange; private set => _isInAttackRange = value; }

        // ---------- Methods ----------
        protected void Awake()
        {
            // Autoreferencing
            _navAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _condPlayerDetectionCheck = GetComponent<ConditionPlayerDetectionCheck>();

            //CondMeleeAttackCheck = GetComponent<ConditionIsInMeleeAttackRangeCheck>(); // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)

            // Variable Initialization
            StateMachine = new EnemyStateMachine();
            IdleState = new IdleState(this, StateMachine);
            //AlertState = new AlertState(this, StateMachine);
            ChaseState = new ChaseState(this, StateMachine);
        }

        protected void OnEnable()
        {
            // subscribing to Events
            Interactable.OnDoorKickIn += FaceAgentTowardsDoor;
            _condPlayerDetectionCheck.OnPlayerDetection += SetIsPlayerDetected;

            //_condMeleeAttackCheck.OnMeleeAttack += SetIsInAttackRangePlayer;        // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)
        }

        protected void OnDisable()
        {
            // unsubscribing from Events
            Interactable.OnDoorKickIn -= FaceAgentTowardsDoor;
            _condPlayerDetectionCheck.OnPlayerDetection -= SetIsPlayerDetected;

            //_condMeleeAttackCheck.OnMeleeAttack -= SetIsInAttackRangePlayer;        // put that later inside the 'MeleeEnemyBehaviour.cs'; JM (31.10.2023)
        }

        // Start is called before the first frame update
        void Start()
        {
            // fixing buggy rotation and transform that happens due to NavMeshAgent on EnemyObjects
            _navAgent.updateRotation = false;
            _navAgent.updateUpAxis = false;

            StateMachine.Initialize(IdleState);
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        // Update is called once per frame
        void Update()
        {
            StateMachine.CurrentState.FrameUpdate();
        }

        private void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            // todo: maybe fill with logic for playing specific Animations on specific AnimationEvents if needed; JM (27.10.2023)
            StateMachine.CurrentState.AnimationTriggerEvent(animTriggerType);
        }

        // todo: maybe put that logic into the Alert-State; Jan (30.10.2023)
        /// <summary>
        /// Sets the Facing direction of the enemy-object towards the door that was kicked in if the enemy-object is within the kick-in-noise-range of the kicked in door
        /// </summary>
        /// <param name="doorPosition"></param>
        /// <param name="doorKickInNoiseRange"></param>
        private void FaceAgentTowardsDoor(Vector3 doorPosition, float doorKickInNoiseRange)
        {
            // Rotate the Enemy-Object so it's facing the Kicked in Door Object when Door was kicked in 
            Collider2D[] enemieColliders = Physics2D.OverlapCircleAll(doorPosition, doorKickInNoiseRange, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < enemieColliders.Length; i++)
            {
                enemieColliders[i].gameObject.transform.right = doorPosition - enemieColliders[i].gameObject.transform.position;

                #region old code
                //alternate solution (does not work properly)
                //Vector2 directionToDoor = (doorPosition - transform.position).normalized;
                //float alphaAngle = Mathf.Atan2(directionToDoor.x, directionToDoor.y);
                //float angleToRotate = (Mathf.PI - alphaAngle) * Mathf.Rad2Deg;
                //Quaternion quart = Quaternion.AngleAxis(angleToRotate, Vector3.forward);

                //enemieColliders[i].gameObject.transform.rotation = quart;
                //Debug.Log($"<color=orange> {enemieColliders[i].name} was rotated by {angleToRotate}° on its Z-Axis </color>");


                //alternate solution (does not work properly)
                //// calculate rotation angle (does not work as intended yet tho); JM (18.10.2023)
                //Vector2 lookDirection = (doorPosition - transform.position).normalized;
                //float angle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg - _rotationModifier;
                //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);

                //// rotat enemy-object
                //enemieColliders[i].gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, quart, 360);
                #endregion
            }
        }

        private void SetIsPlayerDetected(bool isPlayerDetected, GameObject playerObj) 
        {
            IsPlayerDetected = isPlayerDetected;
            PlayerObject = playerObj;
        }

        /// <summary>
        /// put that later inside the 'MeleeEnemyBehaviour.cs; JM (31.10.2023)
        /// </summary>
        /// <param name="isAttackingPlayer"></param>
        /// <param name="playerObj"></param>
        //private void SetIsInAttackRangePlayer(bool isAttackingPlayer, GameObject playerObj)
        //{
        //    IsInAttackRange = isAttackingPlayer;
        //    PlayerObject = playerObj;
        //}
    }
}