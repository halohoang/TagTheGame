using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCasing : MonoBehaviour
{
	// Variables
	[SerializeField] private GameObject bulletCasingPrefab; // Prefab of the bullet casing
	[SerializeField] private Transform casingSpawnPoint; // The point where casings are spawned
	[SerializeField] private float casingSpawnDelay = 0.1f; // Delay between spawning casings

	private GameObject[] bulletPool; // Array of bullet objects

	// Funcctions
	private void Start()
	{
		// Initialize your bullet pool here; make sure to populate it with GameObjects
	}

	private void Update()
	{
		// Check for player input to spawn casings
		if (Input.GetMouseButtonDown(0)) // Adjust the input condition as needed
		{
			SpawnBulletCasings();
		}
	}

	private void SpawnBulletCasings()
	{
		// Loop through the bullet pool
		for (int i = 0; i < bulletPool.Length; i++)
		{
			if (!bulletPool[i].activeSelf) // Check if the bullet is deactivated
			{
				// Spawn a bullet casing at the specified spawn point
				Instantiate(bulletCasingPrefab, casingSpawnPoint.position, Quaternion.identity);
				// Delay between spawning casings (optional)
				StartCoroutine(DelayCasingSpawn(i));
			}
		}
	}

	private IEnumerator DelayCasingSpawn(int index)
	{
		yield return new WaitForSeconds(casingSpawnDelay);

		// Reactivate the bullet after a delay (if needed)
		bulletPool[index].SetActive(true);
	}
}
