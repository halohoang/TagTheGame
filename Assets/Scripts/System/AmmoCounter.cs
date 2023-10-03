using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
	//Variables
	[SerializeField] internal int currentAmmo = 30; // Current ammo count
	[SerializeField]private Transform[] bulletSprites; // Array to store bullet sprites
	[SerializeField] private float _waitForReload;

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
		if (currentAmmo > 0)
		{
			bulletSprites[currentAmmo - 1].gameObject.SetActive(false);
			currentAmmo--;
		}
	}

	// Link to the Reload function from the PlayerShoot Script
	public void Reload()
	{
		if (currentAmmo < 30)
		{
			StartCoroutine(EnableBulletsDuringReload());
		}
	}

	IEnumerator EnableBulletsDuringReload()
	{
		for (int i = currentAmmo; i < 30; i++)
		{
			bulletSprites[i].gameObject.SetActive(true);
			currentAmmo++;
			yield return new WaitForSeconds(_waitForReload); // Adjust the delay as needed
		}
	}
}
