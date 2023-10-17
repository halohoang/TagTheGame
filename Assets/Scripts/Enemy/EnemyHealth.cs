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
	private BoxCollider2D _boxCollider2D;

	/* Spawning Blood Variables */
	[SerializeField] GameObject _bloodHitSpawnPrefab; //Spawning blood 
	[SerializeField] Transform _bloodHitSpawnTransform; // Where does it spawn

	// Functions
	void Start()
	{
		_currentHealth = _maximumHealth;
		_takingDamageScript = GetComponent<TakingDamage>();
		_animator = GetComponent<Animator>();
		_boxCollider2D = GetComponent<BoxCollider2D>();
	}


	void Update()
	{
		
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
		if (_currentHealth == 0)
		{
			Dead();
			_boxCollider2D.enabled = false;
		}

	}
	internal void Dead()
	{
		this.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0,360f));
		_animator.SetTrigger("Dead");
	}
}
