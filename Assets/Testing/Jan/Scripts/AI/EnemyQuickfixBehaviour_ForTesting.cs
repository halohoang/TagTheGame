using NaughtyAttributes;
using EnumLibrary;
using UnityEngine;
using UnityEngine.AI;
using Interactables;
using System;
using UnityEngine.Animations;
using Unity.VisualScripting;
using System.IO;

[RequireComponent(typeof(Rigidbody2D), typeof(NavMeshAgent))]
public class EnemyQuickfixBehaviour_ForTesting : MonoBehaviour
{
    [Header("References")]
    [Space(2)]
    [SerializeField, ReadOnly] private Rigidbody2D _rb2d;
    [SerializeField, ReadOnly] private Transform _playerTransform;
    [SerializeField, ReadOnly] private NavMeshAgent _navAgent;
    [SerializeField] private Transform _viewDirectionHelperTrans;
    [SerializeField] private Collider2D _raycastingCollider;
    [Space(5)]

    [Header("Perception Settings")]
    [Space(2)]
    [Tooltip("Angle of the field of view.")]
    [SerializeField, Range(0.0f, 360.0f)] private float _fOVAngle = 180.0f;
    [SerializeField, Range(0.0f, 50.0f)] private float _viewDistance = 10.0f;
    [SerializeField] private LayerMask _playerDetectionMask;
    [SerializeField, ReadOnly] private bool _isPlayerInFOV;
    //[Space(2)]
    //[SerializeField, Range(0.0f, 50.0f)] private float _auditoryPerceptionRadius = 10.0f;
    [Space(5)]

    [Header("Movement Settings")]
    [Space(2)]
    [SerializeField] private float _movementSpeed;
    [Space(5)]

    [Header("Behavioour Settings (just for QuickfixSolution so far)")]
    [Space(2)]
    [SerializeField] private Enum_Lib.EEnemyType _enemyType;
    [SerializeField] private float _rotationModifier;

    private void Awake()
    {
        // Auto-referencing
        _rb2d = GetComponent<Rigidbody2D>();
        _navAgent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (_viewDirectionHelperTrans == null)
            _viewDirectionHelperTrans = gameObject.transform.GetChild(0).GetComponent<Transform>();
    }

    private void OnEnable()
    {
        // subscribing to Events
        Interactable.OnDoorKickIn += FaceAgentTowardsDoor;
    }

    private void OnDisable()
    {
        // unsubscribing to Events
        Interactable.OnDoorKickIn -= FaceAgentTowardsDoor;
    }

    private void Start()
    {
        // fixing buggy rotation and transform that happens due to NavMeshAgent on EnemyObjects
        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;

        _raycastingCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Dealing Damage to Player when Player enters Trigger-Zone around Enemy        
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
            playerHealth.GetDamage();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // simple AI-Logic (as long as Player is detected via Raycast) execute specific Enemy-Behaviour-Logic

        RaycastHit2D[] hitResults = new RaycastHit2D[1];

        Debug.DrawRay(transform.position, _viewDirectionHelperTrans.position - transform.position, Color.magenta, 0.1f);
        int numHits = _raycastingCollider.Raycast(_viewDirectionHelperTrans.position - transform.position, hitResults, _viewDistance);

        for (int i = 0; i < hitResults.Length; i++)
        {
            // debuging
            if (numHits > 0)
                Debug.Log($"RayCast-Detections: '<color=orange>{hitResults[i].collider.gameObject.name}</color>'");

            if (hitResults[i].collider.gameObject != null && hitResults[i].collider.gameObject.CompareTag("Player"))
            {
                //look towards player
                //Vector2 lookDirection = (_playerTransform.position - transform.position).normalized;
                //float angle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.right = _playerTransform.position - transform.position;

                //Debug.Log($"Player was detected by '<color=magenta>{gameObject.name}</color>'");

                // Quickfix Behaviour for different Enemy Types
                switch (_enemyType)
                {
                    // case 1: Melee-Enemy-Behaviour-Logic
                    case Enum_Lib.EEnemyType.Melee_Enemy:
                        _navAgent.speed = _movementSpeed;
                        _navAgent.SetDestination(_playerTransform.position);
                        break;

                    // case 2: Range-Enemy-Behaviour-Logic
                    case Enum_Lib.EEnemyType.Range_Enemy:

                        // shoot bullet
                        //// Calculate Deviation during the shooting
                        //float deviation = CalculateDeviation();

                        //Quaternion bulletRotation = _bulletSpawnPoint.rotation; // Apply deviation to the bullet's rotation
                        //float randomAngle = UnityEngine.Random.Range(-deviation, deviation); // Randomize the deviation angle

                        //bulletRotation *= Quaternion.Euler(0f, 0f, randomAngle); // Apply rotation around the Z-axis

                        //GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, bulletRotation);
                        //Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();
                        //_ammoCounterScript.DecreaseAmmo(); //Call the Decrease Ammo function from the AmmoCounter script;
                        //_animator.SetBool("Firing", true);
                        //_nextFireTime = Time.time + _firerate;

                        break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _viewDirectionHelperTrans.position);
    }

    //private bool CheckIfPlayerIsInFOV(Vector2 position)
    //{
    //    return _isPlayerInFOV = Vector2.Angle(transform.right, position - (Vector2)transform.position) <= _fOVAngle;
    //}


    private void FaceAgentTowardsDoor(Vector3 doorPosition, float doorKickInNoiseRange)
    {
        // Rotate the Enemy-Object so it's facing the Kicked in Door Object when Door was kicked in 
        Collider2D[] enemieColliders = Physics2D.OverlapCircleAll(doorPosition, doorKickInNoiseRange, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < enemieColliders.Length; i++)
        {
            enemieColliders[i].gameObject.transform.right = doorPosition - enemieColliders[i].gameObject.transform.position;

            //alternate solution (does not work properly)
            //Vector2 directionToDoor = (doorPosition - transform.position).normalized;
            //float alphaAngle = Mathf.Atan2(directionToDoor.x, directionToDoor.y);
            //float angleToRotate = (Mathf.PI - alphaAngle) * Mathf.Rad2Deg;
            //Quaternion quart = Quaternion.AngleAxis(angleToRotate, Vector3.forward);

            //enemieColliders[i].gameObject.transform.rotation = quart;
            //Debug.Log($"<color=orange> {enemieColliders[i].name} was rotated by {angleToRotate}� on its Z-Axis </color>");


            //alternate solution (does not work properly)
            //// calculate rotation angle (does not work as intended yet tho); JM (18.10.2023)
            //Vector2 lookDirection = (doorPosition - transform.position).normalized;
            //float angle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg - _rotationModifier;
            //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);

            //// rotat enemy-object
            //enemieColliders[i].gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, quart, 360);
        }

    }
}
