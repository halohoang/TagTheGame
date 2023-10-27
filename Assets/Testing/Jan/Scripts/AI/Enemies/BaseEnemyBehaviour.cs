using UnityEngine;
using EnumLibrary;
using StateMashine;
using UnityEngine.AI;
using NaughtyAttributes;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseEnemyBehaviour : MonoBehaviour
    {
        // ---------- Fields ----------
        [Header("Needed References")]
        [SerializeField, ReadOnly] private NavMeshAgent _navAgent;
        public NavMeshAgent NavAgent { get => _navAgent; set => _navAgent = value; }

        #region StateMachine Variables
        private EnemyStateMachine _stateMachine;
        private IdleState _idleState;
        private AlertState _alertState;
        private ChaseState _chaseState;
        private MeleeAttackState _meleeAttackState;

        public EnemyStateMachine StateMachine { get => _stateMachine; set => _stateMachine = value; }
        public IdleState IdleState { get => _idleState; set => _idleState = value; }
        public AlertState AlertState { get => _alertState; set => _alertState = value; }
        public ChaseState ChaseState { get => _chaseState; set => _chaseState = value; }
        public MeleeAttackState MeleeAttackState { get => _meleeAttackState; set => _meleeAttackState = value; }
        #endregion

        #region IdleState-Variables

        #endregion
        // ---------- Methods ----------
        private void Awake()
        {
            // Autoreferencing
            _navAgent = GetComponent<NavMeshAgent>();

            // Variable Initialization
            StateMachine = new EnemyStateMachine();
            IdleState = new IdleState(this, StateMachine);
            AlertState = new AlertState(this, StateMachine);
            ChaseState = new ChaseState(this, StateMachine);
            MeleeAttackState = new MeleeAttackState(this, StateMachine);
        }

        // Start is called before the first frame update
        void Start()
        {
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
    }
}