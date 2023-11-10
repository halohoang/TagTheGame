using NaughtyAttributes;
using System;
using Unity.VisualScripting;
using UnityEngine;
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
        [SerializeField, Range(0.0f, 20.0f)] private float _fOVRadius;
        [SerializeField, Range(0.0f, 360.0f), ReadOnly] private float _fOVAngle = 180.0f;   // todo: disable 'Readonly' on real implementation; JM (30.10.2023)
        [SerializeField, Range(0.0f, 50.0f)] private float _viewDistance = 10.0f;
        [SerializeField] private LayerMask _targetDetectionMask;
        [SerializeField] private LayerMask _obstructionMask;
        [SerializeField, ReadOnly] private bool _isPlayerDetected = false;     // needed for estimating if player was detected
        [SerializeField, ReadOnly] private bool _isEnemyDead;

        // Properties
        public GameObject PlayerObj { get => _playerObj; private set => _playerObj = value; }
        public float FOVAngle { get => _fOVAngle; private set => _fOVAngle = value; }
        public float FOVRadius { get => _fOVRadius; private set => _fOVRadius = value; }
        public bool IsEnemyDead { get => _isEnemyDead; private set => _isEnemyDead = value; }
        public bool IsPlayerDetected { get => _isPlayerDetected; private set => _isPlayerDetected = value; }

        private void Awake()
        {
            // autoreferencing
            PlayerObj = GameObject.FindGameObjectWithTag("Player");

            if (_raycastingCollider == null)
                _raycastingCollider = GetComponent<Collider2D>();

            if (_viewDirectionHelperTrans == null)
                _viewDirectionHelperTrans = gameObject.transform.GetChild(0).GetComponent<Transform>();
        }

        void FixedUpdate()
        {
            Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, FOVRadius, _targetDetectionMask);

            if (targetCollider != false && !_isPlayerDead)
            {
                Vector2 directionToTarget = (targetCollider.transform.position - transform.position).normalized;

                if (Vector2.Angle(transform.right, directionToTarget) < FOVAngle * 0.5)
                {
                    float distanceToTarget = Vector2.Distance(transform.position, targetCollider.transform.position);   // todo: maybe cahnge 'V2.Distance()' to (a-b).sqrMagnitude for performance reasons?; JM (03.11.2023)

                    if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                    {
                        IsPlayerDetected = true;
                        FirePlayerDetectionEvent();
                    }
                    else
                    {
                        IsPlayerDetected = false;
                        FirePlayerDetectionEvent();
                    }
                }
                else
                {
                    IsPlayerDetected = false;
                    FirePlayerDetectionEvent();
                }
            }
            else if (_isPlayerDetected)
            {
                IsPlayerDetected = false;
                FirePlayerDetectionEvent();
            }
            #region oldCode
            //// raycasting for player detection by bypassing the this raycasting-objects own collider (via using 'Collider2D-Raycast()')
            //RaycastHit2D[] hitResults = new RaycastHit2D[1];
            //int numHits = _raycastingCollider.Raycast(_viewDirectionHelperTrans.position - transform.position, hitResults, _viewDistance);
            //Debug.DrawRay(transform.position, _viewDirectionHelperTrans.position - transform.position, Color.magenta, 0.1f);

            //for (int i = 0; i < hitResults.Length; i++)
            //{
            //    // debuging
            //    //if (numHits > 0)
            //    //    Debug.Log($"RayCast-Detections: '<color=orange>{hitResults[i].collider.gameObject.name}</color>'");

            //    if (hitResults[i] != false && hitResults[i].collider.gameObject.CompareTag("Player") && !IsEnemyDead)
            //    {
            //        // Setting Player detection Status
            //        IsPlayerDetected = true;
            //        OnPlayerDetection?.Invoke(IsPlayerDetected, _playerObj);
            //        Debug.Log($"Player is detected by '<color=orange>{gameObject.name}</color>'");
            //    }
            //    // set bool '_isPlayerDetected' to false in case EnemyObj is not dead but Raycast does not detect PlayerObj anymore
            //    else if (IsPlayerDetected && !IsEnemyDead && (hitResults[i] == false || !hitResults[i].collider.gameObject.CompareTag("Player")))
            //    {
            //        // Setting Player detection Status
            //        IsPlayerDetected = false;
            //        OnPlayerDetection?.Invoke(IsPlayerDetected, _playerObj);
            //        Debug.Log($"Player is not anymore detected by '<color=orange>{gameObject.name}</color>'");
            //    }
            //}
            #endregion
        }

        private void FirePlayerDetectionEvent()
        {
            OnPlayerDetection?.Invoke(IsPlayerDetected, PlayerObj);
            
            if (IsPlayerDetected)
                Debug.Log($"Player is detected by '<color=orange>{gameObject.name}</color>'");
            else
                Debug.Log($"Player is not anymore detected by '<color=orange>{gameObject.name}</color>'");
        }

        internal void SetIsEnemyDead(bool isEnemyDead)
        {
            IsEnemyDead = isEnemyDead;
        }
    }
}