using EnumLibrary;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace ArchivedSinceDeprecated
{
    public class PlayerHealth : MonoBehaviour
    {
        // --- Events ---
        public static event UnityAction<bool> OnPlayerDeath;

        [Header("References")]
        [SerializeField] private InputReaderSO _inputReader;
        [Space(5)]

        // Variables
        [SerializeField] private float _maxHealth;
        [SerializeField] internal float _currentHealth;
        [SerializeField] GameObject _player;
        [SerializeField] Transform _chargeBarTransform; // Reference to the scale of the bar
        [SerializeField] float _chargeSpeed = 1; // The rate at which bar depletes or charges

        /* Dead Effec */
        private Animator _animator;
        [SerializeField] private List<GameObject> _disableGameObject;
        [SerializeField] private float _flashingSpeed; // Speed of the flashing effect
        [SerializeField] private float _flashDuration; // Duration of the flashing effect

        [Header("Monitoring Values")]
        [SerializeField, ReadOnly] private bool _isPlayerDead;
        [SerializeField] GameObject _deadOverlay;
        [SerializeField] AudioClip _deadSound;
        [SerializeField] AudioClip _hitSound;
        AudioSource _audioSource;

        /* Taking Damage Effect */
        private TakingDamageVFX _takingDamageScript;

        /* Health System */
        [SerializeField] internal int _takenDamage;
        [SerializeField] private float _regenCooldown = 2f; // Adjust the duration as needed
        private bool _canRegen = true;
        private bool _isSprinting;
        private float _regenTimer = 1f;

        private Rigidbody2D _rb2D;

        private TakingDamageVFX _damageVFX;

        public bool IsPlayerDead { get => _isPlayerDead; private set => _isPlayerDead = value; }
        public bool IsSprinting { get => _isSprinting; private set => _isSprinting = value; }

        //Functions
        private void Awake()
        {
            if (_inputReader == null)
                _inputReader = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;
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
            _rb2D = GetComponent<Rigidbody2D>();
            _currentHealth = _maxHealth;
            //_takingDamageScript = GetComponent<TakingDamageVFX>();
            _animator = GetComponent<Animator>();
            _audioSource = gameObject.AddComponent<AudioSource>();

            // initializations
            _damageVFX = new TakingDamageVFX(GetComponent<SpriteRenderer>(), _flashingSpeed, _flashDuration);
        }

        void Update()
        {
            ReduceHP();

            #region Old Hoang Approach
            if (_currentHealth <= 0)    // Logic for Player Death
            {
                IsPlayerDead = true;

                foreach (GameObject gameobject in _disableGameObject)
                {
                    gameobject.SetActive(false);
                }
                _animator.SetTrigger("Dead");

                OnPlayerDeath?.Invoke(IsPlayerDead);
            }
            #endregion
            if (_canRegen && _currentHealth < 100) // only call this logic if Health is below 100
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
        }

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

                    Debug.LogError($"Playerhealth: {_currentHealth}");

                    break;

                case Enum_Lib.ESpaceKey.NotPressed:

                    IsSprinting = false;

                    break;

                default:
                    break;
            }
        }

        internal void GetDamage()
        {
            _currentHealth = _currentHealth - _takenDamage;
            if (_damageVFX != null)
            {
                _audioSource.PlayOneShot(_hitSound);
                StartCoroutine(_damageVFX?.FlashAndRevert());
            }


            // Logic for Player Death
            if (IsSprinting && _currentHealth <= 1.5f)
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
            else if (!IsSprinting && _currentHealth <= 0)
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
        void ReduceHP()
        {
            #region Old Hoang approach
            //if (_player != null)
            //{
            //    // If the player hold down Sandevistan his health bar will start to get depleted
            //    if (Input.GetKey(KeyCode.Space))
            //    {
            //        _currentHealth = Mathf.Max(_currentHealth - 0.5f, 1f);  // ensure that playerHealth does not decrease below '1'
            //        ReduceCharge();
            //    }

            //    // Ensure _currentHealth does not go below 0
            //    _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            //    Debug.LogError($"_ReduceHP()_: -> CurrentPlayerHealth: {_currentHealth}");
            //}
            #endregion

            // todo: here we have the problem why player can't die while space is pressed -> as long as space is pressed the HP can't decrease to '0'
            if (_player != null && IsSprinting)
            {
                _currentHealth = Mathf.Max(_currentHealth - 0.5f, 1f);
                ReduceCharge();
            }

            // Ensure _currentHealth does not go below 0
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            //Debug.LogError($"_ReduceHP()_: -> CurrentPlayerHealth: {_currentHealth}");
        }

        void RegenHP()
        {
            if (!Input.GetKey(KeyCode.Space) && _currentHealth < _maxHealth) { _currentHealth += 1; RegenCharge(); _canRegen = false; }
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

            // Debug.LogError($"_RegenHP()_: -> CurrentPlayerHealth: {_currentHealth}");
        }

        void ReduceCharge()
        {
            if (_chargeBarTransform != null)
            {
                // Calculate the proportional health scale
                float healthScale = _currentHealth / _maxHealth;

                // Calculate the scaled health bar value based on the proportional scale
                float scaledHealthBar = healthScale * 0.16f;

                // Clamp the scaled health bar value between 0 and 0.16
                float clampedScale = Mathf.Clamp(scaledHealthBar, 0f, 0.16f);

                // Update health bar scale based on clamped value
                _chargeBarTransform.localScale = new Vector3(_chargeBarTransform.localScale.x, clampedScale, _chargeBarTransform.localScale.z);
            }
        }

        void RegenCharge()
        {
            if (_chargeBarTransform != null)
            {
                // Calculate the proportional health scale
                float healthScale = _currentHealth / _maxHealth;

                // Calculate the scaled health bar value based on the proportional scale
                float scaledHealthBar = healthScale * 0.16f;

                // Clamp the scaled health bar value between 0 and 0.16
                float clampedScale = Mathf.Clamp(scaledHealthBar, 0f, 0.16f);

                // Update health bar scale based on clamped value
                _chargeBarTransform.localScale = new Vector3(_chargeBarTransform.localScale.x, clampedScale, _chargeBarTransform.localScale.z);
            }
        }

    }
}