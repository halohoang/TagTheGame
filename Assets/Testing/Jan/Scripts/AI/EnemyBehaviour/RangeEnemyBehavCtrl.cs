using NaughtyAttributes;
using UnityEngine;

namespace Enemies
{    
    public class RangeEnemyBehavCtrl : NPCBehaviourController
    {
        [Header("Monitoring for Debugging (specific for Range-Enemy-Behaviour)")]
        [SerializeField, ReadOnly] private bool _isPlayerInShootingRange;

        public bool IsPlayerInShootingRange { get => _isPlayerInShootingRange; set => _isPlayerInShootingRange = value; }

        internal void SetIsPlayerInShootingRange(bool isPlayerInShootingRange) 
        {
            IsPlayerInShootingRange = isPlayerInShootingRange;
        }
    }
}