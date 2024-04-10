using NaughtyAttributes;
using ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using EnumLibrary;
using System.Collections.Generic;

public class PlayerWeaponHandling : MonoBehaviour
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
    [SerializeField] private PlayerEquipmentSO _playerEquipmentSO;
    [SerializeField] private PlayerController _playerCtrl;

    /* Reload System References */
    [Tooltip("Reference to the AmmoCounter-Script-Component of the 'AmmoCounter_Panel'-UI-Object in the UI-Canvas (to be Found in the Hierarchy -> UI/Ingame-UI_Canvas/WeaponUI_Panel")]
    [SerializeField] private AmmoCounter _ammoCounter;

    /* Gun related References*/
    [SerializeField] private Transform _projectileSpawnPos;
    [SerializeField] private GameObject _projectilePrefab;    

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
    [Space(3)]

    /* Camera Shake Settings*/
    [SerializeField] internal float _camShakeDuration = 0.05f;
    [SerializeField] internal float _camShakeAmount = 0.08f;
    [Space(5)]


    [Header("Monitoring Values")]
    /* Reload System */
    [SerializeField, ReadOnly] internal int _maximumBulletCount;
    [SerializeField, ReadOnly] internal int _currentBulletCount;

    /* Firerate Settings*/
    [SerializeField, ReadOnly] private float _firerate;

    /* Different boolian Values */
    [SerializeField, ReadOnly] private bool _isShooting;
    [SerializeField, ReadOnly] private bool _isPlayerDead;
    [SerializeField, ReadOnly] private bool _isGamePaused;
    [SerializeField, ReadOnly] private bool _isArmed;               // Checking whether the player is armed or not
    [SerializeField, ReadOnly] private bool _isFirststWeaponSelected;
    [SerializeField, ReadOnly] private bool _isSecondndWeaponSelected;
    [SerializeField, ReadOnly] private bool _isReloading = false;
    [SerializeField, ReadOnly] private bool _wasWeaponPickedUp;

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

        if (_playerEquipmentSO == null)
        {
            _playerEquipmentSO = Resources.Load("ScriptableObjects/PlayerEquipment") as PlayerEquipmentSO;
            Debug.Log($"<color=yellow>Caution!</color>: Reference for ScriptableObject 'PlayerEquipment' in Inspector of {this} was not set. So it was Set automatically.");
        }

        if (_playerCtrl == null)
        {
            _playerCtrl = GetComponent<PlayerController>();
            Debug.Log($"<color=yellow>Caution!</color>: Reference for 'PlayerController'-Component in Inspector of {this} was not set. So it was Set automatically.");
        }

        if (_ammoCounter == null)
        {
            _ammoCounter = GameObject.FindGameObjectWithTag("UIAmmoCounter").GetComponent<AmmoCounter>();
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
        //_inputReader.OnWeaponSwitch += SwitchWeapon;
        _inputReader.OnHolsteringWeaponInput += HolsterWeapon;
        _inputReader.OnFirstWeaponEquipInput += FirstWeaponEquip;
        _inputReader.OnSecondWeaponEquipInput += SecondWeaponEquip;
        PlayerHealth.OnPlayerDeath += SetIsPlayerDead;
        PauseMenu.OnTogglePauseScene += SetIsGamePaused;
    }
    private void OnDisable()
    {
        //_inputReader.OnWeaponSwitch -= SwitchWeapon;
        _inputReader.OnHolsteringWeaponInput -= HolsterWeapon;
        _inputReader.OnFirstWeaponEquipInput -= FirstWeaponEquip;
        _inputReader.OnSecondWeaponEquipInput -= SecondWeaponEquip;
        PlayerHealth.OnPlayerDeath -= SetIsPlayerDead;
        PauseMenu.OnTogglePauseScene -= SetIsGamePaused;
    }

    private void Start()
    {
        // Set is Armed Status
        //SetIsArmed(/*input 'loaded' value from ScriptableObjec*/); // -> intention is to 'load' isArmed-Data on Scene Start accordingly to the value when player left last scene

        SetBulletCount(_playerEquipmentSO.FirstWeapon);     // todo: maybe delete this expression later if useless (JM, 10.04.2024)
    }

    private void Update()
    {
        //DrawOrHolsterWeapon();
        if (_isArmed && !_isPlayerDead)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _mouseButtonReleaseTime = Time.time; // Record the time when the mouse button was released
                Debug.Log($"Mousbutton was released.");
            }
            else if (Input.GetMouseButton(0) && CanFire() && _currentBulletCount > 0 && _isReloading == false)
            {
                _isShooting = true;
                //_animator.SetBool("Firing", _isShooting);
                Shoot();
                _currentBulletCount--;
                SpawnBulletCasing();
                cameraShake.StartShake(_camShakeDuration, _camShakeAmount);
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
                _ammoCounter.Reload();
                StartCoroutine(Reload());
            }
        }

        if (_wasWeaponPickedUp && !_playerCtrl.IsPlayerMoving) // ensuring that animation will truely be set correctly even in case player movement stopped right on collision
        {
            EquipWeaponAnimation(_isArmed, _playerEquipmentSO.FirstWeapon.WeaponType);
            _wasWeaponPickedUp = false;
        }
    }

    /// <summary>
    /// CollisionCheck for recognizing collision with WeaponObject
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is a of Type Baseweapon, if so trigger pickup weapon
        if (collision.gameObject.TryGetComponent(out BaseWeapon weapon))
        {
            // set armed boolians
            SetWeaponEquipBools(true, true, false);

            // set animation
            _animatorCtrl.SetBool("Armed", _isArmed);

            // call weapon pickup
            _playerEquipmentSO.WeaponPickup(weapon.WeaponType);

            SetWeaponRespectiveValues(_playerEquipmentSO.FirstWeapon);

            // Set Animation            
            EquipWeaponAnimation(_isArmed, _playerEquipmentSO.FirstWeapon.WeaponType);

            Destroy(weapon.gameObject);

            _wasWeaponPickedUp = true;
        }
    }
    #endregion


    #region Custom Methods
    private void FirstWeaponEquip()
    {
        // only equip first weapon if slot is not empty
        if (_playerEquipmentSO.FirstWeapon.WeaponType != Enum_Lib.EWeaponType.Blank)
        {
            SetWeaponEquipBools(true, true, false);

            _animatorCtrl.SetBool("Armed", _isArmed);

            SetWeaponRespectiveValues(_playerEquipmentSO.FirstWeapon);

            EquipWeaponAnimation(_isArmed, _playerEquipmentSO.FirstWeapon.WeaponType);
        }
    }

    private void SecondWeaponEquip()
    {
        // only equip second weapon if slot is not empty
        if (_playerEquipmentSO.SecondWeapon.WeaponType != Enum_Lib.EWeaponType.Blank)
        {
            SetWeaponEquipBools(true, false, true);

            _animatorCtrl.SetBool("Armed", _isArmed);

            SetWeaponRespectiveValues(_playerEquipmentSO.SecondWeapon);

            EquipWeaponAnimation(_isArmed, _playerEquipmentSO.SecondWeapon.WeaponType);
        }
    }

    private void HolsterWeapon()
    {
        _isArmed = false;
        //_gun.SetActive(_isArmed);
        _animatorCtrl.SetBool("Armed", _isArmed);

        //if (!_playerCtrl.IsPlayerMoving && !_isArmed)
        //    _animatorCtrl.SetBool("Armed", false);

        EquipWeaponAnimation(_isArmed, _playerEquipmentSO.FirstWeapon.WeaponType);

        #region OldHoangApproach
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    _isArmed = !_isArmed;
        //    _gun.SetActive(_isArmed);
        //    _animatorCtrl.SetBool("Armed", _isArmed);

        //    if (!_playerCtrl.IsPlayerMoving && !_isArmed)
        //        _animatorCtrl.SetBool("Armed", false);

        //    EquipWeaponAnimation(_isArmed);
        //}
        #endregion
    }

    private void SwitchWeapon()
    {
        // only enabling weapon swap, if _isArmed and if both weapon slots actually contain weapons
        if (_isArmed && (_playerEquipmentSO.FirstWeapon.WeaponType != Enum_Lib.EWeaponType.Blank && _playerEquipmentSO.SecondWeapon.WeaponType != Enum_Lib.EWeaponType.Blank))
        {
            // Update UI


            // Update PlayerEquipmentSO
            _playerEquipmentSO.SwitchWeapon();


            // Enable proper Animation (if Player is armed) accordingly to equipped weapon
            EquipWeaponAnimation(_isArmed, _playerEquipmentSO.FirstWeapon.WeaponType);
        }
    }

    private void EquipWeaponAnimation(bool playAnimation, Enum_Lib.EWeaponType weaponType)
    {
        Debug.Log($"'<color=yellow>EquipWeaponANimation() was called</color>'.");
        if (playAnimation)
        {
            switch (weaponType)
            {
                case Enum_Lib.EWeaponType.Handcannon:
                    Debug.Log($"'<color=yellow>Entered Case for Handgun</color>'.");
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
                    Debug.Log($"'<color=yellow>Entered Case for SMG</color>'.");
                    SetAnimation("SMG");
                    break;

                case Enum_Lib.EWeaponType.Shotgun:
                    Debug.Log($"'<color=yellow>Entered Case for Shotgun</color>'.");
                    SetAnimation("Shotgun");
                    break;

                case Enum_Lib.EWeaponType.EnergyLauncher:
                    Debug.Log($"'<color=yellow>Entered Case for ELauncher</color>'.");
                    SetAnimation("Launcher");
                    break;

                case Enum_Lib.EWeaponType.Blank:
                    Debug.Log($"'<color=yellow>Entered Case for Blank</color>'.");
                    _animatorCtrl.SetBool("Armed", false);
                    _isArmed = false;
                    break;

                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nameOfWeapon"></param>
    private void SetAnimation(string nameOfWeapon)
    {
        if (_playerCtrl.IsPlayerMoving) // Set Animation parameter for Moving animation with weapon
        {
            _animatorCtrl.SetTrigger($"{nameOfWeapon}_Walk");
            Debug.Log($"'<color=yellow>SetAnimation() was called</color>' -> '<color=yellow>walking weapon animation</color>' should have been set up.");
        }
        else                            // set animation parameter for idle with weapon
        {
            _animatorCtrl.SetTrigger($"{nameOfWeapon}_Idle");
            Debug.Log($"'<color=yellow>SetAnimation() was called</color>' -> '<color=yellow>idle weapon animation</color>' should have been set up.");
        }

    }

    /// <summary>
    /// Set the boolians <see cref="_isArmed"/>, <see cref="_isFirststWeaponSelected"/> and <see cref="_isSecondndWeaponSelected"/>.
    /// </summary>
    /// <param name="armedStatus">is the player armed?</param>
    /// <param name="firstWeaponEquip">is the first weapon selected?</param>
    /// <param name="secondWeaponEquip">is the second weapon selected?</param>
    private void SetWeaponEquipBools(bool armedStatus, bool firstWeaponEquip, bool secondWeaponEquip)
    {
        _isArmed = armedStatus;
        _isFirststWeaponSelected = firstWeaponEquip;
        _isSecondndWeaponSelected = secondWeaponEquip;
    }

    /// <summary>
    /// Set the values (<see cref="_maximumBulletCount"/>, <see cref="_currentBulletCount"/>, <see cref="_firerate"/>, <see cref="BaseBullet.ProjectileDamage"/>) respective to the 
    /// Values of the Weapon currently actively selected by the Player.
    /// </summary>
    /// <param name="weaponSlot">The First or Second Weapon of <see cref="PlayerEquipmentSO"/></param>
    private void SetWeaponRespectiveValues(BaseWeapon weaponSlot)
    {
        SetBulletCount(weaponSlot);
        _firerate = weaponSlot.FireRate;
        _projectilePrefab.GetComponent<BaseBullet>().ProjectileDamage = weaponSlot.WeaponDamage;    // Damage
    }

    /// <summary>
    /// Sets the <see cref="_currentBulletCount"/> to <see cref="_playerEquipmentSO.FirstWeapon.MagazineSize"/> if Magazine is full or to <see cref="_playerEquipmentSO.FirstWeapon.CurrentRoundsInMag"/> if Magazine is not full and was not reloaded.
    /// </summary>
    /// /// <param name="weaponSlot"> The First or Second Weapon </param>
    private void SetBulletCount(BaseWeapon weaponSlot)
    {
        // Set MaxBullet Count to Magazine Size of currently held weapon (note, if player holds no weapon this will be '0')
        _maximumBulletCount = weaponSlot.MagazineSize;

        if (weaponSlot.CurrentRoundsInMag == _maximumBulletCount) // if amount of current bullets in weapons magazine equals maximum bullet count than set _currentBullet count to max bullet count
        {
            _currentBulletCount = _maximumBulletCount;
        }
        else // set current bulet count to the rounds currently in the mag of the weapon in hand
        {
            _currentBulletCount = weaponSlot.CurrentRoundsInMag;
        }

        // Set UI-AmmoCounter to amount of current bullets
        _ammoCounter.CurrentAmmo = _currentBulletCount;
        _ammoCounter.SetUIAmmoToActiveWeaponAmmo();
    }

    private bool CanFire()
    {
        return Time.time > _nextFireTime && !_isGamePaused;
    }

    private float CalculateDeviation()
    {
        float holdTriggerDuration = Mathf.Clamp01((Time.time - _mouseButtonReleaseTime) / _whenDeviationKicksIn); // Normalize the duration between 0 and 1, with a maximum of 5 seconds
        return _maxDeviationAngle * holdTriggerDuration;
    }

    private void Shoot()
    {
        if (CanFire() && _isArmed)
        {
            // 1. Play Fire Sound
            if (_fireSound != null)
                _audioSource.PlayOneShot(_fireSound);

            // 2. Instantiate Projectile (respective to 'SpawnedBullet'-Value of the First or Second Weapon)
            if (_isFirststWeaponSelected)
                InstatiateProjectiles(_playerEquipmentSO.FirstWeapon);
            else
                InstatiateProjectiles(_playerEquipmentSO.SecondWeapon);

            // 3. set shooting animation
            //_animator.SetBool("Firing", true);

            // 4. decrease The 'CurrentAmmo'-Value of the AmmoCounter and Set the Ammo-UI respectively
            _ammoCounter.DecreaseAmmo();                          //Call the Decrease Ammo function from the AmmoCounter script;

            // 5. reset the Time the 'Shoot()' can be executed the next time (to ensure the projectiles will be spawned at a specific rate ('fireRate') and not simultaneosly)
            _nextFireTime = Time.time + _firerate;

            // 6. apply Camera Shake (when sprinting while shooting)
            if (Input.GetKey(KeyCode.Space))
            {
                _camShakeDuration = 0;
                _camShakeAmount = 0;
            }
            else if (!Input.GetKeyUp(KeyCode.Space))
            {
                _camShakeDuration = 0.05f;
                _camShakeAmount = 0.08f;
            }
        }
    }

    /// <summary>
    /// Instatiates the amount of projectiles the transmitted 'BaseWeapon-Object' specifies in it's <see cref="BaseWeapon.SpawnedBullets"/> Variable.
    /// </summary>
    /// <param name="weaponSlot">The First or Second Weapon</param>
    private void InstatiateProjectiles(BaseWeapon weaponSlot)
    {
        //// 1. ensure that the Projectiles won't be spawned on the same spot
        //if (weaponSlot.SpawnedBullets > 1)
        //{
        //    float rndY = 0;
        //    List<Vector3> spawnpositionCacheList = new List<Vector3>(weaponSlot.SpawnedBullets);

        //    // creates a random spawn possition for the projectiles and repeats the random calculation if it equals a already cached spawn position
        //    for (int i = 0; i < weaponSlot.SpawnedBullets; i++)
        //    {
        //        // generate random spawn position
        //        rndY = (_projectilePrefab.transform.localScale.y * 0.5f) + Random.Range(-0.5f, 0.5f);
        //        _projectileSpawnPos.position = new Vector2(_projectileSpawnPos.position.x, _projectileSpawnPos.position.y + rndY);
        //        spawnpositionCacheList.Add(_projectileSpawnPos.position);


        //        //while (_projectileSpawnPos.position.y == spawnpositionCacheList[i].y)
        //        //{
        //        //    spawnpositionCacheList.RemoveAt(i);

        //        //    rndY += (_projectilePrefab.transform.localScale.y * 0.5f) + Random.Range(-0.5f, 0.5f);
        //        //    _projectileSpawnPos.position = new Vector2(_projectileSpawnPos.position.x, _projectileSpawnPos.position.y + rndY);
        //        //    spawnpositionCacheList.Insert(i, _projectileSpawnPos.position);
        //        //}
        //    }

        //    for (int i = 0; i < weaponSlot.SpawnedBullets; i++)
        //    {
        //        // 2. instantiate (spawn) the projectiles
        //        GameObject bullet = Instantiate(_projectilePrefab, spawnpositionCacheList[i], GetProjectileRotation());
        //        Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();
        //    }
        //}
        //else
        //{
            // 2. instantiate (spawn) the projectiles
            for (int i = 0; i < weaponSlot.SpawnedBullets; i++)
            {
                GameObject bullet = Instantiate(_projectilePrefab, _projectileSpawnPos.position, GetProjectileRotation());
                Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();
            }
        //}
    }

    /// <summary>
    /// Returns the ProjectileRotation already with an applied deviation.
    /// </summary>
    /// <returns></returns>
    private Quaternion GetProjectileRotation()
    {
        // Calculate Deviation during the shooting
        float deviation = CalculateDeviation();

        Quaternion projectileRotation = _projectileSpawnPos.rotation;     // Apply deviation to the bullet's rotation
        float randomAngle = Random.Range(-deviation, deviation);        // Randomize the deviation angle

        projectileRotation *= Quaternion.Euler(0f, 0f, randomAngle);    // Apply rotation around the Z-axis
        return projectileRotation;
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
        _isGamePaused = isGamePaused;
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