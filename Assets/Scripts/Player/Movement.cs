using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
	// Variables
	/* Player Movement Variables */
	[SerializeField] private float _playerSpeed; //How fast the player move
	[SerializeField] private float _playerDeceleration; //How fast the player stop moving

	[SerializeField] private int _playerJumpCount; // Limit to how many jump before the player touch wall and ground
	[SerializeField] private float _playerJumpForce; //How high the player can jump
	[SerializeField] private float _playerMaxFallSpeed;
	[SerializeField] private float _fallingGravityScaleHoldS;

	[SerializeField] private float _playerDashForce;
	[SerializeField] private int _dashCooldown; //Cooldown timer before can dash again
	int _lastDashTime;
	bool _canDash;

	/* Player face direction */
	[SerializeField] private GameObject _player;
	private Vector2 _mousePosition;


	/* Physic setup */
	private Rigidbody2D _rb2D;
	private bool _isMoving;
	private float _initialGravityScale;
	[SerializeField] private float _gravityMultiplier; //How fast the player fall


	/* Ground Check */
	private bool _isGrounded = true;
	[SerializeField] private LayerMask _groundLayer;        // Layer for the ground
	[SerializeField] private Transform _groundCheckPoint;   // Transform to cast the ground check ray from
	[SerializeField] private float _groundCheckDistance;    // Distance for ground check ray

	/* Wall Check */
	private bool _isWalled = true;
	[SerializeField] private LayerMask _wallLayer;        // Layer for the ground
	[SerializeField] private Transform _wallCheckPoint;   // Transform to cast the ground check ray from
	[SerializeField] private float _wallCheckDistance;    // Distance for ground check ray


	//Functions
	void Start()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		_initialGravityScale = _rb2D.gravityScale;
	}

	void Update()
	{
		PlayerMovement();
		CanDashNow();
		PlayerDash();
		GroundCheck();
		WallCheck();
		FallSpeed();
		PlayerFaceDirection();

	}

	/* Player Face Direction */

	void PlayerFaceDirection()
	{
		/* Get Mouse Position */
		_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		/* Calculate the direction of the mouse from the player */

		Vector2 direction = (_mousePosition - (Vector2)transform.position).normalized;

		/* Calculate the angle of the mouse*/
		float _mouseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		/* Limit the _mouseAngle to between -180 and 180 */
		_mouseAngle = Mathf.Clamp(_mouseAngle, -180f, 180f);

		//Debug.Log("Mouse Angle: " + _mouseAngle);
		//_player.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _mouseAngle));
		if (_mousePosition.x > transform.position.x)
		{
			_player.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			_player.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	/*Player Movement*/
	void PlayerMovement()
	{

		/* Horizontal Movement */
		if (Input.GetKey(KeyCode.A))
		{
			_rb2D.velocity = new Vector2(-_playerSpeed, _rb2D.velocity.y);
			_isMoving = true;
		}
		if (Input.GetKey(KeyCode.D))
		{
			_rb2D.velocity = new Vector2(_playerSpeed, _rb2D.velocity.y);
			_isMoving = true;

		}
		else
		{
			/* Movement Deceleration */
			if (_isMoving == true)
			{
				_rb2D.velocity = new Vector2(Mathf.Lerp(_rb2D.velocity.x, 0f, _playerDeceleration * Time.deltaTime), _rb2D.velocity.y);
				if (_rb2D.velocity.x < 0.2f && _rb2D.velocity.x > -0.2f)
				{
					_rb2D.velocity = new Vector2(0f, _rb2D.velocity.y);
					_isMoving = false;
				}
			}
		}
		/* x2 Jump */
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (_playerJumpCount < 2)
			{
				_rb2D.AddForce(Vector2.up * _playerJumpForce, ForceMode2D.Impulse);
				_playerJumpCount++; //Add 1 to the jump counter
			}
		}

	}

	/*Player Dash*/
	void PlayerDash()
	{
		float _dashDirection = 0f;

		/* Directional Dash */
		if (Input.GetKey(KeyCode.A))
		{
			_dashDirection = -1f;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			_dashDirection = 1f;
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
		{
			/* Apply Dash Force */
			_rb2D.velocity = new Vector2(_playerDashForce * _dashDirection, _rb2D.velocity.y);
			_lastDashTime = _dashCooldown;
			// Set _canDash to false
			_canDash = false;
		}
	}
	void CanDashNow()
	{
		if (!_canDash)
		{
			_lastDashTime -= 1;
			if (_lastDashTime <= 0)
			{
				Debug.Log(_lastDashTime);
				_canDash = true;
			}
		}
	}


	/* Ground Check */
	private void GroundCheck()
	{
		_isGrounded = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, _groundCheckDistance, _groundLayer);
		if (_isGrounded)
		{
			/* Reset Gravity Mutipler to 1 by default  */
			_gravityMultiplier = 1;
			_playerJumpForce = 5;
			_playerJumpCount = 0;
		}
	}

	/* Wall Check */
	private void WallCheck()
	{
		_isWalled = Physics2D.Raycast(_wallCheckPoint.position, Vector2.right, _wallCheckDistance, _wallLayer) ||
					Physics2D.Raycast(_wallCheckPoint.position, Vector2.left, _wallCheckDistance, _wallLayer);
		if (_isWalled)
		{
			if (_playerJumpCount == 1)
			{
				_playerJumpForce = 10;
			}
			_playerJumpCount = 1;
		}
	}



	/* Fall Speed */
	void FallSpeed()
	{
		if (!_isGrounded)
		{
			//// Make the player falls faster after double jump
			//if (_playerJumpCount == 2)
			//{
			//	_gravityMultiplier = 5;
			//}

			// Fall faster when above ground at certain height

			// Fall faster the higher you are compare to the ground
			/* If the player velocity is above 0 then the gravity mutiplyer will increase by 1 every second the player above ground */
			//if (_rb2D.velocity.y > 20)
			//{
			//	_gravityMultiplier += 1;
			//	_rb2D.gravityScale = _initialGravityScale * _gravityMultiplier;

			//}

			// Fall faster when actively holding S
			if (Input.GetKey(KeyCode.S))
			{
				// Increase gravity when holding "S" to make the player fall faster
				_rb2D.gravityScale = _initialGravityScale * _gravityMultiplier * _fallingGravityScaleHoldS;

				// Limit the fall speed
				if (_rb2D.velocity.y < -_playerMaxFallSpeed)
				{
					_rb2D.velocity = new Vector2(_rb2D.velocity.x, -_playerMaxFallSpeed);
				}
			}
			else
			{
				// Reset gravity to its initial value when not holding "S"
				_rb2D.gravityScale = _initialGravityScale * _gravityMultiplier;
			}
		}
		else
		{
			// Reset gravity to its initial value when grounded
			_rb2D.gravityScale = _initialGravityScale * _gravityMultiplier;
		}
	}
}

