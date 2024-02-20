using EnumLibrary;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    #region Events
    //--------------------------------
    // - - - - -  E V E N T S  - - - - 
    //--------------------------------
    public static event UnityAction<bool> OnPlayerDeath;
    #endregion

    #region Variables
    //--------------------------------------
    // - - - - -  V A R I A B L E S  - - - - 
    //--------------------------------------
    [Header("References")]
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _deadOverlay;
    [SerializeField] private Transform _chargeBarTransform;     // Reference to the scale of the bar
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioClip _deadSound;
    [Space(5)]

    [Header("Settings")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _chargeSpeed = 1;            // The rate at which bar depletes or charges    
    /* Health System */
    [SerializeField] internal int _takenDamage;
    [SerializeField] private float _regenCooldown = 2f;         // Adjust the duration as needed
    /*Flashing Effect */
    [SerializeField] private float _flashingSpeed = 0;
    [SerializeField] private float _flashDuration = 0.1f;       // Duration of the flashing effect
    [Space(5)]

    /* Dead Effec */
    [Header("Lists")]
    [SerializeField] private List<GameObject> _disableGameObject;
    [Space(5)]
    
    [Header("Monitoring Values")]
    [SerializeField, ReadOnly] private bool _isPlayerDead;
    [SerializeField, ReadOnly] private bool _canRegen = true;
    [SerializeField, ReadOnly] private bool _isSprinting;
    [SerializeField, ReadOnly] private float _regenTimer = 1f;
    [SerializeField, ReadOnly] private bool isFlashing = false;

    private Color defaultColor = Color.white;


    // - - - Properties - - -
    public bool IsPlayerDead { get => _isPlayerDead; private set => _isPlayerDead = value; }
    public bool IsSprinting { get => _isSprinting; private set => _isSprinting = value; }
    public float CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }
    #endregion


    #region Methods
    //----------------------------------
    // - - - - -  M E T H O D S  - - - - 
    //----------------------------------

    #region Unity-provided Methods
    private void Awake()
    {
        // auto referencing
        if (_inputReader == null)
            _inputReader = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>(); 

        if (_rb2D == null)
            _rb2D = GetComponent<Rigidbody2D>();

        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _inputReader.OnFastMovementInput += ReadInput;
    }

    private void OnDisable()
    {
        _inputReader.OnFastMovementInput -= ReadInput;
    }

    void Start()
    {
        defaultColor = _spriteRenderer.color;
        CurrentHealth = _maxHealth;
    }

    void Update()
    {
        // Health Calculations
        ReduceHP();

        
        if (CurrentHealth <= 0)    // Logic for Player Death
        {
            IsPlayerDead = true;

            foreach (GameObject gameobject in _disableGameObject)
            {
                gameobject.SetActive(false);
            }
            _animator.SetTrigger("Dead");

            OnPlayerDeath?.Invoke(IsPlayerDead);
        }       
        if (_canRegen && CurrentHealth < 100) // only call this logic if Health is below 100
        {
            RegenHP();
        }
        else
        {
            // Update the regen timer
            _regenTimer += Time.deltaTime;
            if (_regenTimer >= _regenCooldown)
            {
                _canRegen = true;
                _regenTimer = 0f; // Reset the timer
            }
        }

        // Visuals on Taking Damage
        if (isFlashing)
        {
            FlashingEffect();
        }
    }
    #endregion


    #region Custom Methods

    /// <summary>
    /// Takes the a Value of the Enum 'ESpace' as param and respective executes Logic for enabling fast movement 
    /// (if SpaceKey is currently pressed) or not (is spacekey is currently not pressed).
    /// </summary>
    /// <param name="spacePressedStatus"></param>
    private void ReadInput(Enum_Lib.ESpaceKey spacePressedStatus)
    {
        switch (spacePressedStatus)
        {
            case Enum_Lib.ESpaceKey.Pressed:

                IsSprinting = true;

                Debug.LogError($"Playerhealth: {CurrentHealth}");

                break;

            case Enum_Lib.ESpaceKey.NotPressed:

                IsSprinting = false;

                break;

            default:
                break;
        }
    }


    #region HeathCalculating Methods
    internal void GetDamage()
    {
        CurrentHealth = CurrentHealth - _takenDamage;

        FlashOnce();

        // Logic for Player Death
        if (IsSprinting && CurrentHealth <= 1.5f)
        {
            IsPlayerDead = true;

            foreach (GameObject gameobject in _disableGameObject)
            {
                gameobject.SetActive(false);
            }
            _animator.SetTrigger("Dead");

            OnPlayerDeath?.Invoke(IsPlayerDead);

            IsSprinting = false;
        }
        else if (!IsSprinting && CurrentHealth <= 0)
        {
            IsPlayerDead = true;

            foreach (GameObject gameobject in _disableGameObject)
            {
                gameobject.SetActive(false);
            }
            _animator.SetTrigger("Dead");
            _deadOverlay.SetActive(true);
            _audioSource.PlayOneShot(_deadSound);
            OnPlayerDeath?.Invoke(IsPlayerDead);
        }

        //Debug.LogError($"_GetDamage()_: -> CurrentPlayerHealth: {_currentHealth}");
    }

    private void ReduceHP()
    {
        // todo: here we have the problem why player can't die while space is pressed -> as long as space is pressed the HP can't decrease to '0'
        if (IsSprinting)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - 0.5f, 1f);
            ReduceCharge();
        }

        // Ensure _currentHealth does not go below 0
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, _maxHealth);
        //Debug.LogError($"_ReduceHP()_: -> CurrentPlayerHealth: {_currentHealth}");
    }

    private void RegenHP()
    {
        if (!Input.GetKey(KeyCode.Space) && CurrentHealth < _maxHealth) { CurrentHealth += 1; RegenCharge(); _canRegen = false; }
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, _maxHealth);

        // Debug.LogError($"_RegenHP()_: -> CurrentPlayerHealth: {_currentHealth}");
    }

    private void ReduceCharge()
    {
        if (_chargeBarTransform != null)
        {
            // Calculate the proportional health scale
            float healthScale = CurrentHealth / _maxHealth;

            // Calculate the scaled health bar value based on the proportional scale
            float scaledHealthBar = healthScale * 0.16f;

            // Clamp the scaled health bar value between 0 and 0.16
            float clampedScale = Mathf.Clamp(scaledHealthBar, 0f, 0.16f);

            // Update health bar scale based on clamped value
            _chargeBarTransform.localScale = new Vector3(_chargeBarTransform.localScale.x, clampedScale, _chargeBarTransform.localScale.z);
        }
    }

    private void RegenCharge()
    {
        if (_chargeBarTransform != null)
        {
            // Calculate the proportional health scale
            float healthScale = CurrentHealth / _maxHealth;

            // Calculate the scaled health bar value based on the proportional scale
            float scaledHealthBar = healthScale * 0.16f;

            // Clamp the scaled health bar value between 0 and 0.16
            float clampedScale = Mathf.Clamp(scaledHealthBar, 0f, 0.16f);

            // Update health bar scale based on clamped value
            _chargeBarTransform.localScale = new Vector3(_chargeBarTransform.localScale.x, clampedScale, _chargeBarTransform.localScale.z);
        }
    }
    #endregion


    #region Taking Damage Visuals
    // -----------------------------
    // --- Taking Damage related ---
    // -----------------------------

    internal void FlashOnce()
    {
        _audioSource.PlayOneShot(_audioClip);

        // Start flashing immediately
        StartCoroutine(FlashAndRevert());
    }

    private IEnumerator FlashAndRevert()
    {
        isFlashing = true;

        // Turn the enemy fully red
        _spriteRenderer.color = Color.red;

        // Wait for the flashing duration
        yield return new WaitForSeconds(_flashDuration);

        // Revert to the default color
        isFlashing = false;
        _spriteRenderer.color = defaultColor;
    }

    private void FlashingEffect()
    {
        // No need to interpolate, just set to fully red
        _spriteRenderer.color = Color.red;
    }
    #endregion

    #endregion

    #endregion
}