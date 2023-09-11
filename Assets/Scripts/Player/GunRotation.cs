using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
	// Variables
	[SerializeField] private Transform _gun; //Find the Player Transform
	[SerializeField] private Transform _player;
	private Vector2 _mousePosition;


	// Functions
	void Update()
	{
		if (_gun != null)
		{
			_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Vector2 direction = (_mousePosition - (Vector2)transform.position).normalized;

			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

			if (_player.transform.localScale.x < 0)
			{
				angle += 180f;
			}
				_gun.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));


		}
	}
}
