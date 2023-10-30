using UnityEngine;
using EnumLibrary;
using StateMashine;
using UnityEngine.AI;
using NaughtyAttributes;
using Interactables;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseEnemyBehaviour : MonoBehaviour
    {
        // ---------- Fields ----------
        [Header("Needed References")]
        [SerializeField, ReadOnly] private NavMeshAgent _navAgent;
        public NavMeshAgent NavAgent { get => _navAgent; set => _navAgent = value; }
        [Space(5)]
        
        [Header("Monitoring for Debugging")]
        [SerializeField, ReadOnly] private bool _isPlayerDetected;  

        
        // StateMachine-Related Variables
        private EnemyStateMachine _stateMachine;
        private IdleState _idleState;
        //private AlertState _alertState;
        private ChaseState _chaseState;
        private MeleeAttackState _meleeAttackState;


        // Properties
        public bool IsPlayerDetected { get => _isPlayerDetected; private set => _isPlayerDetected = value; }
        public EnemyStateMachine StateMachine { get => _stateMachine; set => _stateMachine = value; }
        public IdleState IdleState { get => _idleState; set => _idleState = value; }
        //public AlertState AlertState { get => _alertState; set => _alertState = value; }
        public ChaseState ChaseState { get => _chaseState; set => _chaseState = value; }
        public MeleeAttackState MeleeAttackState { get => _meleeAttackState; set => _meleeAttackState = value; }
        

        // ---------- Methods ----------
        private void Awake()
        {
            // Autoreferencing
            _navAgent = GetComponent<NavMeshAgent>();

            // Variable Initialization
            StateMachine = new EnemyStateMachine();
            IdleState = new IdleState(this, StateMachine);
            //AlertState = new AlertState(this, StateMachine);
            ChaseState = new ChaseState(this, StateMachine);
            MeleeAttackState = new MeleeAttackState(this, StateMachine);
        }

        private void OnEnable()
        {
            // subscribing to Events
            Interactable.OnDoorKickIn += FaceAgentTowardsDoor;
        }

        private void OnDisable()
        {
            // unsubscribing from Events
            Interactable.OnDoorKickIn -= FaceAgentTowardsDoor;
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

        internal void SetIsPlayerDetected(bool isPlayerDetected) 
        {
            IsPlayerDetected = isPlayerDetected;
        }
    }
}