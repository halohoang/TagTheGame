using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyStateAnimation : MonoBehaviour
{
	// Variables
	private Animator _animator;
	/* Reference to Movement script */
	internal Movement _movementScript;
	private bool _isMoving = false;

	private bool _isArmed = false;



	// Functions

	void Start()
	{
		_animator = GetComponent<Animator>();
	}

	void Update()
	{
		bool isMovingNow = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);
		if (_animator != null)
		{
			_animator.SetBool("isMoving", isMovingNow);
			//if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
			//{
			//	_animator.SetBool("isMoving", true);
			//}
			if (Input.GetKeyDown(KeyCode.Q))
			{
				_isArmed = !_isArmed;
				_animator.SetBool("Armed", _isArmed);
			}
			 if (Input.GetKeyDown(KeyCode.E))
			{
				_isArmed = !_isArmed;
				_animator.SetBool("Armed", _isArmed);
			}
			if (!isMovingNow && !_isArmed)
			{
				_animator.SetBool("Armed", false);
			}



			//else
			//{
			//	_animator.SetBool("isMoving", false);


			//}
		}

	}
}
