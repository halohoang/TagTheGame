using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerShoot : MonoBehaviour
{
	// Variables

	/* Gun properties */
	[SerializeField] GameObject _uzi;
	[SerializeField] private float _firerate = 0.1f;
	private float _nextFireTime; //Time when the player can shoot again
	[SerializeField] private Transform _gunTransform; //Transform of the gun
	[SerializeField] private LayerMask _hitMask; //Layer mask to filter what the hitscan can hit
	private bool _isArmed; //Checking whether the player is armed or not

	// Functions
	private void Update()
	{
		SwitchWeapon();
		if (_isArmed && Input.GetMouseButton(0) && Time.time >= _nextFireTime)
		{
			Shoot();
			_nextFireTime = Time.time + _firerate;
		}
	}
	

	void SwitchWeapon()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			_isArmed = !_isArmed;
			_uzi.SetActive(_isArmed);
		}
	}

	void Shoot()
	{
		// Perform the hitscan raycast
		RaycastHit2D hit = Physics2D.Raycast(_gunTransform.position, _gunTransform.right, Mathf.Infinity, _hitMask);

		if (hit.collider != null)
		{
			// Handle the hit - e.g., apply damage to an enemy, show effects, etc.
			Debug.Log("Hit something: " + hit.collider.name);
		}
		else
		{
			// Handle a miss or hitting something without a collider
			Debug.Log("Missed or hit something without a collider.");
		}
	}


}
