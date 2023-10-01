using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	// Variables
	[SerializeField] private float _maxHealth;
	[SerializeField] internal float _currentHealth;
	[SerializeField] GameObject _player;
	[SerializeField] Transform _chargeBarTransform; // Reference to the scale of the bar
	[SerializeField] float _chargeSpeed = 0.1f; // The rate at which bar depletes or charges

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
				ReduceCharge();
			}

			// Ensure _currentHealth does not go below 0
			_currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);


			//Testing whether the player heals. Regenerate 1 HP per frame
			if (!Input.GetKey(KeyCode.Space) && _currentHealth != _maxHealth)
			{
				_currentHealth += 0.1f;
				RegenCharge();
			}

			_currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

		}
	}

	void ReduceCharge()
	{
		if (_chargeBarTransform != null)
		{
			Debug.Log("reduce");
			_chargeBarTransform.localScale -= new Vector3(0f, _chargeSpeed, 0f) * Time.deltaTime; // Reduce the Y scale of the charge bar by chargeSpeed
			_chargeBarTransform.localScale = new Vector3(_chargeBarTransform.localScale.x, Mathf.Max(0f, _chargeBarTransform.localScale.y), _chargeBarTransform.localScale.z); // Make sure scale does not go below 0
		}
	}
	void RegenCharge()
	{
		if (_chargeBarTransform != null)
		{
			_chargeBarTransform.localScale += new Vector3(0f, _chargeSpeed * 0.3f, 0f) * Time.deltaTime; // Increase the Y scale of the charge bar by chargeSpeed
			_chargeBarTransform.localScale = new Vector3(_chargeBarTransform.localScale.x, Mathf.Min(_chargeBarTransform.localScale.y, 0.16f), _chargeBarTransform.localScale.z); // Make sure scale does not go above 0

		}
	}

}
