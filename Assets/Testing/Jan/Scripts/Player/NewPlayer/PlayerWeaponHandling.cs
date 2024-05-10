using NaughtyAttributes;
using ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using EnumLibrary;

public class PlayerWeaponHandling : MonoBehaviour
{
    #region Events
    //--------------------------------
    // - - - - -  E V E N T S  - - - - 
    //--------------------------------

    public static event UnityAction<bool, Vector3, float> OnPlayerShoot;    // invoked in 'PlayerAttackOnInput()' (when projectile is instantiated); JM (08.05.24)
    public static event UnityAction<int, int> OnSetBulletCount;             // invoked in 'SetBulletCount()' (when new bullet counts are (re)setted, e.g. on weapon switch/pickup); JM (08.05.24)
    public static event UnityAction<int> OnBulletsInstantiated;             // invoked in 'SpawnProjectile()' (respective during execution of 'PlayerAttackOnInput()'); JM (08.05.24)
    public static event UnityAction<int> OnReload;                          // invoked in 'Realoding()' (respective when Reload-Input was detected); JM (08.05.24)
    public static event UnityAction<BaseWeapon> OnWeaponEquip;              // invoked in 'FirstWeaponEquip()', 'SecondWEaponEquip()', 'HolsterWeapon()' and 'OnCollisionEnter2D()'; JM (10.05.24)
    #endregion


    #region Variables
    //--------------------------------------
    // - - - - -  V A R I A B L E S  - - - - 
    //--------------------------------------

    //--- SerializedFields Variables ---
    [Header("References")]
    #region Tooltip
    [Tooltip("Reference to the Scriptable Object Asset 'InputReader' (to be found in Project Folder -> Resources/ScriptableObjects; will be referenced automatically if not set manually here)")]
    #endregion
    [SerializeField] private InputReaderSO _inputReader;
    #region Tooltip
    [Tooltip("Reference to the Scriptable Object Asset 'PlayerEquipment' (to be found in Project Folder -> Resources/ScriptableObjects; will be referenced automatically if not set manually here)")]
    #endregion
    [SerializeField] private PlayerEquipmentSO _playerEquipmentSO;
    #region Tooltip
    [Tooltip("Reference to the Player Controller Component of the Player Object (will be referenced automatically if not set manually here")]
    #endregion
    [SerializeField] private PlayerController _playerCtrl;

    /* Reload System References */
    #region Tooltip
    [Tooltip("Reference to the AmmoCounter-Script-Component of the 'AmmoCounter_Panel'-UI-Object in the UI-Canvas (to be found in the hierarchy -> UI/Ingame-UI_Canvas/WeaponUI_Panel")]
    #endregion
    [SerializeField] private AmmoCounter _ammoCounter;

    /* Gun related References*/
    #region Tooltip
    [Tooltip("The transform positon the projectile object shall spawn on instatiation.")]
    #endregion
    [SerializeField] private Transform _projectileSpawnPos;
    #region Tooltip
    [Tooltip("The prefab of the projectile object that shall be spawned.")]
    #endregion
    [SerializeField] private GameObject _projectilePrefab;

    /* Bullet Casing Spawining References*/
    #region Tooltip
    [Tooltip("The transform positon the bullet casing object shall spawn on instatiation.")]
    #endregion
    [SerializeField] private Transform _casingSpawnPosition;
    #region Tooltip
    [Tooltip("The prefab of the bullet casing object that shall be spawned.")]
    #endregion
    [SerializeField] private GameObject _bulletCasingPrefab;

    /* AudioClip References*/
    #region Tooltip
    [Tooltip("The audio clip that shall be playey on shooting a weapon. (aka the shooting sound.)")]
    #endregion
    [SerializeField] private AudioClip _shootingSoundClip;
    #region Tooltip
    [Tooltip("The audio clip that shall be playey on reloading a weapon. (aka the reload sound.)")]
    #endregion
    [SerializeField] private AudioClip _reloadSound;                // Reload sound

    /* Camera Shake References */
    #region Tooltip
    [Tooltip("The 'CameraRecoilShake'-Component, currently a component of the main camera object.")]
    #endregion
    [SerializeField] private CameraRecoilShake _cameraShake;

    /* Muzzle Flash References */
    #region Tooltip
    [Tooltip("The 'MuzzleFlashPlayer'-Object (currently a child object of the player object)")]
    #endregion
    [SerializeField] private GameObject _muzzleFlash;
    #region Tooltip
    [Tooltip("The 'Animator'-Component of the Player (will be referneced automatically if not set manually here.))")]
    #endregion
    [SerializeField] private Animator _animatorCtrl;
    [Space(5)]


    [Header("Settings")]
    /* Reload system */
    //[SerializeField] internal int _minimumBulletCount;            // Will use to do sound for low ammo  sound
    #region Tooltip
    [Tooltip("The duration of reload time.")]
    #endregion
    [SerializeField] private float _reloadTime;                     // To sync with how long the reload animation is    

    /* Firerate Settings*/
    private float _nextFireTime;

    /* Recoil Settings*/
    #region Tooltip
    [Tooltip("Maximum Angle for deviation of the direction the projecctiles will move towards when shooting.")]
    #endregion
    [SerializeField] private float _maxDeviationAngle = 5f;         // Maximum deviation the bullet will be off from the straight line
    #region Tooltip
    [Tooltip("The Time the deviation to the movement direction fot he projectile is applied (in seconds)")]
    #endregion
    [SerializeField] private float _whenDeviationKicksIn;

    /* Shooting Noise Range Settings*/
    #region Tooltip
    [Tooltip("The range (in Unity Units) the shooting shall be recognizable by enemies.")]
    #endregion
    [SerializeField, Range(0.0f, 25.0f), EnableIf("_showNoiseRangeGizmo")] private float _shootingNoiseRange = 10.0f;
    #region Tooltip
    [Tooltip("Defines whether the green gizmo circle around the player showing the noise range when shooting, is shown in the editor or not.")]
    #endregion
    [SerializeField] private bool _showNoiseRangeGizmo = true;
    [Space(3)]

    /* Camera Shake Settings*/
    #region Tooltip
    [Tooltip("Duration of appliance of the camera shake effect when shooting a gun")]
    #endregion
    [SerializeField] internal float _camShakeDuration = 0.05f;
    #region Tooltip
    [Tooltip("The amount the camera shall shake (when shooting gun)")]
    #endregion
    [SerializeField] internal float _camShakeAmount = 0.08f;
    [Space(5)]


    [Header("Monitoring Values")]
    /* Reload System */
    #region Tooltip
    [Tooltip("The maximum bullet count accordingly to the magazine size of the currently selected weapon.")]
    #endregion
    [SerializeField, ReadOnly] internal int _maximumBulletCount;
    #region Tooltip
    [Tooltip("The current amount of bullets of the currently selected weapon.")]
    #endregion
    [SerializeField, ReadOnly] internal int _currentBulletCount;

    /* Firerate Settings*/
    #region Tooltip
    [Tooltip("Determines the frequence of bullets that can be instantiated after one another. (The less the value the shorter the time frequenze of projectile spawning.)")]
    #endregion
    [SerializeField, ReadOnly] private float _fireRate;

    /* Different boolian Values */
    [SerializeField, ReadOnly] private bool _isShooting;
    [SerializeField, ReadOnly] private bool _isPlayerDead;
    [SerializeField, ReadOnly] private bool _isGamePaused;
    [SerializeField, ReadOnly] private bool _isArmed;                                           // Checking whether the player is armed or not
    [SerializeField, ReadOnly] private bool _isFirststWeaponSelected;
    [SerializeField, ReadOnly] private bool _isSecondndWeaponSelected;
    [SerializeField, ReadOnly] private bool _isReloading = false;
    [SerializeField, ReadOnly] private bool _wasWeaponPickedUp;
    [SerializeField, ReadOnly] private Enum_Lib.ELeftMouseButton _leftMouseButtonStatus;
    #region Tooltip
    [Tooltip("The time when the mouse button was released (in seconds)")]
    #endregion
    [SerializeField, ReadOnly] private float _mouseButtonReleaseTime;                          // used for calculation of deviation

    // --- private Variables ---
    private AudioSource _audioSource;
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

        //if (_ammoCounter == null)
        //{
        //    _ammoCounter = GameObject.FindGameObjectWithTag("UIAmmoCounter").GetComponent<AmmoCounter>();
        //}

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
        _inputReader.OnAttackInput += ReadAttackinput;
        _inputReader.OnAttackInputStop += ReadAttackinput;
        _inputReader.OnReloadingInput += Realoding;
        PlayerHealth.OnPlayerDeath += SetIsPlayerDead;
        PauseMenu.OnTogglePauseScene += SetIsGamePaused;
    }
    private void OnDisable()
    {
        //_inputReader.OnWeaponSwitch -= SwitchWeapon;
        _inputReader.OnHolsteringWeaponInput -= HolsterWeapon;
        _inputReader.OnFirstWeaponEquipInput -= FirstWeaponEquip;
        _inputReader.OnSecondWeaponEquipInput -= SecondWeaponEquip;
        _inputReader.OnAttackInput -= ReadAttackinput;
        _inputReader.OnAttackInputStop -= ReadAttackinput;
        _inputReader.OnReloadingInput -= Realoding;
        PlayerHealth.OnPlayerDeath -= SetIsPlayerDead;
        PauseMenu.OnTogglePauseScene -= SetIsGamePaused;
    }

    private void Start()
    {
        // reset LeftMousebutton Status to Not pressed (otherwise it might be considered as pressed initially if no Input by Player was recognized on Gamestart yet)
        _leftMouseButtonStatus = Enum_Lib.ELeftMouseButton.NotPressed;

        // Set is Armed Status
        //SetIsArmed(/*input 'loaded' value from ScriptableObjec*/); // -> intention is to 'load' isArmed-Data on Scene Start accordingly to the value when player left last scene

        //SetBulletCount(_playerEquipmentSO.FirstWeapon);     // todo: maybe delete this expression later if useless (JM, 10.04.2024)
    }

    private void Update()
    {
        if (_isArmed && !_isPlayerDead)
            PlayerAttackOnInput();

        #region old Hoangs prototype input solution
        ////DrawOrHolsterWeapon();
        //if (_isArmed && !_isPlayerDead)
        //{
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        _mouseButtonReleaseTime = Time.time; // Record the time when the mouse button was released
        //        Debug.Log($"Mousbutton was released.");
        //    }
        //    else if (Input.GetMouseButton(0) && CanFire() && _currentBulletCount > 0 && _isReloading == false)
        //    {
        //        _isShooting = true;
        //        //_animator.SetBool("Firing", _isShooting);
        //        Shoot();
        //        _currentBulletCount--;
        //        SpawnBulletCasing();
        //        cameraShake.StartShake(_camShakeDuration, _camShakeAmount);
        //        Debug.Log("Shake");

        //        OnPlayerShoot?.Invoke(_isShooting, transform.position, _shootingNoiseRange);
        //    }
        //    else
        //    {
        //        _isShooting = false;
        //        //_animator.SetBool("Firing", _isShooting);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.R) && _isReloading == false && !Input.GetMouseButton(0) && !_isPlayerDead)
        //{
        //    if (_currentBulletCount < _maximumBulletCount)
        //    {
        //        //_ammoCounter.Reload();
        //        OnReload?.Invoke(); // informing AmmoCounter about Reloading
        //        StartCoroutine(Reload());
        //    }
        //}
        #endregion

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

            OnWeaponEquip?.Invoke(_playerEquipmentSO.FirstWeapon); // fire event e.g. to inform UIManager for updating UI respectively to pickedup weapon

            _wasWeaponPickedUp = true;
        }
    }
    #endregion

    #region Custom Methods
    private void ReadAttackinput(Enum_Lib.ELeftMouseButton lMBPressStatus)
    {
        _leftMouseButtonStatus = lMBPressStatus;

        if (lMBPressStatus == Enum_Lib.ELeftMouseButton.Released)
        {
            _mouseButtonReleaseTime = Time.time; // Record the time when the mouse button was released
            Debug.Log($"Mousbutton was released.");
        }

        #region obsolete for now
        //if (_isArmed && !_isPlayerDead)
        //{
        //    // read input and execute proper logic
        //    switch (lMBPressStatus)
        //    {
        //        case Enum_Lib.ELeftMouseButton.Pressed:
        //            if (CanFire() && _isArmed && _currentBulletCount > 0 && !_isReloading)
        //            {
        //                _isShooting = true;
        //                PlayAudio(_shootingSoundClip);

        //                //set shooting animation
        //                //_animatorCtrl.SetBool("Firing", _isShooting);

        //                SpawnProjectile();

        //                SpawnBulletCasing();

        //                ExecuteCameraShake();

        //                OnPlayerShoot?.Invoke(_isShooting, transform.position, _shootingNoiseRange);

        //                // reset the Time the shooting logic can be executed the next time (e.g. to ensure the projectiles will be spawned at a specific rate ('fireRate') and not simultaneosly)
        //                _nextFireTime = Time.time + _firerate;
        //            }
        //            else
        //            {
        //                _isShooting = false;
        //                //_animatorCtrl.SetBool("Firing", _isShooting);
        //            }
        //            break;

        //        case Enum_Lib.ELeftMouseButton.NotPressed:
        //            _isShooting = false;
        //            break;

        //        case Enum_Lib.ELeftMouseButton.Released:
        //            _mouseButtonReleaseTime = Time.time; // Record the time when the mouse button was released
        //            Debug.Log($"Mousbutton was released.");
        //            break;
        //    }
        //}
        #endregion
    }

    /// <summary>
    /// Executes Attack/Shooting related Logic (e.g. shooting sound, Projectile spawning, camera shake etc.) respective to Mouse-Input.
    /// </summary>
    private void PlayerAttackOnInput()
    {
        // read input and execute proper logic
        switch (_leftMouseButtonStatus)
        {
            case Enum_Lib.ELeftMouseButton.Pressed:
                if (CanFire() && _isArmed && _currentBulletCount > 0 && !_isReloading)
                {
                    _isShooting = true;
                    PlayAudio(_shootingSoundClip);

                    //set shooting animation
                    //_animatorCtrl.SetBool("Firing", _isShooting);

                    SpawnProjectile();

                    SpawnBulletCasing();

                    ExecuteCameraShake();

                    OnPlayerShoot?.Invoke(_isShooting, transform.position, _shootingNoiseRange);

                    // reset the Time the shooting logic can be executed the next time (e.g. to ensure the projectiles will be spawned at a specific rate ('fireRate') and not simultaneosly)
                    _nextFireTime = Time.time + _fireRate;
                }
                break;

            case Enum_Lib.ELeftMouseButton.NotPressed:
                _isShooting = false;
                break;

            case Enum_Lib.ELeftMouseButton.Released:
                _isShooting = false;
                break;
        }
    }

    /// <summary>
    /// Instantiates Projectiles respective to whether the first or second weapon is selected; also decreases <see cref="_currentBulletCount"/>-Value and fires respective Event to inform 
    /// <see cref="AmmoCounter"/> to initiate an update of the Ammo-UI respective to the actual <see cref="_currentBulletCount"/>-Value.
    /// </summary>
    private void SpawnProjectile()
    {
        // Instantiate Projectiles (respective to 'SpawnedBullet'-Value of the first or second Weapon)
        if (_isFirststWeaponSelected)
            InstatiateProjectiles(_playerEquipmentSO.FirstWeapon);
        else
            InstatiateProjectiles(_playerEquipmentSO.SecondWeapon);

        _currentBulletCount--;

        // storing the current amount of rounds in mag in ScriptableObject respective to wether First or Second Weapon is Selected
        _playerEquipmentSO.UpdateRoundsInMag(
            _isFirststWeaponSelected ? Enum_Lib.ESelectedWeapon.FirstWeapon : Enum_Lib.ESelectedWeapon.SecondWeapon, _currentBulletCount);                         

        OnBulletsInstantiated?.Invoke(_currentBulletCount);                                // informing AmmoCounter about shooting with updated ammount of Bullets          
    }

    private void ExecuteCameraShake()
    {
        //apply Camera Shake(when sprinting while shooting)

        #region old Hoangs approach on setting CamShake Values
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    _camShakeDuration = 0;
        //    _camShakeAmount = 0;
        //}
        //else if (!Input.GetKeyUp(KeyCode.Space))
        //{
        //    _camShakeDuration = 0.05f;
        //    _camShakeAmount = 0.08f;
        //}
        #endregion

        if (_playerCtrl.IsPlayerSprinting)
        {
            _camShakeDuration = 0;
            _camShakeAmount = 0;
        }
        else
        {
            _camShakeDuration = 0.05f;
            _camShakeAmount = 0.08f;
        }

        _cameraShake.StartShake(_camShakeDuration, _camShakeAmount);
        Debug.Log("Shake");
    }

    private bool CanFire()
    {
        return Time.time > _nextFireTime && !_isGamePaused;
    }

    /// <summary>
    /// Plays transmitted Audioclip if it is not null
    /// </summary>
    /// <param name="clipToPlay"></param>
    private void PlayAudio(AudioClip clipToPlay)
    {
        if (clipToPlay != null)
            _audioSource.PlayOneShot(clipToPlay);
    }

    private void Realoding()
    {
        if (_isReloading == false && !_isShooting && !_isPlayerDead)
        {
            if (_currentBulletCount < _maximumBulletCount)
            {
                _audioSource.PlayOneShot(_reloadSound);
                StartCoroutine(Reload());
            }
        }
    }

    private void FirstWeaponEquip()
    {
        // only equip first weapon if slot is not empty
        if (_playerEquipmentSO.FirstWeapon.WeaponType != Enum_Lib.EWeaponType.Blank && !_isReloading)
        {
            SetWeaponEquipBools(true, true, false);

            _animatorCtrl.SetBool("Armed", _isArmed);

            SetWeaponRespectiveValues(_playerEquipmentSO.FirstWeapon);

            EquipWeaponAnimation(_isArmed, _playerEquipmentSO.FirstWeapon.WeaponType);

            OnWeaponEquip?.Invoke(_playerEquipmentSO.FirstWeapon);
        }
    }

    private void SecondWeaponEquip()
    {
        // only equip second weapon if slot is not empty and is not reloading
        if (_playerEquipmentSO.SecondWeapon.WeaponType != Enum_Lib.EWeaponType.Blank && !_isReloading)
        {
            SetWeaponEquipBools(true, false, true);

            _animatorCtrl.SetBool("Armed", _isArmed);

            SetWeaponRespectiveValues(_playerEquipmentSO.SecondWeapon);

            EquipWeaponAnimation(_isArmed, _playerEquipmentSO.SecondWeapon.WeaponType);

            OnWeaponEquip?.Invoke(_playerEquipmentSO.SecondWeapon);
        }
    }

    private void HolsterWeapon()
    {
        _isArmed = false;
        //_gun.SetActive(_isArmed);
        _animatorCtrl.SetBool("Armed", _isArmed);

        //if (!_playerCtrl.IsPlayerMoving && !_isArmed)
        //    _animatorCtrl.SetBool("Armed", false);

        //EquipWeaponAnimation(_isArmed, _playerEquipmentSO.FirstWeapon.WeaponType);

        SetWeaponRespectiveValues(_playerEquipmentSO.BlankHands);

        OnWeaponEquip?.Invoke(_playerEquipmentSO.BlankHands);

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

    /// <summary>
    /// Executes the Animation for the specific weapon types via calling the <see cref="SetAnimation()"/> respective to the transmitted parameter-values 
    /// (which determine which weapon-animation shall be played and if a animation shall be layed at all).
    /// </summary>
    /// <param name="playAnimation">shall the animation be played or not?</param>
    /// <param name="weaponType">The Weapontype spcified by Enum_Lib.EWeapnType</param>
    private void EquipWeaponAnimation(bool playAnimation, Enum_Lib.EWeaponType weaponType)
    {
        Debug.Log($"'<color=yellow>EquipWeaponAnimation() was called</color>'.");
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
    /// Set the values (<see cref="_maximumBulletCount"/>, <see cref="_currentBulletCount"/>, <see cref="_fireRate"/>, <see cref="BaseBullet.ProjectileDamage"/>) respective to the 
    /// Values of the Weapon currently actively selected by the Player. Also Fires an event to inform the <see cref="AmmoCounter"/> to update the Ammo-UI.
    /// </summary>
    /// <param name="weaponSlot">The First or Second Weapon of <see cref="PlayerEquipmentSO"/></param>
    private void SetWeaponRespectiveValues(BaseWeapon weaponSlot)
    {
        SetBulletCount(weaponSlot);
        _fireRate = weaponSlot.FireRate;
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

        if (weaponSlot.CurrentRoundsInMag == _maximumBulletCount) // if amount of current bullets in weapons magazine equals maximum bullet count than set _currentBulletCount to _maxbulletCount
        {
            _currentBulletCount = _maximumBulletCount;
        }
        else // set _currentBuletCount to the rounds currently in the mag of the weapon of the selceted weapon slot
        {
            _currentBulletCount = weaponSlot.CurrentRoundsInMag;
        }

        // Set UI-AmmoCounter to amount of current bullets
        OnSetBulletCount?.Invoke(_currentBulletCount, _maximumBulletCount); // informing ammocounter about changes of the max- and current bullet count
        //_ammoCounter.CurrentAmmo = _currentBulletCount;
        //_ammoCounter.MagazineSize = _maximumBulletCount;
        //_ammoCounter.SetUIAmmoToActiveWeaponAmmo();
    }

    private float CalculateDeviation()
    {
        float holdTriggerDuration = Mathf.Clamp01((Time.time - _mouseButtonReleaseTime) / _whenDeviationKicksIn); // Normalize the duration between 0 and 1, with a maximum of 5 seconds
        return _maxDeviationAngle * holdTriggerDuration;
    }

    /// <summary>
    /// Instatiates the amount of projectiles the transmitted 'BaseWeapon-Object' specifies in it's <see cref="BaseWeapon.SpawnedBullets"/> Variable.
    /// </summary>
    /// <param name="weaponSlot">The First or Second Weapon</param>
    private void InstatiateProjectiles(BaseWeapon weaponSlot)
    {
        // instantiate (spawn) the projectiles
        for (int i = 0; i < weaponSlot.SpawnedBullets; i++)
        {
            /*GameObject bullet = */
            Instantiate(_projectilePrefab, _projectileSpawnPos.position, GetProjectileRotation());
            //Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();
        }
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
        float timePerBullet = 1.0f / (float)_maximumBulletCount;           // Time for each bullet to enable

        // Play reload animation

        for (int i = _currentBulletCount; i < _maximumBulletCount; i++)
        {
            _currentBulletCount++;
            yield return new WaitForSeconds(timePerBullet);
            OnReload?.Invoke(_currentBulletCount);         // informing UIManager about Reloading

            // storing the current amount of rounds in mag in ScriptableObject respective to wether First or Second Weapon is Selected
            _playerEquipmentSO.UpdateRoundsInMag(
                _isFirststWeaponSelected ? Enum_Lib.ESelectedWeapon.FirstWeapon : Enum_Lib.ESelectedWeapon.SecondWeapon, _currentBulletCount);                        
        }
        _isReloading = false;                                   // Set reloading flag to false when the reload is complete

        #region old Hoang Approach
        //int bulletsLeftToFullMag = _maximumBulletCount - _currentBulletCount;
        //if (bulletsLeftToFullMag > 0)
        //{
        //    _audioSource.PlayOneShot(_reloadSound);

        //    if (bulletsLeftToFullMag <= _currentBulletCount)
        //    {
        //        _currentBulletCount += bulletsLeftToFullMag;
        //    }
        //    else
        //    {
        //        _currentBulletCount = _maximumBulletCount;
        //    }

        //}
        //yield return new WaitForSeconds(_reloadTime);
        //_isReloading = false;
        #endregion
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
    private void Shoot()
    {
        if (CanFire() && _isArmed)
        {
            PlayAudio(_shootingSoundClip);

            // 2. Instantiate Projectiles (respective to 'SpawnedBullet'-Value of the first or second Weapon)
            if (_isFirststWeaponSelected)
                InstatiateProjectiles(_playerEquipmentSO.FirstWeapon);
            else
                InstatiateProjectiles(_playerEquipmentSO.SecondWeapon);

            // 3. set shooting animation
            //_animator.SetBool("Firing", true);

            // 4. decrease The 'CurrentAmmo'-Value of the AmmoCounter and Set the Ammo-UI respectively
            //_ammoCounter.DecreaseAmmo();                          //Call the Decrease Ammo function from the AmmoCounter script;
            //OnBulletsnstantiated?.Invoke(_currentBulletCount);                                // informing AmmoCounter about shooting            

            // 5. apply Camera Shake (when sprinting while shooting)
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
            Debug.Log($"CurrentShakeDuration: '{_camShakeDuration}' | CurrentShakeAmount: '{_camShakeAmount}'");
        }
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
    #endregion

    #endregion
}