using Enemies;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace StateMashine
{
    public class ConditionPlayerDetectionCheck : BaseConditionCheck
    {
        [Header("References")]
        [Space(2)]
        [SerializeField, ReadOnly] private GameObject _playerObj;
        [SerializeField, ReadOnly] private BaseEnemyBehaviour _enemyBehaviour;
        [SerializeField] private Transform _viewDirectionHelperTrans;
        [SerializeField] private Collider2D _raycastingCollider;
        [Space(5)]

        [Header("Perception Settings")]
        [Space(2)]
        [Tooltip("Angle of the field of view.")]
        [SerializeField, Range(0.0f, 360.0f), ReadOnly] private float _fOVAngle = 180.0f;   // todo: disable 'Readonly' on real implementation; JM (30.10.2023)
        [SerializeField, Range(0.0f, 50.0f)] private float _viewDistance = 10.0f;
        [SerializeField] private LayerMask _playerDetectionMask;
        [SerializeField, ReadOnly] private bool _isPlayerInFOV;
        [SerializeField, ReadOnly] private bool _isPlayerDetected = false;     // needed for estimating if player was detected so if so, the enemy will be 'searching' for the player
        [SerializeField, ReadOnly] private bool _isEnemyDead;

        public bool IsPlayerInFOV { get => _isPlayerInFOV; set => _isPlayerInFOV = value; }
        public bool IsEnemyDead { get => _isEnemyDead; set => _isEnemyDead = value; }

        private void Awake()
        {
            _playerObj = GameObject.FindGameObjectWithTag("Player");
            _enemyBehaviour = GetComponent<BaseEnemyBehaviour>();

            if (_raycastingCollider == null)
                _raycastingCollider = GetComponent<Collider2D>();

            if (_viewDirectionHelperTrans == null)
                _viewDirectionHelperTrans = gameObject.transform.GetChild(0).GetComponent<Transform>();
        }

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


                if (hitResults[i] != false && hitResults[i].collider.gameObject.CompareTag("Player") && !IsEnemyDead)
                {
                    _isPlayerDetected = true;
                    _enemyBehaviour.SetIsPlayerDetected(_isPlayerDetected);
                    Debug.Log($"Player is detected by '<color=orange>{gameObject.name}</color>'");

                    transform.right = _playerObj.transform.position - transform.position;
                    
                    // (!)todo: do not forget to set Animation/Animator accordingly to Playerdetection (do it via the 'AnimationTriggerEvent()' or similar!); JM (30.10.2023)
                    //_animtor.SetBool("Engage", true);
                }
                // set bool '_isPlayerDetected' to false in case EnemyObj is not dead but Raycast does not detect PlayerObj anymore
                else if (_isPlayerDetected && !IsEnemyDead && (hitResults[i] == false || !hitResults[i].collider.gameObject.CompareTag("Player"))) 
                {                    
                    _isPlayerDetected = false;
                    _enemyBehaviour.SetIsPlayerDetected(_isPlayerDetected);
                    Debug.Log($"Player is not anymore detected by '<color=orange>{gameObject.name}</color>'");

                    // (!)todo: do not forget to set Animation/Animator accordingly to Playerdetection (do it via the 'AnimationTriggerEvent()' or similar!); JM (30.10.2023)
                    //_animtor.SetBool("Engage", false);
                }
            }
        }
    }
}