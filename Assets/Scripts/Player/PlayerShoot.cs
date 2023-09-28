using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
	// Variables
	[SerializeField] GameObject _uzi;
	private bool _isArmed;

	// Functions
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			_isArmed = !_isArmed;
			_uzi.SetActive(_isArmed);
		}
	}
}
