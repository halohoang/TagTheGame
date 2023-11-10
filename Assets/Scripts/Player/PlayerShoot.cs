using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour
{
	public static event UnityAction<bool, Vector3, float> OnPlayerShoot;
	// Variables

	/* Gun properties */
	[SerializeField] GameObject _gun;
	[SerializeField] private Transform _bulletSpawnPoint; // Transform of the gun
	private bool _isArmed; // Checking whether the player is armed or not
	[SerializeField] private GameObject _bulletPrefab;
	//[SerializeField] internal PlayerBullet _bulletScript;

	/* Reload system */
	[SerializeField] internal int _maximumBulletCount;
	[SerializeField] internal int _currentBulletCount;
	//[SerializeField] internal int _minimumBulletCount; // Will use to do sound for low ammo  sound
	private bool _isReloading = false;
	[SerializeField] private float _reloadTime; // To sync with how long the reload animation is
	[SerializeField] AmmoCounter _ammoCounterScript; // Link to the AmmoCounter script

	/* Bullet Casing Spawning */
	[SerializeField] private GameObject _bulletCasingPrefab; // Prefab of the bullet casing
	[SerializeField] private Transform _casingSpawnPosition;

	/* Firerate */
	[SerializeField] private float _firerate = 0.5f;
	private float _nextFireTime;

	/* Recoil */
	[SerializeField] private float _maxDeviationAngle = 5f; // Maximum deviation the bullet will be off from the straight line
	[SerializeField] private float _whenDeviationKicksIn;

	private float _mouseButtonReleaseTime; // Time when the mouse button was last released

	/* Shooting Noise Range */
	[SerializeField, Range(0.0f, 25.0f)] private float _shootingNoiseRange = 10.0f;

	[SerializeField] private AudioClip _fireSound; //Fire sound
	[SerializeField] private AudioClip _reloadSound; // Reload sound
	private AudioSource _audioSource;

	/* Camera Shake */
	[SerializeField] private CameraRecoilShake cameraShake;
	////private float _triggerHoldStartTime = 0f;
	[SerializeField] internal float duration = 0.05f;
	[SerializeField]internal float amount = 0.08f;


	/* Muzzle Flash */
	[SerializeField] private GameObject _muzzleFlash;
	[SerializeField] private Animator _animator;


	[Header("Monitoring Values")]
	[SerializeField, ReadOnly] private bool _isShooting;

    // Functions
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, _shootingNoiseRange);
    }

    private void Start()
	{
		_currentBulletCount = _maximumBulletCount;
		_audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		SwitchWeapon();
		if (_isArmed)
		{
			if (Input.GetMouseButtonUp(0))
			{
				_mouseButtonReleaseTime = Time.time; // Record the time when the mouse button was released
			}
			else if (Input.GetMouseButton(0) && CanFire() && _currentBulletCount > 0 && _isReloading == false)
			{
				_isShooting = true;
				_animator.SetBool("Firing", _isShooting);
				Shoot();
				_currentBulletCount--;
				SpawnBulletCasing();
				cameraShake.StartShake( duration, amount);
				Debug.Log("Shake");

				OnPlayerShoot?.Invoke(_isShooting, transform.position, _shootingNoiseRange);
			}
			else
			{
                _isShooting = false;
                _animator.SetBool("Firing", _isShooting);
			}
		}
		if (Input.GetKeyDown(KeyCode.R) && _isReloading == false && !Input.GetMouseButton(0))
		{
			if (_currentBulletCount < _maximumBulletCount)
			{
				_ammoCounterScript.Reload();
				StartCoroutine(Reload());
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

			/* Play FIre Sound */
			if (_fireSound != null) { _audioSource.PlayOneShot(_fireSound); }
			GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, bulletRotation);
			Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();
			_ammoCounterScript.DecreaseAmmo(); //Call the Decrease Ammo function from the AmmoCounter script;
			_animator.SetBool("Firing", true);
			_nextFireTime = Time.time + _firerate;
			if (Input.GetKey(KeyCode.Space))
			{
				duration = 0;
				amount = 0;
			}else if (!Input.GetKeyUp(KeyCode.Space))
				{
					duration = 0.05f;
						amount = 0.08f;
				}

		}
	}

	IEnumerator Reload()
	{
		_isReloading = true;
		// Play reload animation

		int bulletsLeftToFullMag = _maximumBulletCount - _currentBulletCount;
		if (bulletsLeftToFullMag > 0)
		{
			_audioSource.PlayOneShot(_reloadSound);

			if (bulletsLeftToFullMag <= _currentBulletCount)
			{
				_currentBulletCount += bulletsLeftToFullMag;
			}
			else
			{
				_currentBulletCount = _maximumBulletCount;
			}

		}
		yield return new WaitForSeconds(_reloadTime);
		_isReloading = false;
	}

	private void SpawnBulletCasing()
	{
		// Instantiate a bullet casing at the specified spawn point
		Quaternion casingRotation = Quaternion.Euler(0f,0f,Random.Range(0,360f));
		Instantiate(_bulletCasingPrefab, _casingSpawnPosition.position, casingRotation);
	}
}