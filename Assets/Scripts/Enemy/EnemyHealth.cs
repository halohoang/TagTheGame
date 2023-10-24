using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
	private bool _isDead = false;
	SpriteRenderer _spriteRenderer;

	/* Spawning Blood Variables */
	[SerializeField] List<GameObject> _bloodPoolSpawn; //Spawning blood pool when hit
	[SerializeField] GameObject _bloodDeadPrefab; //Spawning blood pool at where enemy die

	// Functions
	void Start()
	{
		_currentHealth = _maximumHealth;
		_takingDamageScript = GetComponent<TakingDamage>();
		_animator = GetComponent<Animator>();
		_boxCollider2D = GetComponent<BoxCollider2D>();
		SpriteRenderer _spriteRenderer = GetComponent<SpriteRenderer>();
	}

	internal void GetDamage()
	{
		if (_currentHealth > 0)
		{
			_currentHealth -= _takenDamage;
			if (_takingDamageScript != null)
			{
				_takingDamageScript.FlashOnce();

				/* Spawn blood and stay on the ground while enemy moving*/
				int randomIndex = Random.Range(0, _bloodPoolSpawn.Count);
				Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
				Instantiate(_bloodPoolSpawn[randomIndex], transform.position, bloodRotation);
			}
		}
		if (_currentHealth <= 0 && _isDead == false)
		{
			// Randomize dead rotation
			this.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
			_animator.SetTrigger("Dead");
			_isDead = true;
			_boxCollider2D.isTrigger = true;

			// Setup Enemy-Behaviour to EnemyDead
			gameObject.GetComponent<EnemyQuickfixBehaviour_ForTesting>().IsEnemyDead = _isDead;
			gameObject.GetComponent<NavMeshAgent>().isStopped = true;			
			gameObject.GetComponent<EnemyQuickfixBehaviour_ForTesting>().enabled = false;
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		/* Spawn blood where enemy die */
		Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
		Instantiate(_bloodDeadPrefab, transform.position, bloodRotation);
	}
}
