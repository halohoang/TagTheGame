using EnumLibrary;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    #region Events
    //--------------------------------
    // - - - - -  E V E N T S  - - - - 
    //--------------------------------

    
    #endregion

    #region Variables
    //--------------------------------------
    // - - - - -  V A R I A B L E S  - - - - 
    //--------------------------------------
    [Header("References to Resources")]
    [Space(2)]
    [SerializeField] private InputReaderSO _inputReaderSO;
    [SerializeField] private PlayerEquipmentSO _playerEquipmentSO;
    [SerializeField] private Transform _playerTransform;        // Reference to the player's transform.	
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Light2D _light2D;
    [SerializeField] private Sandevistan _sandevistan;
    [SerializeField] private Animator _animCtrl;
    [Space(5)]


    /* Player Movement Variables */
    [Header("Movement Settings")]
    [SerializeField] private float _currentPlayerSpeed;         // How fast the player move
    [SerializeField] private float _minPlayerSpeed;             // slowest the player move
    [SerializeField] private float _maxPlayerSpeed;             // fastest the player move
    [Space(3)]
    [SerializeField] private float _playerDashForce;
    [SerializeField] private float _dashCooldown;
    [Space(5)]


    [Header("Monitoring Values")]
    [SerializeField, ReadOnly] private float _currentDashCooldown;
    [SerializeField, ReadOnly] private Vector2 _movementDirection;
    [SerializeField, ReadOnly] private bool _isPlayerMoving;
    [SerializeField, ReadOnly] private bool _isPlayerDead;

    // Properties
    public bool IsPlayerMoving { get => _isPlayerMoving; private set => _isPlayerMoving = value; }
    #endregion


    #region Methods
    //------------------------------------
    // - - - - -  M E T H O D S  - - - - -
    //------------------------------------

    #region Unity-Provided Methods
    //--------------------------
    // - - - Unity-Methods - - -
    //--------------------------
    private void Awake()
    {
        // Auto Referencing
        if (_light2D == null)
            _light2D = GetComponentInChildren<Light2D>();

        if (_playerStats == null)
            _playerStats = GetComponent<PlayerStats>();

        if (_rigidbody2D == null)
            _rigidbody2D = GetComponent<Rigidbody2D>();

        if (_playerEquipmentSO == null)
            _playerEquipmentSO = Resources.Load("ScriptableObjects/PlayerEquipment") as PlayerEquipmentSO;

        if (_animCtrl == null)
            _animCtrl = GetComponent<Animator>();


        if (_inputReaderSO == null)
        {
            _inputReaderSO = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;
            Debug.Log($"<color=yellow>Caution!</color>: Reference for InputReader in Inspector of {this} was not set. So it was Set automatically, if you want or need to set a specific " +
                $"InputReader Asset, set it manually instead.");
        }
    }

    private void OnEnable()
    {
        PauseMenu.OnRestartScene += CallOnEnableOfInputReader;
        _inputReaderSO.OnMovementInput += ReadMovementInput;
        _inputReaderSO.OnFastMovementInput += ReadSprintInput;
        PlayerHealth.OnPlayerDeath += SetIsPlayerDead;
        PlayerHealth.OnPlayerDeath += DisablePlayerInput;
        Debug.Log($"<color=magenta> OnEnable() was called in {this} </color>");
    }
    private void OnDisable()
    {
        PauseMenu.OnRestartScene -= CallOnEnableOfInputReader;
        _inputReaderSO.OnMovementInput -= ReadMovementInput;
        _inputReaderSO.OnFastMovementInput -= ReadSprintInput;
        PlayerHealth.OnPlayerDeath -= SetIsPlayerDead;
        PlayerHealth.OnPlayerDeath -= DisablePlayerInput;
        Debug.Log($"<color=magenta> OnDisable() was called in {this} </color>");
    }

    void Start()
    {
        _currentPlayerSpeed = _minPlayerSpeed;
    }

    void Update()
    {
        RotateToMousePosition();
    }

    void FixedUpdate()
    {
        PlayerMovement();        
        //PlayerFast();
        //PlayerDash();
    }
    #endregion


    #region Custom Methods
    //---------------------------
    // - - - Custom Methods - - -
    //---------------------------
    private void ReadMovementInput(Vector2 velocity)
    {
        _movementDirection = velocity;
        Debug.Log($"<color=magenta> ReadMovementInput was called </color>");
    }

    /// <summary>
    /// Takes the a Value of the Enum 'ESpace' as param and respective executes Logic for enabling fast movement 
    /// (if SpaceKey is currently pressed) or not (is spacekey is currently not pressed).
    /// </summary>
    /// <param name="spacePressedStatus"></param>
    private void ReadSprintInput(Enum_Lib.ESpaceKey spacePressedStatus)
    {
        switch (spacePressedStatus)
        {
            case Enum_Lib.ESpaceKey.Pressed:

                // Player can move faster
                SetPlayerMovementValues(_maxPlayerSpeed, 0.1f, 1.6f, 2, false);

                #region original Settings Code
                //_currentPlayerSpeed = _maxPlayerSpeed;
                //_light2D.falloffIntensity = 0.1f;
                //_light2D.pointLightOuterRadius = 1.6f;
                //_light2D.intensity = 2;
                //_light2D.GetComponent<LightControl>().enabled = false;
                #endregion

                break;

            case Enum_Lib.ESpaceKey.NotPressed:

                // reset Values to normal Movement Speed
                SetPlayerMovementValues(_minPlayerSpeed, 0.148f, 1.12f, 1, true);

                #region original Settings Code
                //_currentPlayerSpeed = _minPlayerSpeed;
                //_light2D.falloffIntensity = 0.148f;
                //_light2D.pointLightOuterRadius = 1.12f;
                //_light2D.intensity = 1;
                //_light2D.GetComponent<LightControl>().enabled = true;
                #endregion

                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Rotates the player face towards the current position of the mouse cursor
    /// </summary>
    private void RotateToMousePosition()
    {
        if (!_isPlayerDead)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - _playerTransform.position).normalized; // Use player's position.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rigidbody2D.rotation = angle;
        }
    }

    /* Player Movement*/
    void PlayerMovement()
    {
        // if Movement registered (by MovementInput and TimeScale is not paused and Player alive
        if (_movementDirection != Vector2.zero && Time.timeScale != 0 && !_playerStats.IsPlayerDead)
        {
            IsPlayerMoving = true;

            if (_playerStats.CurrentHealth < 2)     // if Player's health is below 2, use Movement Settings as if is not sprinting
                SetPlayerMovementValues(_minPlayerSpeed, 0.148f, 1.12f, 1, true);

            _rigidbody2D.MovePosition(_rigidbody2D.position + _movementDirection * _currentPlayerSpeed * Time.deltaTime);
            #region Debuggers little helper
            //Debug.Log($"<color=magenta> PlayerMovement should have been excuted </color>. MovementDirection: '{_movementDirection}' | TimeScale: '{Time.timeScale}' | " +
            //$"Is Player Dead: '{_playerHealthScript.IsPlayerDead}'");
            #endregion
        }
        else if (_playerStats.IsPlayerDead)
        {
            _rigidbody2D.velocity = Vector2.zero;
            IsPlayerMoving = false;
        }
        else
        {
            IsPlayerMoving = false;
            #region Debuggers little helper
            //Debug.Log($"<color=magenta> PlayerMovement should NOT have been excuted </color>." +
            //$" MovementDirection: '{_movementDirection}' | TimeScale: '{Time.timeScale}' | Is Player Dead: '{_playerHealthScript.IsPlayerDead}'");
            #endregion
        }

        // Set Animation
        _animCtrl.SetBool("isMoving", IsPlayerMoving);
    }

    private void SetPlayerMovementValues(float movementSpeed, float lightFallofIntensity, float pointLightOuterRadius, float lightIntesity, bool enableDisableLightControllComponent)
    {
        _currentPlayerSpeed = movementSpeed;
        _light2D.falloffIntensity = lightFallofIntensity;
        _light2D.pointLightOuterRadius = pointLightOuterRadius;
        _light2D.intensity = lightIntesity;
        _light2D.GetComponent<LightControl>().enabled = enableDisableLightControllComponent;
    }

    #region currently not used
    /* Player Sandevistan */
    void PlayerFast()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Check if health is greater than or equal to 2
        {
            // Player can move faster
            _currentPlayerSpeed = _maxPlayerSpeed;
            _light2D.falloffIntensity = 0.1f;
            _light2D.pointLightOuterRadius = 1.6f;
            _light2D.intensity = 2;
            _light2D.GetComponent<LightControl>().enabled = false;

        }
        if (_playerStats.CurrentHealth < 2 || Input.GetKeyUp(KeyCode.Space))
        {
            // Player's health is below 2, so they can't use "Space" for increased speed
            _currentPlayerSpeed = _minPlayerSpeed;
            _light2D.falloffIntensity = 0.148f;
            _light2D.pointLightOuterRadius = 1.12f;
            _light2D.intensity = 1;
            _light2D.GetComponent<LightControl>().enabled = true;
        }
    }

    /* Player Dash */
    void PlayerDash()
    {
        if (_currentDashCooldown <= 0)
        {

            if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                //StartCoroutine(_sandevistan.Trail());
                /*Dash direction to the left */
                Vector2 dashDirection = Vector2.left;
                /* Apply Dash Force */
                Vector2 dashForce = dashDirection * _playerDashForce * Time.deltaTime;
                transform.Translate(dashForce, Space.World);
                _currentDashCooldown = _dashCooldown;
            }
            if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                //StartCoroutine(_sandevistan.Trail());
                /*Dash direction to the left */
                Vector2 dashDirection = Vector2.right;
                /* Apply Dash Force */
                Vector2 dashForce = dashDirection * _playerDashForce * Time.deltaTime;
                transform.Translate(dashForce, Space.World);
                _currentDashCooldown = _dashCooldown;

            }
            if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                //StartCoroutine(_sandevistan.Trail());
                /*Dash direction to the left */
                Vector2 dashDirection = Vector2.up;
                /* Apply Dash Force */
                Vector2 dashForce = dashDirection * _playerDashForce * 0.5f * Time.deltaTime;
                transform.Translate(dashForce, Space.World);

                _currentDashCooldown = _dashCooldown;
            }

            if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                //StartCoroutine(_sandevistan.Trail());
                /*Dash direction to the left */
                Vector2 dashDirection = Vector2.down;
                /* Apply Dash Force */
                Vector2 dashForce = dashDirection * _playerDashForce * 0.5f * Time.deltaTime;
                transform.Translate(dashForce, Space.World);
                _currentDashCooldown = _dashCooldown;
            }
        }
        else
        {
            _currentDashCooldown -= Time.deltaTime;
        }
    } //Not currently used
    #endregion

    private void CallOnEnableOfInputReader()
    {
        _inputReaderSO.OnEnable();
    }

    private void EnablePlayerInput()
    {
        _inputReaderSO.GameInput.Player.Enable();
    }
    private void DisablePlayerInput(bool arg0)
    {
        _inputReaderSO.GameInput.Player.Disable();
    }

    private void SetIsPlayerDead(bool isPlayerDead)
    {
        _isPlayerDead = isPlayerDead;
    }
    #endregion

    #endregion
}