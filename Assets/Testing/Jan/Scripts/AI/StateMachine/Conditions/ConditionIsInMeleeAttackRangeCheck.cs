using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace StateMashine
{
    public class ConditionIsInMeleeAttackRangeCheck : BaseConditionCheck
    {
        // ---------- Fields ----------
        public event UnityAction<bool, GameObject> OnMeleeAttack;

        [SerializeField, ReadOnly] private bool _isInAttackRange;

        // --- Properties ---
        public bool IsInAttackRange { get => _isInAttackRange; private set => _isInAttackRange = value; }


        // ---------- Methods ----------
        private void OnTriggerStay2D(Collider2D collision)
        {
            // Dealing Damage to Player when Player enters Trigger-Zone around Enemy        
            if (collision.TryGetComponent(out PlayerHealth playerHealth))
            {
                IsInAttackRange = true;
                OnMeleeAttack?.Invoke(IsInAttackRange, collision.gameObject);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerHealth playerHealth))
            {
                IsInAttackRange = false;
                OnMeleeAttack?.Invoke(IsInAttackRange, collision.gameObject);
            }
        }
    }
}