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
	[SerializeField] internal Bullet _bulletScript;

	/* Firerate */
	[SerializeField] private float _firerate = 0.5f;
	private float _nextFireTime;

	// Functions
	private void Update()
	{
		SwitchWeapon();
		if (_isArmed && Input.GetMouseButton(0) && CanFire())
		{
			Shoot();
		}

	}

	bool CanFire()
	{
		return Time.time > _nextFireTime;
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
		if (CanFire())
		{

			GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
			Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();
			_nextFireTime = Time.time + _firerate;
		}
	}
}
