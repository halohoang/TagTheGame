using NaughtyAttributes;
using UnityEngine;

namespace StateMashine
{
    public class BaseConditionCheck : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected bool _isPlayerDead;

        private void OnEnable()
        {
            PlayerHealth.OnPlayerDeath += SetIsPlayerDead;
        }

        private void OnDisable()
        {
            PlayerHealth.OnPlayerDeath -= SetIsPlayerDead;
        }

        public virtual bool ConditionCheck()
        {
            return false;
        }

        private void SetIsPlayerDead(bool isPlayerDead)
        {
            _isPlayerDead = isPlayerDead;
        }
    }
}