using Interactables;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Perception
{
    public class NPCPerception : MonoBehaviour
    {
        #region Events
        //--------------------------------
        // - - - - -  E V E N T S  - - - - 
        //--------------------------------

        // visual perception related
        public static event UnityAction<bool, GameObject> OnTargetDetection;

        // auditive perception related
        public static event UnityAction<bool, Vector3> OnSomethingAlarmingIsHappening;

        // tactile perception related
        public static event UnityAction<bool, GameObject> OnInMeleeAttackRange;
        public static event UnityAction<bool, GameObject> OnCollidingWithOtherObject;

        #endregion

        #region Variables
        //--------------------------------------
        // - - - - -  V A R I A B L E S  - - - - 
        //--------------------------------------

        [Header("General Perception Settings")]
        #region Tooltip
        [Tooltip("The LayerMask of the object that shall be recognized as target by this enemy.")]
        #endregion
        [SerializeField] private LayerMask _targetDetectionMask;
        #region Tooltip
        [Tooltip("The LayerMask of objects that shall be recognized as obstacle for the vision (like walls, or doors) by this enemy. Like Objects this enemy can't look through.")]
        #endregion
        [SerializeField] private LayerMask _obstructionMask;
        [Space(5)]

        [Header("Visual Perception")]
        [Header("Needed References for visual perception")]
        #region Tooltip
        [Tooltip("The collider the the physics ray shall be casted from for object detection. Usually it should be the collider attached to this object.")]
        #endregion
        [SerializeField] private Collider2D _raycastingCollider;
        [Space(3)]        
        [Header("Visual Perception Settings")]
        #region Tooltip
        [Tooltip("The overall radius of the field of view. Equals the dinstance of the field of view.")]
        #endregion
        [SerializeField, Range(0.0f, 20.0f)] private float _fOVRadius;        
        #region Tooltip
        [Tooltip("Angle of the field of view.")]
        #endregion
        [SerializeField, Range(0.0f, 360.0f)] private float _fOVAngle = 180.0f;
        [Space(5)]

        //[Header("Audtitive Perception Settings")]
        //[Space(5)]

        //[Header("Tactile Perception Settings")]
        //[Space(5)]

        [Header("Monitoring Values")]
        
        #region Tooltip
        [Tooltip("The object whis is targeted by this enemy, repsective to the 'Target Detection Mask'.")]
        #endregion
        [SerializeField] private GameObject _targetObject;        
        #region Tooltip
        [Tooltip("Depicts if the player character is currently detected by this enemy.")]
        #endregion
        [SerializeField] private bool _isTargetDetected = false;        
        #region Tooltip
        [Tooltip("Depicts if the player character is currently dead or not.")]
        #endregion
        [SerializeField] private bool _isTargetDead;
        #region Tooltip
        [Tooltip("Depicts if this NPC is currently dead or not.")]
        #endregion
        [SerializeField] private bool _isThisNPCDead;
        #region Tooltip
        [Tooltip("Is this NPC in attack range to the detected target object?")]
        #endregion
        [SerializeField] private bool _isInAttackRange;
        #region Tooltip
        [Tooltip("Is this NPC colliding with another object e.g. another NPC or an obstacle?")]
        #endregion
        [SerializeField] private bool _isCollidingWithOtherObject;

        // - - - Properties - - -
        public float FOVAngle { get => _fOVAngle; private set => _fOVAngle = value; }
        public float FOVRadius { get => _fOVRadius; private set => _fOVRadius = value; }
        public GameObject TargetObject { get => _targetObject; private set => _targetObject = value; }
        public bool IsTargetDetected { get => _isTargetDetected; private set => _isTargetDetected = value; }
        internal LayerMask TargetDetectionMask { get => _targetDetectionMask; set => _targetDetectionMask = value; }
        internal LayerMask ObstructionMask { get => _obstructionMask; set => _obstructionMask = value; }
        internal bool IsTargetDead { get => _isTargetDead; private set => _isTargetDead = value; }
        internal bool IsThisNPCDead { get => _isThisNPCDead; private set => _isThisNPCDead = value; }
        internal bool IsInAttackRange { get => _isInAttackRange; private set => _isInAttackRange = value; }
        internal bool IsCollidingWithOtherObject { get => _isCollidingWithOtherObject; set => _isCollidingWithOtherObject = value; }
        #endregion

        #region Methods
        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------

        // - - - Unity Provided Methods - - -
        private void Awake()
        {
            #region nullChecks & respective autoreferencing
            // autoreferencing
            if (_raycastingCollider == null)
                _raycastingCollider = GetComponent<Collider2D>();
            #endregion
        }

        private void OnEnable()
        {
            // for auditive perveption
            Interactable_Door.OnDoorKickIn += CheckIfAffected;
            PlayerWeaponHandling.OnPlayerShoot += CheckIfAffected;

            // for general values
            PlayerStats.OnPlayerDeath += SetIsTargetDead;
            EnemyStats.OnEnemyDeathEvent += SetIsDead;
        }
        private void OnDisable()
        {
            // for auditive perveption
            Interactable_Door.OnDoorKickIn -= CheckIfAffected;
            PlayerWeaponHandling.OnPlayerShoot -= CheckIfAffected;

            // for general values
            PlayerStats.OnPlayerDeath -= SetIsTargetDead;
            EnemyStats.OnEnemyDeathEvent -= SetIsDead;
        }

        private void FixedUpdate()
        {
            // Execute logic for visual Perception
            if (IsThisNPCDead || IsTargetDead)
                return;
            else
                TargetDetectionCheck();
        }

        #region Tactile Perception Methods
        // Methods related to tactile perception

        /// <summary>
        /// Check if Collision with other Enemy is detected.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _isCollidingWithOtherObject = true;
                //NavAgent.isStopped = true;
                //CollisionObjectPos = collision.transform.position;

                OnCollidingWithOtherObject?.Invoke(_isCollidingWithOtherObject, collision.gameObject);

                Debug.Log($"'<color=lime>{gameObject.name}</color>': collided with '{collision.gameObject.name}'");
            }
            else
                _isCollidingWithOtherObject = false;
        }

        /// <summary>
        /// Check if is in triggerzone and thus in Attackrange to target Object
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            // when Player is alive invoke MeleeAttack Event for informing, that Player is in Attack Range
            if (IsTargetDead)
                return;
            else if (collision.gameObject == _targetObject)
            {
                IsInAttackRange = true;
                OnInMeleeAttackRange?.Invoke(IsInAttackRange, collision.gameObject);
            }
        }

        /// <summary>
        /// Check if exiting triggerzone and thus is not in attackrange to target object.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            // when Player is alive invoke MeleeAttack Event for informing, that Player is not in Attack Range anymore
            if (IsTargetDead)
                return;
            else if (collision.gameObject == _targetObject)
            {
                IsInAttackRange = false;
                OnInMeleeAttackRange?.Invoke(IsInAttackRange, collision.gameObject);
            }
        }
        #endregion

        // - - - Custom Methods - - -

        #region Visual Perception Methods
        // Methods related to visual Perception

        /// <summary>
        /// Checks if the target object is inside the Field ov view (<see cref="FOVRadius"/>, <see cref="FOVAngle"/>) and therefore detected by this npc object. Respectiv to the check result
        /// an event will be fired which carrys/transmitts result-respective values for informing about the outcome of the detection check.
        /// </summary>
        private void TargetDetectionCheck()
        {
            // storing the collider of the target object (e.g. the collider of the player object)
            Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, FOVRadius, _targetDetectionMask);

            if (targetCollider != false)   // if there is a target detected
            {
                // 1.: set the _targetObject to the object the target collider is attached to
                _targetObject = targetCollider.gameObject;

                // 2.: get the direction and distance to the target object
                Vector2 directionToTarget = (targetCollider.transform.position - transform.position).normalized;
                float distanceToTarget = (transform.position - targetCollider.transform.position).sqrMagnitude;

                // 3.: Check if target object is inside field of view
                // if target object is not inside the field of view fire event with according values and return from this method
                if (!(Vector2.Angle(transform.right, directionToTarget) < FOVAngle * 0.5))
                {
                    _isTargetDetected = false;
                    InformAboutPlayerDetectionStatus();
                    return;
                }

                // 4: Check if there is no obstacle object detected between the target object and this enemy object
                // if there is an obstacle Object detected between the target object and this, fire event with according values and return from this method
                if (Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                {
                    _isTargetDetected = false;
                    InformAboutPlayerDetectionStatus();
                    return;
                }

                // 5.: if target object is inside the field of view and there is no obstacle object detected between this and the target object, fire Event with according values
                _isTargetDetected = true;
                InformAboutPlayerDetectionStatus();

            }
            else if (IsTargetDetected) // set '_isTargetDetected' to false if it is not already set to false and there is no target detected
            {
                _isTargetDetected = false;
                InformAboutPlayerDetectionStatus();
            }
        }

        /// <summary>
        /// Fires the event specific for the detection of the target object (e.g. player object) and transmitts the bool that depicts whether the target object was detected and the 
        /// actual detected object
        /// </summary>
        private void InformAboutPlayerDetectionStatus()
        {
            OnTargetDetection?.Invoke(_isTargetDetected, _targetObject);

            #region debuggers little helper
            //if (IsPlayerDetected)
            //    Debug.Log($"Player is detected by '<color=orange>{gameObject.name}</color>'");
            //else
            //    Debug.Log($"Player is not anymore detected by '<color=orange>{gameObject.name}</color>'");
            #endregion
        }
        #endregion


        #region Auditive Perception Methods
        // Methods related to auditive Perception

        /// <summary>
        /// Checks if this enemy object is affected by alarming event like Door-KickIn or 'hearing' shooting noises etc.
        /// </summary>
        /// <param name="isSomethinAlarmingHappening"></param>
        /// <param name="positionOfAlarmingEvent"></param>
        /// <param name="CollidersWithinRange"></param>
        private void CheckIfAffected(bool isSomethinAlarmingHappening, Vector3 positionOfAlarmingEvent, Collider2D[] CollidersWithinRange)
        {
            Collider2D thisCollider = GetComponent<Collider2D>();

            // check if this object is among the enemy objects that are actually affected by the alarming event
            for (int i = 0; i < CollidersWithinRange.Length; i++)
            {
                if (thisCollider == CollidersWithinRange[i])
                    OnSomethingAlarmingIsHappening?.Invoke(isSomethinAlarmingHappening, positionOfAlarmingEvent);
            }
        }
        #endregion


        #region General Methods
        // General perception Methods

        /// <summary>
        /// Sets the bool <see cref="_isTargetDead"/> respective to transmitted parameter 'targetDeadStatus'.
        /// </summary>
        /// <param name="targetDeadStatus"></param>
        private void SetIsTargetDead(bool targetDeadStatus)
        {
            _isTargetDead = targetDeadStatus;
        }

        /// <summary>
        /// Sets the bool <see cref="_isThisNPCDead"/> respective to transmitted parameter 'isDeadStatus' if this gameobject is equal to the transmitted gameObject.
        /// </summary>
        /// <param name="isDeadStatus"></param>
        /// <param name="affectedNPCObject"></param>
        private void SetIsDead(bool isDeadStatus, GameObject affectedNPCObject)
        {
            if (this.gameObject == affectedNPCObject)
                _isThisNPCDead = isDeadStatus;
        }
        #endregion

        #endregion
    }
}