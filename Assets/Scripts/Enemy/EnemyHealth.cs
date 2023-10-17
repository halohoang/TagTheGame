using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	// Variables
	[SerializeField] private float _maximumHealth;
	[SerializeField] internal float _currentHealth;
	[SerializeField] private float _takenDamage;
	private TakingDamage _takingDamageScript;


	/* Animation */
	[SerializeField] Animator _animator;

	/* Spawning Blood Variables */
	[SerializeField] GameObject _bloodHitSpawnPrefab; //Spawning blood 
	[SerializeField] Transform _bloodHitSpawnTransform; // Where does it spawn

	// Functions
	void Start()
	{
		_currentHealth = _maximumHealth;
		_takingDamageScript = GetComponent<TakingDamage>();
		_animator = GetComponent<Animator>();
	}


	void Update()
	{
		if (_currentHealth == 0)
		{
			Dead();
		}
	}

	internal void GetDamage()
	{
		if (_currentHealth > 0)
		{

			_currentHealth -= _takenDamage;
			if (_takingDamageScript != null)
			{
				_takingDamageScript.FlashOnce();
				Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
				Instantiate(_bloodHitSpawnPrefab, _bloodHitSpawnTransform.position, bloodRotation);
			}
		}
	}
	internal void Dead()
	{
		_animator.SetTrigger("Dead");
	}
}
