using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace ArchivedSinceDeprecated
{
    public class VisualPerception : BasePerception
    {
        #region Events
        //--------------------------------
        // - - - - -  E V E N T S  - - - - 
        //--------------------------------

        public static event UnityAction<bool, GameObject> OnTargetDetection;
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
        #region Tooltip
        [Tooltip("Depicts if the player character is currently detected by this enemy.")]
        #endregion
        [SerializeField, ReadOnly] private bool _isTargetDetected = false;
        [Space(5)]

        [Header("References")]
        #region Tooltip
        [Tooltip("The collider the the physics ray shall be casted from for object detection. Usually it should be the collider attached to this object.")]
        #endregion
        [SerializeField] private Collider2D _raycastingCollider;
        [Space(5)]

        [Header("Perception Settings")]
        #region Tooltip
        [Tooltip("The overall radius of the field of view. Equals the dinstance of the field of view.")]
        #endregion
        [SerializeField, Range(0.0f, 20.0f)] private float _fOVRadius;
        #region Tooltip
        [Tooltip("Angle of the field of view.")]
        #endregion
        [SerializeField, Range(0.0f, 360.0f)] private float _fOVAngle = 180.0f;
        #region Tooltip
        [Tooltip("The LayerMask of the object that shall be recognized as target by this enemy.")]
        #endregion
        [SerializeField] private LayerMask _targetDetectionMask;
        #region Tooltip
        [Tooltip("The LayerMask of objects that shall be recognized as obstacle for the vision (like walls, or doors) by this enemy. Like Objects this enemy can't look through.")]
        #endregion
        [SerializeField] private LayerMask _obstructionMask;


        // - - - Properties - - -
        public float FOVAngle { get => _fOVAngle; private set => _fOVAngle = value; }
        public float FOVRadius { get => _fOVRadius; private set => _fOVRadius = value; }
        protected LayerMask TargetDetectionMask { get => _targetDetectionMask; set => _targetDetectionMask = value; }
        protected LayerMask ObstructionMask { get => _obstructionMask; set => _obstructionMask = value; }
        public GameObject TargetObject { get => _targetObject; private set => _targetObject = value; }
        public bool IsTargetDetected { get => _isTargetDetected; private set => _isTargetDetected = value; }        
        #endregion


        #region Methods
        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------

        // - - - Unity Provided Methods - - -
        private void Awake()
        {
            // autoreferencing
            if (_raycastingCollider == null)
                _raycastingCollider = GetComponent<Collider2D>();
        }

        void FixedUpdate()
        {
            if (IsDead || IsTargetDead)
                return;
            else
                TargetDetectionCheck();
        }

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

        // - - - Custom Methods - - -
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
    }
}