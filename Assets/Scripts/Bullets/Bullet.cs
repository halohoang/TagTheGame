using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	// Variables
	/* Bullet Properties */
	[SerializeField] internal float _bulletSpeed;

	/*Bounce Properties */
	[SerializeField] private int _maxBounce;
	private int _currentBounce;
	internal Rigidbody2D _bulletRB2D;

	// Function
	void Start()
	{
		_bulletRB2D = GetComponent<Rigidbody2D>();
		_currentBounce = 0;
	}

	void Update()
	{
		if (_currentBounce >= _maxBounce)
		{
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Bullet")
		{
			Vector2 normal = collision.contacts[0].normal; // Get the normal of the collision
			Vector2 newDirection = Vector2.Reflect(_bulletRB2D.velocity.normalized, normal); // Calculate the new direction of the bullet
			_bulletRB2D.velocity = newDirection.normalized * _bulletSpeed; // Set the velocity to the new direction with the same speed
			_currentBounce++;
		}
	}
}
