using NaughtyAttributes;
using EnumLibrary;
using UnityEngine;
using UnityEngine.AI;

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
    [Space(5)]

    [Header("Movement Settings")]
    [Space(2)]
    [SerializeField] private float _movementSpeed;
    [Space(5)]

    [Header("Behavioour Settings (just for QuickfixSolution so far)")]
    [Space(2)]
    [SerializeField] private Enum_Lib.EEnemyType _enemyType;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _navAgent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
                _navAgent.destination = _playerTransform.position;
                break;

            case Enum_Lib.EEnemyType.Range_Enemy:
                // todo: fill with logic yet; JM (17.10.2023)
                break;
        }

        if (Physics2D.Raycast(transform.position, transform.right, _viewDistance, _playerDetectionMask))
        {

        }
    }

    private void OnDrawGizmos()
    {

    }

    private bool CheckIfPlayerIsInFOV(Vector2 position)
    {
        return _isPlayerInFOV = Vector2.Angle(transform.right, position - (Vector2)transform.position) <= _fOVAngle;
    }
}
