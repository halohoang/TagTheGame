using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class Movement : MonoBehaviour
{
	// Variables
	/* Player Movement Variables */
	[SerializeField] private float _currentPlayerSpeed; //How fast the player move
	[SerializeField] private float _minPlayerSpeed; //slowest the player move
	[SerializeField] private float _maxPlayerSpeed; //fastest the player move

	[SerializeField] internal float _playerDashForce;
	[SerializeField] private Sandevistan _sandevistan;
	[SerializeField] internal float _dashCooldown;
	internal float _currentDashCooldown;

	[SerializeField] Light2D _light2D;

	//Functions
	void Start()
	{
		_currentPlayerSpeed = _minPlayerSpeed;
		_light2D = GetComponentInChildren<Light2D>();
	}

	void Update()
	{
		PlayerMovement();
		PlayerFast();
		//PlayerDash();
		
	}



	/* Player Movement*/
	void PlayerMovement()
	{

		/* Horizontal Movement */
		if (Input.GetKey(KeyCode.A))
		{

			transform.Translate(Vector2.left * _currentPlayerSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.Translate(Vector2.right * _currentPlayerSpeed * Time.deltaTime, Space.World);
		}

		if (Input.GetKey(KeyCode.W))
		{

			transform.Translate(Vector2.up * _currentPlayerSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey(KeyCode.S))
		{

			transform.Translate(Vector2.down * _currentPlayerSpeed * Time.deltaTime, Space.World);
		}
	}

	/* Player Sandevistan */
	void PlayerFast()
	{
		//Player move faster
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_currentPlayerSpeed = _maxPlayerSpeed;
			_light2D.falloffIntensity = 0.1f;
			_light2D.pointLightOuterRadius = 1.6f;
			_light2D.intensity = 2;
			_light2D.GetComponent<LightControl>().enabled = false;
			
		}
		if (!Input.GetKey(KeyCode.Space)) { _currentPlayerSpeed = _minPlayerSpeed;
			_light2D.falloffIntensity = 0.148f;
			_light2D.pointLightOuterRadius = 1.12f;
			_light2D.intensity = 1;
			_light2D.GetComponent<LightControl>().enabled = true;
		} //Revert player back to normal speed
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
}

