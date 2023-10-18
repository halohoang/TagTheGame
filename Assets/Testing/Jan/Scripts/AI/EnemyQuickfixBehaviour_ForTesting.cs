using NaughtyAttributes;
using EnumLibrary;
using UnityEngine;
using UnityEngine.AI;
using Interactables;
using System;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D), typeof(NavMeshAgent))]
public class EnemyQuickfixBehaviour_ForTesting : MonoBehaviour
{
    [Header("References")]
    [Space(2)]
    [SerializeField, ReadOnly] private Rigidbody2D _rb2d;
    [SerializeField, ReadOnly] private Transform _playerTransform;
    [SerializeField, ReadOnly] private NavMeshAgent _navAgent;
    [Space(5)]

    [Header("Perception Settings")]
    [Space(2)]
    [Tooltip("Angle of the field of view.")]
    [SerializeField, Range(0.0f, 360.0f)] private float _fOVAngle = 180.0f;
    [SerializeField] private float _viewDistance;
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
        _rb2d = GetComponent<Rigidbody2D>();
        _navAgent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void OnEnable()
    {
        Interactable.OnDoorKickIn += FaceAgentTowardsDoor;
    }

    private void OnDisable()
    {
        Interactable.OnDoorKickIn -= FaceAgentTowardsDoor;
    }

    private void Start()
    {
        // fixing buggy rotation and transform that happens due to NavMeshAgent on EnemyObjects
        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Quickfix Behaviour for different Enemy Types
        switch (_enemyType)
        {
            case Enum_Lib.EEnemyType.Melee_Enemy:
                _navAgent.SetDestination(_playerTransform.position);
                break;

            case Enum_Lib.EEnemyType.Range_Enemy:
                // todo: fill with logic yet; JM (17.10.2023)
                break;
        }

        if (Physics2D.Raycast(transform.position, transform.right, _viewDistance, _playerDetectionMask))
        {

        }
    }

    private bool CheckIfPlayerIsInFOV(Vector2 position)
    {
        return _isPlayerInFOV = Vector2.Angle(transform.right, position - (Vector2)transform.position) <= _fOVAngle;
    }

    private void FaceAgentTowardsDoor(Vector3 doorPosition, float doorKickInNoiseRange)
    {
        // Rotate the Enemy-Object so it's facing the Kicked in Door Object when Door was kicked in        
        Collider2D[] enemieColliders = Physics2D.OverlapCircleAll(doorPosition, doorKickInNoiseRange, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < enemieColliders.Length; i++)
        {
            // calculate rotation angle (does not work as intended yet tho); JM (18.10.2023)
            Vector2 lookDirection = (doorPosition - transform.position).normalized;
            float angle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg - _rotationModifier;
            Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);

            // rotat enemy-object
            enemieColliders[i].gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, quart, 360);
        }
    }
}
