using EnumLibrary;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Movement : MonoBehaviour
{
    // Variables
    [Header("References to Resources")]
    [SerializeField] private InputReaderSO _inputReaderSO;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [Space(5)]

    /* Player Movement Variables */
    [SerializeField] private float _currentPlayerSpeed; //How fast the player move
    [SerializeField] private float _minPlayerSpeed; //slowest the player move
    [SerializeField] private float _maxPlayerSpeed; //fastest the player move


    [SerializeField] internal float _playerDashForce;
    [SerializeField] private Sandevistan _sandevistan;
    [SerializeField] internal float _dashCooldown;
    internal float _currentDashCooldown;

    internal PlayerHealth _playerHealthScript;
    private Vector2 _movementDirection;
    private bool _isPlayerMoving;

    [SerializeField] Light2D _light2D;

    public bool IsPlayerMoving { get => _isPlayerMoving; private set => _isPlayerMoving = value; }

    private void Awake()
    {
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
        PlayerHealth.OnPlayerDeath += DisablePlayerInput;
        Debug.Log($"<color=magenta> OnEnable() was called in {this} </color>");
    }
    private void OnDisable()
    {
        PauseMenu.OnRestartScene -= CallOnEnableOfInputReader;
        _inputReaderSO.OnMovementInput -= ReadMovementInput;
        _inputReaderSO.OnFastMovementInput -= ReadSprintInput;
        PlayerHealth.OnPlayerDeath -= DisablePlayerInput;
        Debug.Log($"<color=magenta> OnDisable() was called in {this} </color>");
    }

    //Functions
    void Start()
    {
        _currentPlayerSpeed = _minPlayerSpeed;
        _light2D = GetComponentInChildren<Light2D>();
        _playerHealthScript = GetComponent<PlayerHealth>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        PlayerMovement();
        //PlayerFast();
        //PlayerDash();
    }

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

                if (_playerHealthScript._currentHealth < 2)
                    goto case Enum_Lib.ESpaceKey.NotPressed;    // Player's health is below 2, so they can't use "Space" for increased speed

                // Player can move faster
                _currentPlayerSpeed = _maxPlayerSpeed;
                _light2D.falloffIntensity = 0.1f;
                _light2D.pointLightOuterRadius = 1.6f;
                _light2D.intensity = 2;
                _light2D.GetComponent<LightControl>().enabled = false;

                break;

            case Enum_Lib.ESpaceKey.NotPressed:
                // reset Values to normal Movement Speed
                _currentPlayerSpeed = _minPlayerSpeed;
                _light2D.falloffIntensity = 0.148f;
                _light2D.pointLightOuterRadius = 1.12f;
                _light2D.intensity = 1;
                _light2D.GetComponent<LightControl>().enabled = true;

                break;

            default:
                break;
        }
    }

    /* Player Movement*/
    void PlayerMovement()
    {
        // if Movement registered (by MovementInput and TimeScale is not paused and Player alive
        if (_movementDirection != Vector2.zero && Time.timeScale != 0 && !_playerHealthScript.IsPlayerDead)
        {
            IsPlayerMoving = true;

            //PlayerFast();

            _rigidbody2D.MovePosition(_rigidbody2D.position + _movementDirection * _currentPlayerSpeed * Time.deltaTime);
            //Debug.Log($"<color=magenta> PlayerMovement should have been excuted </color>. MovementDirection: '{_movementDirection}' | TimeScale: '{Time.timeScale}' | " +
            //$"Is Player Dead: '{_playerHealthScript.IsPlayerDead}'");
        }
        else
        {
            IsPlayerMoving = false;
            //Debug.Log($"<color=magenta> PlayerMovement should NOT have been excuted </color>." +
            //$" MovementDirection: '{_movementDirection}' | TimeScale: '{Time.timeScale}' | Is Player Dead: '{_playerHealthScript.IsPlayerDead}'");
        }

        #region old Code from Hoang
        //if (_playerHealthScript._currentHealth > 0)
        //{
        //    /* Horizontal Movement */
        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        transform.Translate(Vector2.left * _currentPlayerSpeed * Time.deltaTime, Space.World);
        //    }
        //    if (Input.GetKey(KeyCode.D))
        //    {
        //        transform.Translate(Vector2.right * _currentPlayerSpeed * Time.deltaTime, Space.World);
        //    }

        //    if (Input.GetKey(KeyCode.W))
        //    {
        //        transform.Translate(Vector2.up * _currentPlayerSpeed * Time.deltaTime, Space.World);
        //    }
        //    if (Input.GetKey(KeyCode.S))
        //    {
        //        transform.Translate(Vector2.down * _currentPlayerSpeed * Time.deltaTime, Space.World);
        //    }
        //}
        #endregion       
    }

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
        if (_playerHealthScript._currentHealth < 2 || Input.GetKeyUp(KeyCode.Space))
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
}

