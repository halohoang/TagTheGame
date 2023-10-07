using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCasing : MonoBehaviour
{
	//Variables
	[SerializeField] private float ejectionForce = 2.0f; // The force applied to the casing when spawned

	[SerializeField] private float _despawnTime = 5.0f; // The time in seconds before the casing is destroyed

	private void Start()
	{
		// Apply an initial force to simulate ejection from the gun
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			Vector2 ejectionDirection = Random.insideUnitCircle.normalized;
			rb.AddForce(ejectionDirection * ejectionForce, ForceMode2D.Impulse);
		}
	}

	private void Update()
	{
		// Destroy the casing after a set amount of time
		Invoke("DeactiveObject", _despawnTime);
	}

	void DeactiveObject()
	{
		gameObject.SetActive(false);
	}

}
