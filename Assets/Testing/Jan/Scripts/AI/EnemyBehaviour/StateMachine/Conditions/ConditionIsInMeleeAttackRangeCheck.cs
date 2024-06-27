using NaughtyAttributes;
using StateMashine;
using UnityEngine;
using UnityEngine.Events;

namespace ArchivedSinceDeprecated
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
            // when Player is alive invoke MeleeAttack Event for informing, that Player is in Attack Range
            if (collision.TryGetComponent(out PlayerHealth playerHealth) && !_isPlayerDead)
            {
                IsInAttackRange = true;
                OnMeleeAttack?.Invoke(IsInAttackRange, collision.gameObject);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            // when Player is alive invoke MeleeAttack Event for informing, that Player is not in Attack Range anymore
            if (collision.TryGetComponent(out PlayerHealth playerHealth) && !_isPlayerDead)
            {
                IsInAttackRange = false;
                OnMeleeAttack?.Invoke(IsInAttackRange, collision.gameObject);
            }
        }
    }
}