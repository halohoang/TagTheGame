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

	/* Spawning Blood Variables */
	[SerializeField] GameObject _bloodHitSpawnPrefab; //Spawning blood 
	[SerializeField] Transform _bloodHitSpawnTransform; // Where does it spawn
	
	// Functions
	void Start()
	{
		_currentHealth = _maximumHealth;
		_takingDamageScript = GetComponent<TakingDamage>();
	}


	void Update()
	{

	}

	internal void GetDamage()
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
