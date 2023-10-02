using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
	// Variables

	/* Gun properties */
	[SerializeField] GameObject _gun;
	[SerializeField] private Transform _bulletSpawnPoint; //Transform of the gun
	private bool _isArmed; //Checking whether the player is armed or not
	[SerializeField] private GameObject _bulletPrefab;
	[SerializeField]internal Bullet _bulletScript;

	/* Firerate */
	[SerializeField] 

	// Functions
	private void Update()
	{
		SwitchWeapon();
		if (_isArmed && Input.GetMouseButton(0))
		{
		Shoot();
		}
		
	}
	

	void SwitchWeapon()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			_isArmed = !_isArmed;
			_gun.SetActive(_isArmed);
		}
	}

	void Shoot()
	{
		GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
		Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();
	}

	


}
