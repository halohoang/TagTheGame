using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
	// Variables

	/* Gun properties */
	[SerializeField] GameObject _gun;
	[SerializeField] private Transform _bulletSpawnPoint; // Transform of the gun
	private bool _isArmed; // Checking whether the player is armed or not
	[SerializeField] private GameObject _bulletPrefab;
	[SerializeField] internal Bullet _bulletScript;

	/* Firerate */
	[SerializeField] private float _firerate = 0.5f;
	private float _nextFireTime;

	/* Recoil */
	[SerializeField] private float _maxDeviationAngle = 5f; // Maximum deviation the bullet will be off from the straight line
	[SerializeField] private float _whenDeviationKicksIn;

	private float _mouseButtonReleaseTime; // Time when the mouse button was last released

	/* Camera Shake */
	[SerializeField] private CameraRecoilShake cameraShake;
	private float _triggerHoldStartTime = 0f;

	/* Muzzle Flash */
	[SerializeField] private GameObject _muzzleFlash;
	[SerializeField] private Animator _animator;


	// Functions
	private void Update()
	{
		SwitchWeapon();
		if (_isArmed)
		{
			if (Input.GetMouseButtonDown(0))
			{
				_mouseButtonReleaseTime = Time.time; // Record the time when the mouse button was released
			}
			else if (Input.GetMouseButton(0) && CanFire())
			{
				_animator.SetBool("Firing", true);
				Shoot();
			}
			else
			{
			_animator.SetBool("Firing", false);
			}
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

	bool CanFire()
	{
		return Time.time > _nextFireTime;
	}

	float CalculateDeviation()
	{
		float holdTriggerDuration = Mathf.Clamp01((Time.time - _mouseButtonReleaseTime) / _whenDeviationKicksIn); // Normalize the duration between 0 and 1, with a maximum of 5 seconds
		return _maxDeviationAngle * holdTriggerDuration;
	}

	void Shoot()
	{
		if (CanFire())
		{
			// Calculate Deviation during the shooting
			float deviation = CalculateDeviation();

			Quaternion bulletRotation = _bulletSpawnPoint.rotation; // Apply deviation to the bullet's rotation
			float randomAngle = Random.Range(-deviation, deviation); // Randomize the deviation angle

			bulletRotation *= Quaternion.Euler(0f, 0f, randomAngle); // Apply rotation around the Z-axis

			GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, bulletRotation);
			Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();
			_animator.SetBool("Firing", true);
			_nextFireTime = Time.time + _firerate;
			float startShakeDuration = 0.1f;
			float startShakeAmount = 0.15f;
			cameraShake.StartShake(startShakeDuration, startShakeAmount);
		}

	}
}