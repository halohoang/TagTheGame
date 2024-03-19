using NaughtyAttributes;
using ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using EnumLibrary;

public class PlayerAttack : MonoBehaviour
{
    #region Events
    //--------------------------------
    // - - - - -  E V E N T S  - - - - 
    //--------------------------------

    public static event UnityAction<bool, Vector3, float> OnPlayerShoot;
    #endregion


    #region Variables
    //--------------------------------------
    // - - - - -  V A R I A B L E S  - - - - 
    //--------------------------------------

    //--- SerializedFields Variables ---
    [Header("References")]
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private PlayerEquipmentSO _playerEquipment;
    [SerializeField] private PlayerController _playerCtrl;

    /* Gun related References*/
    [SerializeField] GameObject _gun;
    [SerializeField] private Transform _bulletSpawnPoint;           // Transform of the gun
    [SerializeField] private GameObject _bulletPrefab;

    /* Reload System References */
    [SerializeField] AmmoCounter _ammoCounterScript;                // Link to the AmmoCounter script

    /* Bullet Casing Spawining References*/
    [SerializeField] private GameObject _bulletCasingPrefab;        // Prefab of the bullet casing
    [SerializeField] private Transform _casingSpawnPosition;

    /* AudioClip References*/
    [SerializeField] private AudioClip _fireSound;                  // Fire sound
    [SerializeField] private AudioClip _reloadSound;                // Reload sound

    /* Camera Shake References */
    [SerializeField] private CameraRecoilShake cameraShake;

    /* Muzzle Flash References */
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private Animator _animatorCtrl;
    [Space(5)]


    [Header("Settings")]
    /* Reload system */
    //[SerializeField] internal int _minimumBulletCount;            // Will use to do sound for low ammo  sound    
    [SerializeField] private float _reloadTime;                     // To sync with how long the reload animation is    

    /* Firerate Settings*/
    private float _nextFireTime;

    /* Recoil Settings*/
    [SerializeField] private float _maxDeviationAngle = 5f;         // Maximum deviation the bullet will be off from the straight line
    [SerializeField] private float _whenDeviationKicksIn;

    /* Shooting Noise Range Settings*/
    [SerializeField, Range(0.0f, 25.0f), EnableIf("_showNoiseRangeGizmo")] private float _shootingNoiseRange = 10.0f;
    [Tooltip("Defines whether the green gizmo circle around the player showing the noise range when shooting, is shown in the editor or not.")]
    [SerializeField] private bool _showNoiseRangeGizmo = true;

    /* Camera Shake Settings*/
    [SerializeField] internal float duration = 0.05f;
    [SerializeField] internal float amount = 0.08f;
    [Space(5)]


    [Header("Monitoring Values")]
    /* Reload System */
    [SerializeField, ReadOnly] internal int _maximumBulletCount;
    [SerializeField, ReadOnly] internal int _currentBulletCount;

    /* Firerate Settings*/
    [SerializeField, ReadOnly] private float _firerate = 0.5f;

    /* Different boolian Values */
    [SerializeField, ReadOnly] private bool _isShooting;
    [SerializeField, ReadOnly] private bool _isPlayerDead;
    [SerializeField, ReadOnly] private bool _IsGamePaused;
    [SerializeField, ReadOnly] private bool _isArmed;               // Checking whether the player is armed or not
    [SerializeField, ReadOnly] private bool _isReloading = false;

    // --- private Variables ---
    private AudioSource _audioSource;
    private float _mouseButtonReleaseTime;                          // Time when the mouse button was last released
    #endregion


    #region Methods
    //----------------------------------
    // - - - - -  M E T H O D S  - - - - 
    //----------------------------------

    #region Unity-provided Methods
    private void OnDrawGizmos()
    {
        if (_showNoiseRangeGizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _shootingNoiseRange);
        }
    }

    private void Awake()
    {
        // auto referencing
        if (_inputReader == null)
        {
            _inputReader = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;
            Debug.Log($"<color=yellow>Caution!</color>: Reference for ScriptableObject 'InputReader' in Inspector of {this} was not set. So it was Set automatically, if you want or need to set a specific InputReader Asset, set it manually instead.");
        }

        if (_playerEquipment == null)
        {
            _playerEquipment = Resources.Load("ScriptableObjects/PlayerEquipment") as PlayerEquipmentSO;
            Debug.Log($"<color=yellow>Caution!</color>: Reference for ScriptableObject 'PlayerEquipment' in Inspector of {this} was not set. So it was Set automatically.");
        }

        if (_playerCtrl == null)
        {
            _playerCtrl = GetComponent<PlayerController>();
            Debug.Log($"<color=yellow>Caution!</color>: Reference for 'PlayerController'-Component in Inspector of {this} was not set. So it was Set automatically.");
        }

        if (_animatorCtrl == null)
        {
            _animatorCtrl = GetComponent<Animator>();
            Debug.Log($"<color=yellow>Caution!</color>: Reference for 'Animator'-Component in Inspector of {this} was not set. So it was Set automatically.");
        }

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _inputReader.OnWeaponSwitch += SwitchWeapon;
        PlayerHealth.OnPlayerDeath += SetIsPlayerDead;
        PauseMenu.OnTogglePauseScene += SetIsGamePaused;
    }
    private void OnDisable()
    {
        _inputReader.OnWeaponSwitch -= SwitchWeapon;
        PlayerHealth.OnPlayerDeath -= SetIsPlayerDead;
        PauseMenu.OnTogglePauseScene -= SetIsGamePaused;
    }

    private void Start()
    {
        // Set is Armed Status
        //SetIsArmed(/*input 'loaded' value from ScriptableObjec*/); // -> intention is to 'load' isArmed-Data on Scene Start accordingly to the value when player left last scene

        // Set MaxBullet Count to Magazine Size of currently held weapon (note, if player holds no weapon this will be '0')
        _maximumBulletCount = _playerEquipment.WeaponInHeand.MagazineSize;

        if (_playerEquipment.WeaponInHeand.CurrentRoundsInMag == _maximumBulletCount) // if amount of current bullets in weapons magazine equals maximum bullet count than set _currentBullet count to max bullet count
        {
            _currentBulletCount = _maximumBulletCount;
        }
        else // set current bulet count to the rounds currently in the mag of the weapon in hand
        {
            _currentBulletCount = _playerEquipment.WeaponInHeand.CurrentRoundsInMag;
        }
    }

    private void Update()
    {
        DrawOrHolsterWeapon();
        if (_isArmed && !_isPlayerDead)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _mouseButtonReleaseTime = Time.time; // Record the time when the mouse button was released
            }
            else if (Input.GetMouseButton(0) && CanFire() && _currentBulletCount > 0 && _isReloading == false)
            {
                _isShooting = true;
                //_animator.SetBool("Firing", _isShooting);
                Shoot();
                _currentBulletCount--;
                SpawnBulletCasing();
                cameraShake.StartShake(duration, amount);
                Debug.Log("Shake");

                OnPlayerShoot?.Invoke(_isShooting, transform.position, _shootingNoiseRange);
            }
            else
            {
                _isShooting = false;
                //_animator.SetBool("Firing", _isShooting);
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && _isReloading == false && !Input.GetMouseButton(0) && !_isPlayerDead)
        {
            if (_currentBulletCount < _maximumBulletCount)
            {
                _ammoCounterScript.Reload();
                StartCoroutine(Reload());
            }
        }

        //EquipWeaponAnimation(_isArmed);
    }
    #endregion


    #region Custom Methods
    private void DrawOrHolsterWeapon()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isArmed = !_isArmed;
            _gun.SetActive(_isArmed);
            _animatorCtrl.SetBool("Armed", _isArmed);

            if (!_playerCtrl.IsPlayerMoving && !_isArmed)
                _animatorCtrl.SetBool("Armed", false);

            EquipWeaponAnimation(_isArmed);
        }
    }

    private void SwitchWeapon()
    {
        // Update UI


        // Update PlayerEquipmentSO
        _playerEquipment.SwitchWeapon();


        // Enable proper Animation (if Player is armed) accordingly to equipped weapon
        EquipWeaponAnimation(_isArmed);
    }

    private void EquipWeaponAnimation(bool playAnimation)
    {
        if (playAnimation)
        {
            switch (_playerEquipment.WeaponInHeand.WeaponType)
            {
                case Enum_Lib.EWeaponType.Handgun:
                    SetAnimation("Canon");
                    #region alternative
                    //if (_playerCtrl.IsPlayerMoving)
                    //{
                    //    // Set Animation parameter for Moving animation
                    //    _animatorCtrl.SetTrigger("Canon_Walk");
                    //}
                    //else
                    //{
                    //    // set animation  parameter for idle
                    //    _animatorCtrl.SetTrigger("Canon_Idle");
                    //}
                    #endregion
                    break;

                case Enum_Lib.EWeaponType.SMG:
                    SetAnimation("SMG");
                    break;

                case Enum_Lib.EWeaponType.Shotgun:
                    SetAnimation("Shotgun");
                    break;

                case Enum_Lib.EWeaponType.EnergyLauncher:
                    SetAnimation("Launcher");
                    break;

                case Enum_Lib.EWeaponType.Blank:
                    _animatorCtrl.SetBool("Armed", false);
                    break;

                default:
                    break;
            }
        }
    }

    private void SetAnimation(string nameOfWeapon)
    {
        if (_playerCtrl.IsPlayerMoving) // Set Animation parameter for Moving animation with Handgun    
            _animatorCtrl.SetTrigger($"{nameOfWeapon}_Walk");
        else                            // set animation parameter for idle with Handgun
            _animatorCtrl.SetTrigger($"{nameOfWeapon}_Idle");
    }

    private bool CanFire()
    {
        return Time.time > _nextFireTime && !_IsGamePaused;
    }

    private float CalculateDeviation()
    {
        float holdTriggerDuration = Mathf.Clamp01((Time.time - _mouseButtonReleaseTime) / _whenDeviationKicksIn); // Normalize the duration between 0 and 1, with a maximum of 5 seconds
        return _maxDeviationAngle * holdTriggerDuration;
    }

    private void Shoot()
    {
        if (CanFire())
        {
            // Calculate Deviation during the shooting
            float deviation = CalculateDeviation();

            Quaternion bulletRotation = _bulletSpawnPoint.rotation;     // Apply deviation to the bullet's rotation
            float randomAngle = Random.Range(-deviation, deviation);    // Randomize the deviation angle

            bulletRotation *= Quaternion.Euler(0f, 0f, randomAngle);    // Apply rotation around the Z-axis

            /* Play FIre Sound */
            if (_fireSound != null) { _audioSource.PlayOneShot(_fireSound); }
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, bulletRotation);
            Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();
            _ammoCounterScript.DecreaseAmmo();                          //Call the Decrease Ammo function from the AmmoCounter script;

            // set shooting animation
            //_animator.SetBool("Firing", true);


            _nextFireTime = Time.time + _firerate;
            if (Input.GetKey(KeyCode.Space))
            {
                duration = 0;
                amount = 0;
            }
            else if (!Input.GetKeyUp(KeyCode.Space))
            {
                duration = 0.05f;
                amount = 0.08f;
            }

        }
    }

    IEnumerator Reload()
    {
        _isReloading = true;
        // Play reload animation

        int bulletsLeftToFullMag = _maximumBulletCount - _currentBulletCount;
        if (bulletsLeftToFullMag > 0)
        {
            _audioSource.PlayOneShot(_reloadSound);

            if (bulletsLeftToFullMag <= _currentBulletCount)
            {
                _currentBulletCount += bulletsLeftToFullMag;
            }
            else
            {
                _currentBulletCount = _maximumBulletCount;
            }

        }
        yield return new WaitForSeconds(_reloadTime);
        _isReloading = false;
    }

    private void SpawnBulletCasing()
    {
        // Instantiate a bullet casing at the specified spawn point
        Quaternion casingRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
        Instantiate(_bulletCasingPrefab, _casingSpawnPosition.position, casingRotation);
    }

    private void SetIsGamePaused(bool isGamePaused)
    {
        _IsGamePaused = isGamePaused;
    }

    private void SetIsPlayerDead(bool playerDeadStatus)
    {
        _isPlayerDead = playerDeadStatus;
    }

    private void SetIsArmed(bool isArmedStatus)
    {
        _isArmed = isArmedStatus;
    }
    #endregion

    #endregion
}