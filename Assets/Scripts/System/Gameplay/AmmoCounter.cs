using NaughtyAttributes;
using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    // Variables
    [Header("References")]
    [SerializeField] private PlayerEquipmentSO _playerEquipmentSO;
    [SerializeField] private GameObject _reloadHint;
    //[SerializeField] private Transform[] bulletSprites; // Array to store bullet sprites
    [SerializeField] private List<Transform> _bulletSprites; // Array to store bullet sprites
    [Space(5)]

    [Header("Settings")]
    [SerializeField] private float _waitForReload;
    [Space(5)]

    [Header("Monitoring Values")]
    [Tooltip("List of the current active BulletSprites inside the above 'Bullet Sprites' List ")]
    [SerializeField, ReadOnly] private List<Transform> _activeSprites;
    [Tooltip("List of the current inactive BulletSprites inside the above 'Bullet Sprites' List ")]
    [SerializeField, ReadOnly] private List<Transform> _inactiveSprites;
    [Tooltip("The ammo count of the current selected weapon, will be updated automatically on runtime.")]
    [SerializeField, ReadOnly] private int _currentAmmo = 30;      // Current ammo count
    [Tooltip("The original height value of the RectTransForm")]
    [SerializeField, ReadOnly] private float _originRectTransHeight;
    [Tooltip("The original width value of the RectTransForm")]
    [SerializeField, ReadOnly] private float _originRectTransWidth;

    private bool _isReloading = false; // Flag to track reloading status

    internal int CurrentAmmo { get => _currentAmmo; set => _currentAmmo = value; }
    internal float OriginRectTransHeight { get => _originRectTransHeight; private set => _originRectTransHeight = value; }
    internal float OriginRectTransWidth { get => _originRectTransWidth; private set => _originRectTransWidth = value; }

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
        // save original height value of RectTransform
        OriginRectTransHeight = GetComponent<RectTransform>().rect.height;
        OriginRectTransWidth = GetComponent<RectTransform>().rect.width;

        // instatiate Lists
        _bulletSprites = new List<Transform>(transform.childCount);
        _activeSprites = new List<Transform>();
        _inactiveSprites = new List<Transform>();

        // Get all the child bullet sprites and store them in the 'bulletSprites'-array
        for (int i = 0; i < transform.childCount; i++)
        {
            _bulletSprites.Add(transform.GetChild(i));
        }

        //// set Lists for active and inactive 
        //for (int i = 0; i < _bulletSprites.Count; i++)
        //{
        //    if (_bulletSprites[i].gameObject.activeSelf)
        //        _activeSprites.Add(_bulletSprites[i]);
        //    else
        //        _inactiveSprites.Add(_bulletSprites[i]);
        //}
    }

    private void Update()
    {
        //if(CurrentAmmo < 30)
        //{
        //	_reloadHint.SetActive(true);
        //}
        //if (CurrentAmmo >= 30) { _reloadHint.SetActive(false); }
    }

    /// <summary>
    /// Sets the UI shown Ammo Bullets respective to the actual AmmoCount of the selected Weapon
    /// </summary>
    public void SetUIAmmoToActiveWeaponAmmo()
    {
        // 1. Get Reference to the RectTransform-Component of this UI-GameObject
        RectTransform rectTrans = gameObject.GetComponent<RectTransform>();

        // 2. Set the Lists of the active and inactive BulletSprites in the '_bulletSprites'-List
        for (int i = 0; i < _bulletSprites.Count; i++)
        {
            if (_bulletSprites[i].gameObject.activeSelf)
                _activeSprites.Add(_bulletSprites[i]);
            else
                _inactiveSprites.Add(_bulletSprites[i]);
        }

        // 3. enable/disable the BulletSprites Object in the '_bulletSprites'-List respectively to the transmitted 'ammountCount'
        // if ammoCount is greater od less than the Count of the SpriteObjects in the '_bulletSprites'-List enable or disable respectively to the difference between both counts
        if (CurrentAmmo > _bulletSprites.Count)
        {
            for (int i = 0; i < CurrentAmmo - _bulletSprites.Count; i++)
            {
                _bulletSprites[i].gameObject.SetActive(false);
            }

            // decrease the Height of the 'AmmoCount_Panel'-Object respective to its active Bullet-Sprite-ChildObjects
            rectTrans.sizeDelta = new Vector2(rectTrans.rect.width, rectTrans.rect.height - (float)CurrentAmmo * transform.GetChild(0).GetComponent<RectTransform>().rect.width);
        }
        else if (CurrentAmmo < _bulletSprites.Count)
        {
            for (int i = 0; i < _bulletSprites.Count - CurrentAmmo; i++)
            {
                _bulletSprites[i].gameObject.SetActive(true);
            }

            // increase the Height of the 'AmmoCount_Panel'-Object respective to its active Bullet-Sprite-ChildObjects
            rectTrans.sizeDelta = new Vector2(rectTrans.rect.width, rectTrans.rect.height + (float)CurrentAmmo * transform.GetChild(0).GetComponent<RectTransform>().rect.width);
        }
    }

    //todo: rework following Method; JM (08.04.2024)
    // Link to the Shoot Function from the PlayerShoot script
    /// <summary>
    /// Decreases the <see cref="CurrentAmmo"/> Value and disables the BulletSprites in the Ammo-UI.
    /// </summary>
    public void DecreaseAmmo()
    {
        if (!_isReloading && CurrentAmmo > 0)
        {
            _bulletSprites[CurrentAmmo - 1].gameObject.SetActive(false);
            CurrentAmmo--;
        }
    }

    // Link to the Reload function from the PlayerShoot Script
    public void Reload()
    {
        if (CurrentAmmo < 30 && !_isReloading)
        {
            StartCoroutine(EnableBulletsDuringReload());
        }

    }


    IEnumerator EnableBulletsDuringReload()
    {
        _isReloading = true; // Set reloading flag to true
        float timePerBullet = 1.0f / 30.0f; // Time for each bullet to enable
        for (int i = CurrentAmmo; i < 30; i++)
        {
            _bulletSprites[i].gameObject.SetActive(true);
            CurrentAmmo++;
            yield return new WaitForSeconds(timePerBullet);
        }
        _isReloading = false; // Set reloading flag to false when the reload is complete
    }
}
