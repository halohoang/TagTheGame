using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounce : MonoBehaviour
{
	// Variables
	[SerializeField] private float _speed = 10f;
	[SerializeField] private int _maxBounce;
	private int _currentBounce;
	private Rigidbody2D _rb2D;
	private Vector2 _direction;

	//Functions
	void Start()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		_rb2D.velocity = transform.right * _speed;
		_direction = _rb2D.velocity.normalized;
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
		Vector2 _normal = collision.contacts[0].normal; // Get the normal of the collision
		_direction = Vector2.Reflect(_direction, _normal); // Calculate the new direction of the bullet
		_rb2D.velocity = _direction * _speed; //the speed of the bullet will increase exponentially after each bounce.
		_currentBounce += 1;
	}
}
