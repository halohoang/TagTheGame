using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace ArchivedSinceDeprecated
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

        //[Header("Monitoring Values")]
        #region Tooltip
        [Tooltip("The object whis is targeted by this enemy, repsective to the 'Target Detection Mask'.")]
        #endregion
        [SerializeField, ReadOnly] private GameObject _targetObject;
        [SerializeField, ReadOnly] private bool _isInAttackRange;
        [SerializeField, ReadOnly] private bool _isCollidingWithOtherEnemy;

        // - - - Properties - - -
        public bool IsInAttackRange { get => _isInAttackRange; private set => _isInAttackRange = value; }
        public GameObject TargetObject { get => _targetObject; set => _targetObject = value; }

        #endregion

        #region Methods
        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------        

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

        private void OnTriggerStay2D(Collider2D collision)
        {
            // when Player is alive invoke MeleeAttack Event for informing, that Player is in Attack Range
            if (IsTargetDead)
                return;
            else if (collision.gameObject == _targetObject)
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
            else if (collision.gameObject == _targetObject)
            {
                IsInAttackRange = false;
                OnMeleeAttack?.Invoke(IsInAttackRange, collision.gameObject);
            }
            #endregion
        }
    }
}