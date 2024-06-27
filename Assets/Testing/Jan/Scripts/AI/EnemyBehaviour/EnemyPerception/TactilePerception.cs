using NaughtyAttributes;
using NPCPerception;
using UnityEngine;
using UnityEngine.Events;

namespace Perception
{
    public class TactilePerception : BasePerception
    {
        #region Events
        //--------------------------------
        // - - - - -  E V E N T S  - - - - 
        //--------------------------------
        public static event UnityAction<bool, GameObject> OnMeleeAttack;
        public static event UnityAction<bool, GameObject> OnCollidingWithOtherEnemy;

        #endregion

        #region Variables
        //--------------------------------------
        // - - - - -  V A R I A B L E S  - - - - 
        //--------------------------------------

        [Header("Monitoring Values")]
        [SerializeField, ReadOnly] private bool _isInAttackRange;
        //[SerializeField, ReadOnly] private bool _isDead;
        //[SerializeField, ReadOnly] private bool _isPlayerDead;
        [SerializeField, ReadOnly] private bool _isCollidingWithOtherEnemy;

        // - - - Properties - - -
        public bool IsInAttackRange { get => _isInAttackRange; private set => _isInAttackRange = value; }

        #endregion

        #region Methods
        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------
        //private void OnEnable()
        //{
        //    PlayerStats.OnPlayerDeath += SetIsPlayerDead;
        //    EnemyStats.OnEnemyDeathEvent += SetIsDead;

        //}
        //private void OnDisable()
        //{
        //    PlayerStats.OnPlayerDeath -= SetIsPlayerDead;
        //    EnemyStats.OnEnemyDeathEvent -= SetIsDead;
        //}

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _isCollidingWithOtherEnemy = true;
                //NavAgent.isStopped = true;
                //CollisionObjectPos = collision.transform.position;

                OnCollidingWithOtherEnemy?.Invoke(_isCollidingWithOtherEnemy, collision.gameObject);

                Debug.Log($"'<color=lime>{gameObject.name}</color>': collided with '{collision.gameObject.name}'");
            }
            else
                _isCollidingWithOtherEnemy = false;
        }

        //private void OnCollisionStay2D(Collision2D collision)
        //{
        //    // not implemented yet
        //}

        //private void OnCollisionExit2D(Collision2D collision)
        //{
        //    // not implemented yet
        //}

        private void OnTriggerStay2D(Collider2D collision)
        {
            // when Player is alive invoke MeleeAttack Event for informing, that Player is in Attack Range
            if (IsTargetDead)
                return;
            else if (collision.gameObject == TargetObject)
            {
                IsInAttackRange = true;
                OnMeleeAttack?.Invoke(IsInAttackRange, collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // when Player is alive invoke MeleeAttack Event for informing, that Player is not in Attack Range anymore
            if (IsTargetDead)
                return;
            else if (collision.gameObject == TargetObject)
            {
                IsInAttackRange = false;
                OnMeleeAttack?.Invoke(IsInAttackRange, collision.gameObject);
            }

            //private void SetIsPlayerDead(bool playerDeadStatus)
            //{
            //    _isPlayerDead = playerDeadStatus;
            //}

            ///// <summary>
            ///// Sets the bool <see cref="_isDead"/> respective to transmitted parameter 'isDeadStatus' if this gameobject is equal to the transmitted gameObject.
            ///// </summary>
            ///// <param name="isDeadStatus"></param>
            ///// <param name="affectedNPCObject"></param>
            //private void SetIsDead(bool isDeadStatus, GameObject affectedNPCObject)
            //{
            //    if (this.gameObject == affectedNPCObject)
            //        _isDead = isDeadStatus;
            //}
            #endregion
        }
    }
}