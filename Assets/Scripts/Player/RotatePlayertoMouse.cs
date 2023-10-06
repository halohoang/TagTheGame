using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayertoMouse : MonoBehaviour
{
	// Variables
	private Rigidbody2D _rb2D;
	[SerializeField] private Transform _playerTransform; // Reference to the player's transform.

	void Start()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		/*_playerTransform = transform.parent;*/ // Get the player's transform from the parent.
	}

	//Functions
	void Update()
	{
		RotateToMousePosition();
	}

	void RotateToMousePosition()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = (mousePos - _playerTransform.position).normalized; // Use player's position.
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		_rb2D.rotation = angle;
	}
}
