using NaughtyAttributes;
using Player;
using UnityEngine;
using UnityEngine.Events;

public class VisualPerception : MonoBehaviour
{
    #region Events
    //--------------------------------
    // - - - - -  E V E N T S  - - - - 
    //--------------------------------

    public event UnityAction<bool, GameObject> OnPlayerDetection;
    #endregion

    #region Variables
    //--------------------------------------
    // - - - - -  V A R I A B L E S  - - - - 
    //--------------------------------------

    [Header("References")]
    [Space(2)]
    #region Tooltip
    [Tooltip("The collider the the physics ray shall be casted from for object detection. Usually it should be the collider attached to this object.")]
    #endregion
    [SerializeField] private Collider2D _raycastingCollider;
    [Space(5)]

    [Header("Perception Settings")]
    [Space(2)]
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
    [Space(5)]

    [Header("Monitoring Values")]
    [Space(2)]
    #region Tooltip
    [Tooltip("The object whis is targeted by this enemy, repsective to the 'Target Detection Mask'.")]
    #endregion
    [SerializeField, ReadOnly] private GameObject _targetObject;
    #region Tooltip
    [Tooltip("Depicts if the player character is currently detected by this enemy.")]
    #endregion
    [SerializeField, ReadOnly] private bool _isPlayerDetected = false;
    #region Tooltip
    [Tooltip("Depicts if this enemy is currently dead or not.")]
    #endregion
    [SerializeField, ReadOnly] private bool _isEnemyDead;
    #region Tooltip
    [Tooltip("Depicts if the player character is currently dead or not.")]
    #endregion
    [SerializeField, ReadOnly] private bool _isPlayerDead;


    // - - - Properties - - -
    public GameObject TargetObject { get => _targetObject; private set => _targetObject = value; }
    public float FOVAngle { get => _fOVAngle; private set => _fOVAngle = value; }
    public float FOVRadius { get => _fOVRadius; private set => _fOVRadius = value; }
    public bool IsTargetDetected { get => _isPlayerDetected; private set => _isPlayerDetected = value; }
    public bool IsEnemyDead { get => _isEnemyDead; private set => _isEnemyDead = value; }
    public bool IsPlayerDead { get => _isPlayerDead; set => _isPlayerDead = value; }
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

    private void OnEnable()
    {
        PlayerStats.OnPlayerDeath += SetIsPlayerDead;
    }
    private void OnDisable()
    {
        PlayerStats.OnPlayerDeath -= SetIsPlayerDead;
    }

    void FixedUpdate()
    {
        // storing the collider of the target object (e.g. the collider of the player object)
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, FOVRadius, _targetDetectionMask);
        //targetCollider.gameObject.TryGetComponent(out PlayerStats playerStats);

        #region cleaned If-Statements (not Tested yet)
        if (targetCollider != false && !IsPlayerDead)   // if there is a target detected and the player is not dead
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
                IsTargetDetected = false;
                FireTargetDetectedEvent();
                return;
            }

            // 4: Check if there is no obstacle object detected between the target object and this enemy object
            // if there is an obstacle Object detected between the target object and this, fire event with according values and return from this method
            if (Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
            {
                IsTargetDetected = false;
                FireTargetDetectedEvent();
                return;
            }

            // 5.: if target object is inside the field of view and there is no obstacle object detected between this and the target object, fire Event with according values
            IsTargetDetected = true;
            FireTargetDetectedEvent();

        }
        else if (_isPlayerDetected)
        {
            IsTargetDetected = false;
            FireTargetDetectedEvent();
        }
        #endregion

        #region old nested If-Statements
        //if (targetCollider != false && !IsPlayerDead)   // if there is a target detected and the player is not dead
        //{
        //    // set the _targetObject to the object the target collider is attached to
        //    _targetObject = targetCollider.gameObject;

        //    // get the direction to the target object
        //    Vector2 directionToTarget = (targetCollider.transform.position - transform.position).normalized;

        //    if (Vector2.Angle(transform.right, directionToTarget) < FOVAngle * 0.5) // if target object is inside the field of view
        //    {
        //        // store the distance to the target object
        //        float distanceToTarget = (transform.position - targetCollider.transform.position).sqrMagnitude; // distance calculation (changed that from 'V2.Distance()' because of performace reasons); JM (17.05.2024)

        //        // if there is no obstacle object detecte between the target object and this enemy object
        //        if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
        //        {                    
        //            IsTargetDetected = true;
        //            FireTargetDetectedEvent();
        //        }
        //        else // if there is an obstacle object detacted between the target object and this enemy object
        //        {
        //            IsTargetDetected = false;
        //            FireTargetDetectedEvent();
        //        }
        //    }
        //    else // if target object is not inside the field of view of this object
        //    {
        //        IsTargetDetected = false;
        //        FireTargetDetectedEvent();
        //    }
        //}
        //else if (_isPlayerDetected)
        //{
        //    IsTargetDetected = false;
        //    FireTargetDetectedEvent();
        //}
        #endregion
    }

    // - - - Custom Methods - - -
    /// <summary>
    /// Fires the event specific for the detection of the target object (e.g. player object) and transmitts the bool that depicts whether the target object was detected and the 
    /// actual detected object
    /// </summary>
    private void FireTargetDetectedEvent()
    {
        OnPlayerDetection?.Invoke(IsTargetDetected, TargetObject);

        #region debuggers little helper
        //if (IsPlayerDetected)
        //    Debug.Log($"Player is detected by '<color=orange>{gameObject.name}</color>'");
        //else
        //    Debug.Log($"Player is not anymore detected by '<color=orange>{gameObject.name}</color>'");
        #endregion
    }

    internal void SetIsEnemyDead(bool enemyDeadSatus)
    {
        IsEnemyDead = enemyDeadSatus;
    }

    private void SetIsPlayerDead(bool playerDeadStatus)
    {
        IsPlayerDead = playerDeadStatus;
    }
    #endregion
}
