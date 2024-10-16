using EnumLibrary;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [DisallowMultipleComponent]
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
        [SerializeField] private Sprite _deadOverlaySprite;
        [SerializeField] private Transform _chargeBarTransform;                     // Reference to the scale of the bar
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _deadSound;
        [Space(5)]

        [Header("Settings")]
        /* Health System */
        [SerializeField, Range(0.1f, 100.0f)] private float _maxHealth;
        #region Tooltip
        [Tooltip("The damage the PlayerCharacter will take on any attack impact, no matter if bullet or melee attack.")]
        #endregion
        [SerializeField, Range(0.1f, 100.0f)] private int _takenDamage;            // todo: for more variability on taken damage this might need to be changed yet
        [SerializeField] private float _chargeSpeed = 1;                            // The rate at which bar depletes or charges    
        [SerializeField] private float _regenCooldown = 2f;                         // Adjust the duration as needed
        /*Flashing Effect */
        [SerializeField] private float _flashingSpeed = 0;
        [SerializeField] private float _flashDuration = 0.1f;                       // Duration of the flashing effect
        [Space(5)]

        /* Dead Effec */
        [Header("Lists")]
        [SerializeField] private List<GameObject> _disableGameObject;
        [Space(5)]

        [Header("Monitoring Values")]
        [SerializeField, ReadOnly] private bool _isPlayerInvincible = false;
        [SerializeField, ReadOnly] private float _currentHealth;                    // todo: implement a stamina and seperate this from the health value; JM
        [SerializeField, ReadOnly] private float _regenTimer = 1f;
        [SerializeField, ReadOnly] private bool _isPlayerDead;
        [SerializeField, ReadOnly] private bool _canRegen = true;
        [SerializeField, ReadOnly] private bool _isSprinting;
        [SerializeField, ReadOnly] private BoxCollider2D[] _colliderToDisableOnDeath;
        //[SerializeField, ReadOnly] private bool isFlashing = false;

        private Color defaultColor = Color.white;
        private TakingDamageVFX _damageVFX;

        // - - - Properties - - -
        public bool IsPlayerDead { get => _isPlayerDead; private set => _isPlayerDead = value; }
        public bool IsSprinting { get => _isSprinting; private set => _isSprinting = value; }
        public float CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }
        public bool IsPlayerInvincible { get => _isPlayerInvincible; private set => _isPlayerInvincible = value; }
        internal int TakenDamage { get => _takenDamage; private set => _takenDamage = value; }
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

            if (_deadOverlaySprite == null)
                _deadOverlaySprite = Resources.Load<Sprite>("Sprites/Player/PlayerDead");

            _colliderToDisableOnDeath = new BoxCollider2D[GetComponents<BoxCollider2D>().Length];
            _colliderToDisableOnDeath = GetComponents<BoxCollider2D>();
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
            // initializadions
            _damageVFX = new TakingDamageVFX(_spriteRenderer, _flashingSpeed, _flashDuration);

            defaultColor = _spriteRenderer.color;
            CurrentHealth = _maxHealth;
        }

        void Update()
        {
            // Health Calculations
            ReduceHP();
            
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

            // Visual effects on Taking Damage
            if (_damageVFX.IsFlashing)
                _damageVFX?.FlashingEffect();
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

                    Debug.Log($"Sprinting Input detected -> Playerhealth: {CurrentHealth}");

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
            if (!IsPlayerInvincible)
                CurrentHealth = CurrentHealth - _takenDamage;

            // Visual effects on taking damage        
            StartCoroutine(_damageVFX?.FlashAndRevert());

            // Logic for Player Death
            if (IsSprinting && CurrentHealth <= 1.5f)
            {
                ExecutePlayerDeadLogic();

                IsSprinting = false;
            }
            else if (!IsSprinting && CurrentHealth <= 0)
            {
                ExecutePlayerDeadLogic();
            }

            //Debug.LogError($"_GetDamage()_: -> CurrentPlayerHealth: {_currentHealth}");
        }

        /// <summary>
        /// Executes all necessary logic on player death event like setting specific values, disabling objects and components and eventually firing the 'OnPlayerDeath'-Event
        /// to inform other Classes about PlayerDead
        /// </summary>
        private void ExecutePlayerDeadLogic()
        {
            // 1. Set PlayerDead value
            IsPlayerDead = true;

            // 2.  Disable rigidbody-simulation and exchange Sprite for Player to Dead-Sprite
            _rb2D.simulated = false;                        // turn of physics simulation on rigidbody so the body won'T slide over the ground if any force was applied shortly before death
            _animator.enabled = false;      // Disable Animator to avoid interference (Todo: rework later and clean this later; JM (12.09.24))
            _spriteRenderer.sprite = _deadOverlaySprite;
            _spriteRenderer.sortingLayerName = "default";   // reset sorting layer of sprite so enemies walk over dead player

            // 3. play dead sound
            _audioSource.PlayOneShot(_deadSound);   

            // 4. disable all Childobjects of Player-Obj on Death (Goggle Light etc.) 
            foreach (GameObject gameobject in _disableGameObject)
            {
                gameobject.SetActive(false);
            }
            
            // 5. disable all Boxcollider-components on Player-Objec to avoid interaction with still living Enemies
            for (int i = 0; i < _colliderToDisableOnDeath.Length; i++)
                _colliderToDisableOnDeath[i].enabled = false;

            // 6. Fire 'OnPlayerDeath'-Event to inform other Classes about Player Dead
            OnPlayerDeath?.Invoke(IsPlayerDead);
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

        internal void SetPlayerInvincibleStatus(bool isInvincible)
        {
            IsPlayerInvincible = isInvincible;
        }

        #region Taking Damage Visuals
        // -----------------------------
        // --- Taking Damage related ---
        // -----------------------------

        //internal void FlashOnce()
        //{
        //    _audioSource.PlayOneShot(_audioClip);

        //    // Start flashing immediately
        //    StartCoroutine(FlashAndRevert());
        //}

        //private IEnumerator FlashAndRevert()
        //{
        //    isFlashing = true;

        //    // Turn the enemy fully red
        //    _spriteRenderer.color = Color.red;

        //    // Wait for the flashing duration
        //    yield return new WaitForSeconds(_flashDuration);

        //    // Revert to the default color
        //    isFlashing = false;
        //    _spriteRenderer.color = defaultColor;
        //}

        //private void FlashingEffect()
        //{
        //    // No need to interpolate, just set to fully red
        //    _spriteRenderer.color = Color.red;
        //}
        #endregion

        #endregion

        #endregion
    }
}