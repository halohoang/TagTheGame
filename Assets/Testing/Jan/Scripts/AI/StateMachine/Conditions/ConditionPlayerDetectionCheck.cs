using Enemies;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace StateMashine
{
    public class ConditionPlayerDetectionCheck : BaseConditionCheck
    {
        public event UnityAction<bool, GameObject> OnPlayerDetection;

        [Header("References")]
        [Space(2)]
        [SerializeField, ReadOnly] private GameObject _playerObj;
        [SerializeField] private Transform _viewDirectionHelperTrans;
        [SerializeField] private Collider2D _raycastingCollider;
        [Space(5)]

        [Header("Perception Settings")]
        [Space(2)]
        [Tooltip("Angle of the field of view.")]
        //[SerializeField, Range(0.0f, 360.0f), ReadOnly] private float _fOVAngle = 180.0f;   // todo: disable 'Readonly' on real implementation; JM (30.10.2023)
        [SerializeField, Range(0.0f, 50.0f)] private float _viewDistance = 10.0f;
        //[SerializeField] private LayerMask _playerDetectionMask;
        //[SerializeField, ReadOnly] private bool _isPlayerInFOV;
        [SerializeField, ReadOnly] private bool _isPlayerDetected = false;     // needed for estimating if player was detected so if so, the enemy will be 'searching' for the player
        [SerializeField, ReadOnly] private bool _isEnemyDead;

        // Properties
        //public bool IsPlayerInFOV { get => _isPlayerInFOV; set => _isPlayerInFOV = value; }
        public bool IsEnemyDead { get => _isEnemyDead; private set => _isEnemyDead = value; }
        public bool IsPlayerDetected { get => _isPlayerDetected; private set => _isPlayerDetected = value; }

        private void Awake()
        {
            // autoreferencing
            _playerObj = GameObject.FindGameObjectWithTag("Player");

            if (_raycastingCollider == null)
                _raycastingCollider = GetComponent<Collider2D>();

            if (_viewDirectionHelperTrans == null)
                _viewDirectionHelperTrans = gameObject.transform.GetChild(0).GetComponent<Transform>();
        }

        void FixedUpdate()
        {
            // raycasting for player detection
            RaycastHit2D[] hitResults = new RaycastHit2D[1];
            int numHits = _raycastingCollider.Raycast(_viewDirectionHelperTrans.position - transform.position, hitResults, _viewDistance);
            Debug.DrawRay(transform.position, _viewDirectionHelperTrans.position - transform.position, Color.magenta, 0.1f);

            for (int i = 0; i < hitResults.Length; i++)
            {
                // debuging
                //if (numHits > 0)
                //    Debug.Log($"RayCast-Detections: '<color=orange>{hitResults[i].collider.gameObject.name}</color>'");


                if (hitResults[i] != false && hitResults[i].collider.gameObject.CompareTag("Player") && !IsEnemyDead)
                {
                    // Setting Player detection Status
                    IsPlayerDetected = true;
                    OnPlayerDetection?.Invoke(IsPlayerDetected, _playerObj);
                    Debug.Log($"Player is detected by '<color=orange>{gameObject.name}</color>'");
                }
                // set bool '_isPlayerDetected' to false in case EnemyObj is not dead but Raycast does not detect PlayerObj anymore
                else if (IsPlayerDetected && !IsEnemyDead && (hitResults[i] == false || !hitResults[i].collider.gameObject.CompareTag("Player"))) 
                {
                    // Setting Player detection Status
                    IsPlayerDetected = false;
                    OnPlayerDetection?.Invoke(IsPlayerDetected, _playerObj);
                    Debug.Log($"Player is not anymore detected by '<color=orange>{gameObject.name}</color>'");
                }
            }
        }

       internal void SetIsEnemyDead(bool isEnemyDead)
        {
            IsEnemyDead = isEnemyDead;
        }
    }
}