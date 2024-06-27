using NaughtyAttributes;
using UnityEngine;

namespace NPCPerception
{
    public class BasePerception : MonoBehaviour
    {
        //--------------------------------------
        // - - - - -  V A R I A B L E S  - - - - 
        //--------------------------------------

        [Header("Perception Settings")]
        #region Tooltip
        [Tooltip("The LayerMask of the object that shall be recognized as target by this enemy.")]
        #endregion
        [SerializeField] private LayerMask _targetDetectionMask;
        #region Tooltip
        [Tooltip("The LayerMask of objects that shall be recognized as obstacle for the vision (like walls, or doors) by this enemy. Like Objects this enemy can't look through.")]
        #endregion
        [SerializeField] private LayerMask _obstructionMask;
        [Space(5)]        

        [Header("Monitoring Values")]
        #region Tooltip
        [Tooltip("The object whis is targeted by this enemy, repsective to the 'Target Detection Mask'.")]
        #endregion
        [SerializeField, ReadOnly] private GameObject _targetObject;
        #region Tooltip
        [Tooltip("Depicts if the player character is currently detected by this enemy.")]
        #endregion
        [SerializeField, ReadOnly] private bool _isTargetDetected = false;
        #region Tooltip
        [Tooltip("Depicts if this enemy is currently dead or not.")]
        #endregion
        [SerializeField, ReadOnly] private bool _isDead;
        #region Tooltip
        [Tooltip("Depicts if the player character is currently dead or not.")]
        #endregion
        [SerializeField, ReadOnly] private bool _isTargetDead;

        // - - - Properties - - -
        protected GameObject TargetObject { get => _targetObject; set => _targetObject = value; }
        protected bool IsTargetDetected { get => _isTargetDetected; set => _isTargetDetected = value; }
        protected bool IsDead { get => _isDead; private set => _isDead = value; }
        protected bool IsTargetDead { get => _isTargetDead; private set => _isTargetDead = value; }
        protected LayerMask TargetDetectionMask { get => _targetDetectionMask; set => _targetDetectionMask = value; }
        protected LayerMask ObstructionMask { get => _obstructionMask; set => _obstructionMask = value; }


        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------

        // - - - Unity Provided Methods - - -
        private void OnEnable()
        {
            PlayerStats.OnPlayerDeath += SetIsTargetDead;
            EnemyStats.OnEnemyDeathEvent += SetIsDead;
        }
        private void OnDisable()
        {
            PlayerStats.OnPlayerDeath -= SetIsTargetDead;
            EnemyStats.OnEnemyDeathEvent -= SetIsDead;
        }

        // - - - Custom Methods - - -
        private void SetIsTargetDead(bool targetDeadStatus)
        {
            _isTargetDead = targetDeadStatus;
        }

        /// <summary>
        /// Sets the bool <see cref="_isDead"/> respective to transmitted parameter 'isDeadStatus' if this gameobject is equal to the transmitted gameObject.
        /// </summary>
        /// <param name="isDeadStatus"></param>
        /// <param name="affectedNPCObject"></param>
        private void SetIsDead(bool isDeadStatus, GameObject affectedNPCObject)
        {
            if (this.gameObject == affectedNPCObject)
                _isDead = isDeadStatus;
        }
    }
}