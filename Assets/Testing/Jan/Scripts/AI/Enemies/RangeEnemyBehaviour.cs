using NaughtyAttributes;
using UnityEngine;

namespace Enemies
{    
    public class RangeEnemyBehaviour : BaseEnemyBehaviour
    {
        // will be depicted in the Inspector under 'Monitoring for Debugging' Header
        [SerializeField, ReadOnly] private bool _isPlayerInShootingRange;

        public bool IsPlayerInShootingRange { get => _isPlayerInShootingRange; set => _isPlayerInShootingRange = value; }

        internal void SetIsPlayerInShootingRange(bool isPlayerInShootingRange) 
        {
            IsPlayerInShootingRange = isPlayerInShootingRange;
        }
    }
}