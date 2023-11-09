using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
	// Variables
	[SerializeField] internal int currentAmmo = 30; // Current ammo count
	[SerializeField] private Transform[] bulletSprites; // Array to store bullet sprites
	[SerializeField] private float _waitForReload;

	private bool isReloading = false; // Flag to track reloading status

	private void Start()
	{
		// Get all the child bullet sprites and store them in an array
		bulletSprites = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			bulletSprites[i] = transform.GetChild(i);
		}
	}

	// Link to the Shoot Function from the PlayerShoot script
	public void DecreaseAmmo()
	{
		if (!isReloading && currentAmmo > 0)
		{
			bulletSprites[currentAmmo - 1].gameObject.SetActive(false);
			currentAmmo--;
		}
	}

	// Link to the Reload function from the PlayerShoot Script
	public void Reload()
	{
		if (currentAmmo < 30 && !isReloading)
		{
			StartCoroutine(EnableBulletsDuringReload());
		}
	}

	IEnumerator EnableBulletsDuringReload()
	{
		isReloading = true; // Set reloading flag to true
		float timePerBullet = 1.0f / 30.0f; // Time for each bullet to enable
		for (int i = currentAmmo; i < 30; i++)
		{
			bulletSprites[i].gameObject.SetActive(true);
			currentAmmo++;
			yield return new WaitForSeconds(timePerBullet);
		}
		isReloading = false; // Set reloading flag to false when the reload is complete
	}
}
