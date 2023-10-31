using UnityEngine;
using StateMashine;
using NaughtyAttributes;

namespace Enemies
{
    public class MeleeEnemyBehaviour : BaseEnemyBehaviour
    {
        // todo: find a solution for a proper defining of the MeleeEnemyBehaviour-Class and seperating the specific logic form it sparent class, make proper usage of polymorphism
        // ---------- Fields ----------

        [Header("References specific for (Melee-Enemy-Behaviour)")]
        [SerializeField, ReadOnly] private ConditionIsInMeleeAttackRangeCheck _condMeleeAttackCheck;
        [Space(5)]

        [Header("Monitoring for Debugging (specific for Melee-Enemy-Behaviour)")]
        [SerializeField, ReadOnly] private bool _isInAttackRange;


        // StateMachine-Related Variables
        private MeleeAttackState _meleeAttackState;

        // --- Properties ---
        public ConditionIsInMeleeAttackRangeCheck CondMeleeAttackCheck { get => _condMeleeAttackCheck; private set => _condMeleeAttackCheck = value; }

        public bool IsInAttackRange { get => _isInAttackRange; private set => _isInAttackRange = value; }

        public MeleeAttackState MeleeAttackState { get => _meleeAttackState; set => _meleeAttackState = value; }

        // ---------- Methods ----------
        new private void Awake()
        {
            base.Awake();

            MeleeAttackState = new MeleeAttackState(this, StateMachine);

            CondMeleeAttackCheck = GetComponent<ConditionIsInMeleeAttackRangeCheck>();
        }

        new private void OnEnable()
        {
            base.OnEnable();

            _condMeleeAttackCheck.OnMeleeAttack += SetIsInAttackRangePlayer;
        }

        new private void OnDisable()
        {
            base.OnDisable();

            _condMeleeAttackCheck.OnMeleeAttack -= SetIsInAttackRangePlayer;
        }

        private void SetIsInAttackRangePlayer(bool isAttackingPlayer, GameObject playerObj)
        {
            IsInAttackRange = isAttackingPlayer;
            PlayerObject = playerObj;
        }
    }
}