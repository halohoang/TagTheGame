using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
	// Variable

	/* Spawn Point */
	[SerializeField]
	private Transform _bulletSpawnPoint;

	/* Gun Properties */
	[SerializeField] private float _shootingDelay = 0.5f;
	[SerializeField] private float _lastShootTime;

	[SerializeField] private LayerMask Mask; //Determine which object the raycast can hit





	/* Particle system */
	//[SerializeField] private TrailRenderer _bulletTrail;
	//[SerializeField] private ParticleSystem _impactParticleSystem;


	/* Animation */
	private AnimationState _currentAnimationState = AnimationState.Idle;
	private Animator _animator;

	/* Enum */
	enum AnimationState
	{
		Idle,
		Shoot,
	}
	// Function
	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	//void Start()
	//{
	//	//_animator = GetComponent<Animator>();
	//	//_animator.Play(_currentAnimationState.ToString());
	//	//_animator.speed = 10f;

	//}

	public void Shoot()
	{
		if (_lastShootTime + _shootingDelay < Time.time)
		{
			_animator.SetBool("IsShooting", true);

			Vector2 direction = GetDirection();
			RaycastHit2D hit = Physics2D.Raycast(_bulletSpawnPoint.position, direction, float.MaxValue, Mask);
			Debug.Log("Player Shoot");

			if (hit.collider != null)
			{
				Debug.Log("1");

				Debug.Log("Hit an object on layer: " + hit.collider.gameObject.layer);
				/*Bullet Trail */
				//TrailRenderer trail = Instantiate(_bulletTrail, _bulletSpawnPoint.position, Quaternion.identity);
				//StartCoroutine(SpawnTrail(trail, hit));

				_lastShootTime = Time.time;
			}

		}
	}

	private Vector2 GetDirection()
	{
		Vector2 direction = transform.forward;
		return direction;
	}

	/*Spawning bullet trail */
	//IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit2D Hit)
	//{
	//	float time = 0;
	//	Vector2 startPosition = Trail.transform.position;
	//	while (time < 1)
	//	{
	//		Trail.transform.position = Vector2.Lerp(startPosition, Hit.point, time);
	//		time += Time.deltaTime / Trail.time;
	//		yield return null;
	//	}
	//	_animator.SetBool("IsShooting", false);
	//	Trail.transform.position = Hit.point;

	//	//Instantiate(_impactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));

	//	Destroy(Trail.gameObject, Trail.time);
	//}

	// Update is called once per frame
	//void Update()
	//{

	//}



	//public void ShootAnimation()
	//{
	//	_currentAnimationState = AnimationState.Shoot;
	//	_animator.Play(_currentAnimationState.ToString());
	//	Debug.Log("ShootAnimation called");
	//}
	//public void IdleAnimation()
	//{
	//	_currentAnimationState = AnimationState.Idle;
	//	_animator.Play(_currentAnimationState.ToString());

	//}


}
