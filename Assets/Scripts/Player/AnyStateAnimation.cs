using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyStateAnimation : MonoBehaviour
{
	// Variables
	private Animator _animator;
	/* Reference to Movement script */
	internal Movement _movementScript;
	private bool _isMoving;



	// Functions

	void Start()
	{
		_animator = GetComponent<Animator>();
		_movementScript = GetComponent<Movement>();
		_isMoving = _movementScript._isMoving;
	}

	void Update()
	{
		if(_animator != null)
		{
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
			{
				_animator.SetBool("isMoving", true);
			}
			else
			{


				_animator.SetBool("isMoving", false);


			}
		}
		
	}
}
