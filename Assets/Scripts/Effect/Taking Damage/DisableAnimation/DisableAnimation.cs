using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnimation : MonoBehaviour
{
	// Variables
	private Animator _animator;
	void Start()
	{
		_animator = GetComponent<Animator>();
	}

	// Functions
	public void DisableObject()
	{
		gameObject.SetActive(false);
	}

}
 