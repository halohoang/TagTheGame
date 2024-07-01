using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class EnemyStats : MonoBehaviour
{
    #region Events
    //--------------------------------
    // - - - - -  E V E N T S  - - - - 
    //--------------------------------
    public static event UnityAction<bool, GameObject> OnEnemyDeathEvent;
    #endregion

    #region Variables
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
    [SerializeField] private AudioSource _audioSource;
    #region Tooltip
    [Tooltip("The audioclip that shall be played when enemy is receives damage.")]
    #endregion
    [SerializeField] private AudioClip _receiveDamageSound;
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


    // - - - properties - - -
    internal float CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }
    #endregion

    #region Methods
    //----------------------------------
    // - - - - -  M E T H O D S  - - - - 
    //----------------------------------

    // - - - Unity provided Methods - - -
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

    // - - - Custom Methods - - -
    internal void GetDamage(float damage)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damage;

            // sound effects on taking damage
            _audioSource?.PlayOneShot(_receiveDamageSound);

            // Visual effects on taking damage
            StartCoroutine(_damageVFX?.FlashAndRevert());

            SpawnBloodOnGround();
        }
        else if (CurrentHealth <= 0 && _isDead == false)
        {
            // 1. set important values
            CurrentHealth = 0;
            _isDead = true;
            _boxCollider2D.isTrigger = true;    // set collider to trigger to ensure no collision is possible any more

            // 2. Randomize dead rotation and enable dead animation sprite
            this.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
            _animator.SetTrigger("Dead");
                        
            // 3. disableing light emission
            _light2d.SetActive(false);
            _shadowCaster2D.enabled = false;

            SpawnBloodOnGround();

            // 4. Setup Enemy-Behaviour to EnemyDead
            OnEnemyDeathEvent?.Invoke(_isDead, this.gameObject);
            //gameObject.GetComponent<ConditionPlayerDetectionCheck>().SetIsEnemyDead(_isDead);
            //gameObject.GetComponent<ConditionPlayerDetectionCheck>().enabled = false;
            //gameObject.GetComponent<NPCBehaviourController>().enabled = false;

            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        }
    }

    private void SpawnBloodOnGround()
    {
        /* Spawn blood and stay on the ground while enemy moving*/
        int randomIndex = Random.Range(0, _bloodPrefabPool.Count);
        Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
        Instantiate(_bloodPrefabPool[randomIndex], transform.position, bloodRotation);  // todo: exchange this with using object pool; JM (27.06.2024)
    }
    #endregion
}
