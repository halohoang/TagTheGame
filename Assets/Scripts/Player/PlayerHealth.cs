using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	// Variables
	[SerializeField] private float _maxHealth;
	[SerializeField] internal float _currentHealth;
	[SerializeField] GameObject _player;

	private Rigidbody2D _rb2D;

	// Functions
	void Start()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		_currentHealth = _maxHealth;
	}

	void Update()
	{
		ReduceHP();
	}

	void GetDamage()
	{

	}
	void ReduceHP()
	{
		if (_player != null)
		{

			// If the player hold down Sandevistan his health bar will start to get depleted
			if (Input.GetKey(KeyCode.Space))
			{
				_currentHealth -= 0.5f;
			}

			// Ensure _currentHealth does not go below 0
			_currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);


			//Testing whether the player heals. Regenerate 1 HP per frame
			if (!Input.GetKey(KeyCode.Space) && _currentHealth != _maxHealth)
			{
				_currentHealth += 0.1f;
			}

			_currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

		}
	}
}
