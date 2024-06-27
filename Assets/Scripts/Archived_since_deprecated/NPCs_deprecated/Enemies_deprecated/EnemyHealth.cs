using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMashine;
using Enemies;
using UnityEngine.Rendering.Universal;
using NaughtyAttributes;

namespace ArchivedSinceDeprecated
{
    public class EnemyHealth : MonoBehaviour
    {
        //--------------------------------------
        // - - - - -  V A R I A B L E S  - - - - 
        //--------------------------------------

        [Header("References")]
        [SerializeField] private GameObject _light2d;               // for Lightning and Shadow
        [SerializeField] private Animator _animator;                // for animation
        #region Tooltip
        [Tooltip("Prefabs for randomly spawning on enemy hit (taking damage) event.")]
        #endregion
        [SerializeField] private List<GameObject> _bloodPrefabPool;  // Spawning pool of blood when hit (randomly out of list)
        #region Tooltip
        [Tooltip("Prefab for spawning on enemy death event.")]
        #endregion
        [SerializeField] private GameObject _onDeathBloodPrefab;       // Spawning pool of blood at where enemy die
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;
        [Space(5)]

        [Header("Settings")]
        [SerializeField, Range(0.1f, 100.0f)] private float _maximumHealth;
        [SerializeField, Range(0.1f, 100.0f)] private float _currentHealth;
        [SerializeField] private float _flashingSpeed = 0;    // Speed of the flashing effect on taking damage  
        [SerializeField] private float _flashDuration = 0.1f; // Duration of the flashing effect on taking damage
        [Space(5)]

        [Header("Monitoring Values")]
        [SerializeField, ReadOnly] private bool _isDead = false;

        // private Variables
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider2D;
        private ShadowCaster2D _shadowCaster2D;
        private TakingDamageVFX _damageVFX;


        // properties
        internal float CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }

        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------

        // Unity provided Methods
        void Start()
        {
            #region autoreferencing
            // auto referencing
            if (_animator == null)
                _animator = GetComponent<Animator>();

            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            if (_boxCollider2D == null)
                _boxCollider2D = GetComponent<BoxCollider2D>();

            if (_shadowCaster2D == null)
                _shadowCaster2D = GetComponent<ShadowCaster2D>();
            #endregion

            // initializations
            _damageVFX = new TakingDamageVFX(GetComponent<SpriteRenderer>(), _flashingSpeed, _flashDuration);

            // value initializations
            CurrentHealth = _maximumHealth;
        }

        private void Update()
        {
            // Visual effects on taking damage
            if (_damageVFX.IsFlashing)
                _damageVFX.FlashingEffect();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            /* Spawn blood where enemy died */
            Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
            Instantiate(_onDeathBloodPrefab, transform.position, bloodRotation);
        }

        // Custom Methods
        internal void GetDamage(float damage)
        {
            if (CurrentHealth > 0)
            {
                CurrentHealth -= damage;

                // sound effects on taking damage
                _audioSource.PlayOneShot(_audioClip);

                // Visual effects on taking damage
                StartCoroutine(_damageVFX?.FlashAndRevert());

                /* Spawn blood and stay on the ground while enemy moving*/
                int randomIndex = Random.Range(0, _bloodPrefabPool.Count);
                Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
                Instantiate(_bloodPrefabPool[randomIndex], transform.position, bloodRotation);
            }
            if (CurrentHealth <= 0 && _isDead == false)
            {
                // Randomize dead rotation
                this.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
                _animator.SetTrigger("Dead");
                _isDead = true;
                _boxCollider2D.isTrigger = true;
                _light2d.SetActive(false);
                _shadowCaster2D.enabled = false;

                // Setup Enemy-Behaviour to EnemyDead
                // todo: if AI-Logic/StateMachine is fully implemented, adjust following Logic accordingly; JM (30.10.23)
                if (gameObject.TryGetComponent(out EnemyQuickfixBehaviour_ForTesting enemyQuickFixBehav))
                {
                    // todo: delete this Query if StateMachine and according new AI-Logic is fully set up and functioning; JM (30.10.23)
                    enemyQuickFixBehav.IsEnemyDead = _isDead;
                    enemyQuickFixBehav.enabled = false;
                }
                else
                {
                    Debug.Log($"<color=lime>No Quickfix behaviour cold be found on {this.gameObject.name} so other Behavióur-Scripts will be disabled</color>");
                    gameObject.GetComponent<ConditionPlayerDetectionCheck>().SetIsEnemyDead(_isDead);
                    gameObject.GetComponent<ConditionPlayerDetectionCheck>().enabled = false;
                    gameObject.GetComponent<NPCBehaviourController>().enabled = false;
                }

                gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}