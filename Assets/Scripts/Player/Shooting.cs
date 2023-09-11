using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
	// Variable

	[SerializeField] private GameObject _player;

	/* Gun Properties */

	/* Reference to the Pistol Script */
	internal Pistol _pistolScript;

	// Function
	void Start()
	{
		_pistolScript = GetComponent<Pistol>();
	}

	void Update()
	{
		PlayerShootAnimation();
		PlayerShoot();
	}

	private void PlayerShoot()
	{
		if (Input.GetMouseButton(0))
		{
			_pistolScript.Shoot();
		}

	}

	public void PlayerShootAnimation()
	{
		if (_player != null)
		{

			if (Input.GetMouseButtonDown(0))
			{
				/* Play Player Shoot Animation */
				//_pistolScript.ShootAnimation();
				Debug.Log("Player Shot");

			}
		}
	}
}
