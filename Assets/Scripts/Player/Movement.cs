using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
	// Variables
	/* Player Movement Variables */
	[SerializeField] private float _playerSpeed; //How fast the player move
	[SerializeField] private float _playerDashForce;
	internal bool _isMoving;


	//Functions
	void Start()
	{

	}

	void Update()
	{
		PlayerMovement();
		//PlayerDash();

	}



	/*Player Movement*/
	void PlayerMovement()
	{

		/* Horizontal Movement */
		if (Input.GetKey(KeyCode.A))
		{
			_isMoving = true;
			transform.Translate(Vector2.left * _playerSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.Translate(Vector2.right * _playerSpeed * Time.deltaTime, Space.World);
		}

		if (Input.GetKey(KeyCode.W))
		{
			_isMoving = true;
			transform.Translate(Vector2.up * _playerSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey(KeyCode.S))
		{
			_isMoving = true;
			transform.Translate(Vector2.down * _playerSpeed * Time.deltaTime, Space.World);
		}
		// Check if none of the movement keys are pressed
		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			_isMoving = false;
		}
	}
	/* Player Dash */
	//void PlayerDash()
	//{
	//	if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.Space))
	//	{
	//		/*Dash direction to the left */
	//		Vector2 dashDirection = Vector2.left;
	//		/* Apply Dash Force */
	//		Vector2 dashForce = dashDirection * _playerDashForce * Time.deltaTime;
	//		transform.Translate(dashForce, Space.World);
	//		_isMoving = true;
	//	}
	//	if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.Space))
	//	{
	//		/*Dash direction to the left */
	//		Vector2 dashDirection = Vector2.right;
	//		/* Apply Dash Force */
	//		Vector2 dashForce = dashDirection * _playerDashForce * Time.deltaTime;
	//		transform.Translate(dashForce, Space.World);
	//		_isMoving = true;

	//	}
	//	if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space))
	//	{
	//		/*Dash direction to the left */
	//		Vector2 dashDirection = Vector2.up;
	//		/* Apply Dash Force */
	//		Vector2 dashForce = dashDirection * _playerDashForce * 0.5f * Time.deltaTime;
	//		transform.Translate(dashForce, Space.World);
	//		_isMoving = true;

	//	}

	//	if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
	//	{
	//		/*Dash direction to the left */
	//		Vector2 dashDirection = Vector2.down;
	//		/* Apply Dash Force */
	//		Vector2 dashForce = dashDirection * _playerDashForce * 0.5f * Time.deltaTime;
	//		transform.Translate(dashForce, Space.World);
	//		_isMoving = true;

	//	}
	//}
}

