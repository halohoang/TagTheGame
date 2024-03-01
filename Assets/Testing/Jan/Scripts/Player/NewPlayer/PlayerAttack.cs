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
    [SerializeField] private PlayerAttack _playerAttack;

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
    [SerializeField] private Animator _animator;
    [Space(5)]


    [Header("Settings")]
    /* Reload system */
    [SerializeField] internal int _maximumBulletCount;
    [SerializeField] internal int _currentBulletCount;
    //[SerializeField] internal int _minimumBulletCount;            // Will use to do sound for low ammo  sound    
    [SerializeField] private float _reloadTime;                     // To sync with how long the reload animation is    

    /* Firerate Settings*/
    [SerializeField] private float _firerate = 0.5f;
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
        _currentBulletCount = _maximumBulletCount;
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
    }
    #endregion


    #region Custom Methods
    private void DrawOrHolsterWeapon()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isArmed = !_isArmed;
            _gun.SetActive(_isArmed);
        }
    }

    private void SwitchWeapon()
    {
        // Update UI


        // Enable Animation


        // Update PlayerEquipmentSO
        _playerEquipment.SwitchWeapon();

        switch (_playerEquipment.WeaponInHeand.WeaponType)
        {
            case Enum_Lib.EWeaponType.Handgun:
                //SetAnimation();
                    //    if (_playerCtrl.IsPlayerMoving)
                    //    {
                    //        // Set Animation parameter for Moving animation
                    //    }
                    //    else
                    //    {
                    //        // set animation  parameter for idle
                    //    }
                    break;

            case Enum_Lib.EWeaponType.SMG:
                break;

            case Enum_Lib.EWeaponType.Shotgun:
                break;

            case Enum_Lib.EWeaponType.EnergyLauncher:
                break;

            case Enum_Lib.EWeaponType.Blank:
                break;

            default:
                break;
        }

        //if (_playerEquipment.WeaponInHeand.WeaponType == Enum_Lib.EWeaponType.Handgun)
        //{
        //    if (_playerCtrl.IsPlayerMoving)
        //    {
        //        // Set Animation parameter for Moving animation
        //    }
        //    else
        //    {
        //        // set animation  parameter for idle
        //    }
        //}
        //else if (_playerEquipment.WeaponInHeand.WeaponType == Enum_Lib.EWeaponType.SMG)
        //{

        //}
        //else if (_playerEquipment.WeaponInHeand.WeaponType == Enum_Lib.EWeaponType.Shotgun)
        //{

        //}
        //else if (_playerEquipment.WeaponInHeand.WeaponType == Enum_Lib.EWeaponType.EnergyLauncher)
        //{

        //}
    }

    //private void SetAnimation(Enum_Lib.EWeaponType weaponType)
    //{
    //    if (_playerCtrl.IsPlayerMoving)
    //    {
    //        // Set Animation parameter for Moving animation with Handgun
    //    }
    //    else
    //    {
    //        // set animation parameter for idle with Handgun
    //    }
    //}

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
    #endregion

    #endregion
}