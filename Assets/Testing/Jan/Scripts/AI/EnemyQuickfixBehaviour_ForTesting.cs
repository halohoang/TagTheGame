using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyQuickfixBehaviour_ForTesting : MonoBehaviour
{
    [Header("References")]
    [Space(2)]
    [SerializeField, ReadOnly] private Rigidbody2D _rb2d;
    [SerializeField, ReadOnly] private Transform _playerTransform;
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

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb2d.MovePosition(_playerTransform.position - transform.position * Time.deltaTime/* * _movementSpeed*/);

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
