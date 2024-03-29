using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
	// Variables
	[SerializeField] internal int _currentAmmo = 30;		// Current ammo count
	[SerializeField] private PlayerEquipmentSO _playerEquipmentSO;
	[SerializeField] private Transform[] bulletSprites; // Array to store bullet sprites
	[SerializeField] private float _waitForReload;
	[SerializeField] private GameObject _reloadHint;

	private bool isReloading = false; // Flag to track reloading status

    private void Awake()
    {
        // auto Referencing
        if (_playerEquipmentSO == null)
        {
            _playerEquipmentSO = Resources.Load("ScriptableObjects/PlayerEquipment") as PlayerEquipmentSO;
            Debug.Log($"<color=yellow>Caution!</color>: Reference for ScriptableObject 'PlayerEquipment' in Inspector of {this} was not set. So it was Set automatically.");
        }
    }

    private void Start()
	{
		// Get all the child bullet sprites and store them in an array
		bulletSprites = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			bulletSprites[i] = transform.GetChild(i);
		}
	}
	private void Update()
	{
		if(_currentAmmo < 30)
		{
			_reloadHint.SetActive(true);
		}
		if (_currentAmmo >= 30) { _reloadHint.SetActive(false); }
	}

	// Link to the Shoot Function from the PlayerShoot script
	public void DecreaseAmmo()
	{
		if (!isReloading && _currentAmmo > 0)
		{
			bulletSprites[_currentAmmo - 1].gameObject.SetActive(false);
			_currentAmmo--;
		}
	}

	// Link to the Reload function from the PlayerShoot Script
	public void Reload()
	{
		if (_currentAmmo < 30 && !isReloading)
		{
			StartCoroutine(EnableBulletsDuringReload());
		}
		
	}
	

	IEnumerator EnableBulletsDuringReload()
	{
		isReloading = true; // Set reloading flag to true
		float timePerBullet = 1.0f / 30.0f; // Time for each bullet to enable
		for (int i = _currentAmmo; i < 30; i++)
		{
			bulletSprites[i].gameObject.SetActive(true);
			_currentAmmo++;
			yield return new WaitForSeconds(timePerBullet);
		}
		isReloading = false; // Set reloading flag to false when the reload is complete
	}
}
