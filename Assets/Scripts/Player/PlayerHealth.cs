using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	// Variables
	[SerializeField] private int _maxHealth;
	[SerializeField] internal int _currentHealth;

	private Rigidbody2D _rb2D;

	// Functions
	void Start()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		_currentHealth = _maxHealth;
	}

	void Update()
	{

	}

	void GetDamage()
	{

	}
	void ReduceHP()
	{
		// If the player hold down Sandevistan his health bar will start to get depleted
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_currentHealth -= 1;
		}


		//Testing whether the player heals. Regenerate 1 HP per frame
		if (Input.GetKeyUp(KeyCode.Space))
		{
			_currentHealth += 1;
		}


	}
}
