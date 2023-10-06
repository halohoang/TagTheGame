using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	// Variables
	/* Bullet Properties */
	[SerializeField] internal float _bulletSpeed;

	/*Bounce Properties */
	internal Rigidbody2D _bulletRB2D;
	[SerializeField] internal float _maxBulletAliveTime = 10f; // Maximum of time until the bullet is destroyed
	internal float _currentBulletLiveTime; // Current time until the bullet is destroyed
	private GameObject _bullet;



	// Function
	void Start()
	{
		_bullet = GetComponent<GameObject>();
		_bulletRB2D = GetComponent<Rigidbody2D>();
		_bulletRB2D.velocity = transform.right * _bulletSpeed;
		_currentBulletLiveTime = _maxBulletAliveTime;

	}

	void Update()
	{

		BulletDeactive();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Bullet"))
		{
			gameObject.SetActive(false);
		}

		if (collision.gameObject.CompareTag("Player"))
		{
			PlayerHealth playerHealth;
			playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
			if (playerHealth != null)
			{
				/* Deal Damage */
				playerHealth.GetDamage();
				Debug.Log(playerHealth._currentHealth);
				gameObject.SetActive(false );
			}
			// Call deal damage function
			
		}


		if (collision.gameObject.CompareTag("Enemy"))
		{
			EnemyHealth enemyHealth;
			enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
			if (enemyHealth != null)
			{
				enemyHealth.GetDamage();
				Debug.Log(enemyHealth._currentHealth);
				gameObject.SetActive(false);
			}
		}
	}
	private void BulletDeactive()
	{
		if (_bulletRB2D != null)
		{
			_currentBulletLiveTime -= Time.deltaTime;
			if (_currentBulletLiveTime <= 0) { gameObject.SetActive(false); }
		}
	}
}
