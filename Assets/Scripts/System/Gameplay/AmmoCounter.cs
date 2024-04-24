using NaughtyAttributes;
using ScriptableObjects;
using System;
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
    [SerializeField] private int _magazineSize;
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
    internal int MagazineSize { get => _magazineSize; set => _magazineSize = value; }

    private void Awake()
    {
        // auto Referencing
        if (_playerEquipmentSO == null)
        {
            _playerEquipmentSO = Resources.Load("ScriptableObjects/PlayerEquipment") as PlayerEquipmentSO;
            Debug.Log($"<color=yellow>Caution!</color>: Reference for ScriptableObject 'PlayerEquipment' in Inspector of {this} was not set. So it was Set automatically.");
        }
    }

    private void OnEnable()
    {
        PlayerWeaponHandling.OnSetBulletCount += OnBulletCountChange;
        PlayerWeaponHandling.OnBulletsnstantiated += DecreaseAmmo;
        PlayerWeaponHandling.OnReload += Reload;
    }
    private void OnDisable()
    {
        PlayerWeaponHandling.OnSetBulletCount -= OnBulletCountChange;
        PlayerWeaponHandling.OnBulletsnstantiated -= DecreaseAmmo;
        PlayerWeaponHandling.OnReload -= Reload;
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
    }

    private void Update()
    {
        //if(CurrentAmmo < 30)
        //{
        //	_reloadHint.SetActive(true);
        //}
        //if (CurrentAmmo >= 30) { _reloadHint.SetActive(false); }
    }

    private void OnBulletCountChange(int ammoCount, int magazineSize)
    {
        // 1. First update Bullet count related values
        CurrentAmmo = ammoCount;
        MagazineSize = magazineSize;

        // 2. Set Ammo-related Ui respectively to updated Ammo Values
        SetUIAmmoToActiveWeaponAmmo();
    }

    /// <summary>
    /// Sets the UI shown Ammo Bullets respective to the actual AmmoCount of the selected Weapon
    /// </summary>
    public void SetUIAmmoToActiveWeaponAmmo()
    {
        // 1. Get Reference to the RectTransform-Component of this UI-GameObject
        RectTransform rectTrans = gameObject.GetComponent<RectTransform>();

        // 2. clear _active/_inactive-Lists and sort active and inactive BulletSprites in the '_bulletSprites'-List a new
        _activeSprites.Clear();
        _inactiveSprites.Clear();
        Debug.Log($"'SetUIAmmoToActiveWeaponAmmo()' was called");

        for (int i = 0; i < _bulletSprites.Count; i++)
        {
            if (_bulletSprites[i].gameObject.activeSelf)
            {
                _activeSprites.Add(_bulletSprites[i]);
            }
            else
            {
                _inactiveSprites.Add(_bulletSprites[i]);
            }
            Debug.Log($"ActiveState of BulletSprites-List-Element on Idx: '<color=lime>{_bulletSprites[i]}</color>' = '<color=lime>{_bulletSprites[i].gameObject.activeSelf}</color>'");
        }

        // 3. enable/disable the BulletSprites Object in the '_bulletSprites'-List respectively to if they are active or not and accroding to the 'AmmountCount'
        // if ammoCount is greater or less than the Count of the SpriteObjects in the '_bulletSprites'-List enable or disable respectively to the difference between both counts
        if (_activeSprites.Count > 0 && _activeSprites.Count > CurrentAmmo)
        {
            Debug.Log($"_activeSprites.Count('<color=magenta>{_activeSprites.Count}</color>') is creater than Current Ammo ('<color=magenta>{CurrentAmmo}</color>')");
            // a.) disable the difference amount between '_activeSpriter.Count' and 'CurrentAmmo' from '_activeSprites'_List and shift them to '_inactiveSprites'-List
            for (int i = 0; i < _activeSprites.Count - CurrentAmmo; i++)
            {
                //disable Sprite -> then insert this disabled Obj to inactiveSprite List -> then remove disabled Object from activeSprite-List
                _activeSprites[i].gameObject.SetActive(false);
                Debug.Log($"Deactivated BulletSprite '<color=orange>{_activeSprites[i].name}</color>' at Idx: '<color=orange>{i}</color>'");

                if (_inactiveSprites.Count > i)
                {
                    _inactiveSprites.Insert(i, _activeSprites[i]);
                    Debug.Log($"inserted BulletSprite '<color=orange>{_activeSprites[i].name}</color>' at Idx: '<color=orange>{i}</color>' to _inactiveSprites-List: ('<color=orange>{_inactiveSprites[i].name}</color>', Idx: '<color=orange>{i}</color>')");
                }
                else
                {
                    _inactiveSprites.Add(_activeSprites[i]);
                    Debug.Log($"_inactiveSprites-List did not contain element at Idx: '<color=orange>{_inactiveSprites[i]}</color>' thus '<color=orange>{_activeSprites[i].name}</color>' was added at the end of the list");
                }
                //Debug.Log($"Remove deactivated BulletSprite ('<color=orange>{_activeSprites[i].name}</color>') from _activeSprites-List at Idx: '<color=orange>{i}</color>'");
                //_activeSprites.RemoveAt(i);
            }

            // b.) remove every disabled object from '_activeSprites'-List
            for (int i = _activeSprites.Count; i > 0; i--)
            {
                if (!_activeSprites[i - 1].gameObject.activeSelf)
                {
                    Debug.Log($"Remove deactivated BulletSprite ('<color=orange>{_activeSprites[i - 1].name}</color>') from _activeSprites-List at Idx: '<color=orange>{i}</color>'");
                    _activeSprites.RemoveAt(i);
                }
            }

            // decrease the Height of the 'AmmoCount_Panel'-Object respective to its active Bullet-Sprite-ChildObjects
            //rectTrans.sizeDelta = new Vector2(rectTrans.rect.width, rectTrans.rect.height - (float)CurrentAmmo * transform.GetChild(0).GetComponent<RectTransform>().rect.width);
        }
        else if (_activeSprites.Count > 0 && _activeSprites.Count < CurrentAmmo)
        {
            Debug.Log($"'<color=magenta>{CurrentAmmo - _activeSprites.Count}</color>'");
            for (int i = 0; i < CurrentAmmo - _activeSprites.Count; i++)
            {
                //enable Sprite -> then insert this enabled Obj to activeSprite List -> then remove this enabled Object from inactiveSprite-List
                _inactiveSprites[i].gameObject.SetActive(true);
                Debug.Log($"Activated BulletSprite '<color=lime>{_inactiveSprites[i].name}</color>' at Idx: '<color=lime>{i}</color>'");

                if (_inactiveSprites.Count > i)
                {
                    _activeSprites.Insert(i, _inactiveSprites[i]);
                    Debug.Log($"inserted BulletSprite '<color=lime>{_inactiveSprites[i].name}</color>' at Idx: '<color=lime>{i}</color>' to _inactiveSprites-List: ('<color=lime>{_activeSprites[i].name}</color>', Idx: '<color=lime>{i}</color>')");
                }
                else
                {
                    _inactiveSprites.Add(_activeSprites[i]);
                    Debug.Log($"_inactiveSprites-List did not contain element at Idx: '<color=orange>{_inactiveSprites[i]}</color>' thus '<color=orange>{_activeSprites[i].name}</color>' was added at the end of the list");
                }

                //_inactiveSprites.RemoveAt(i);
                //Debug.Log($"Removed activated BulletSprite from '<color=lime>{_inactiveSprites[i].name}</color>' at Idx: '<color=lime>{i}</color>'");
            }

            // b.) remove every disabled object from '_activeSprites'-List
            for (int i = 0; i < _inactiveSprites.Count; i++)
            {
                if (_inactiveSprites[i].gameObject.activeSelf)
                {
                    Debug.Log($"Remove deactivated BulletSprite ('<color=orange>{_inactiveSprites[i].name}</color>') from _inactiveSprites-List at Idx: '<color=orange>{i}</color>'");
                    _inactiveSprites.RemoveAt(i);
                }
            }

            //// increase the Height of the 'AmmoCount_Panel'-Object respective to its active Bullet-Sprite-ChildObjects
            //rectTrans.sizeDelta = new Vector2(rectTrans.rect.width, rectTrans.rect.height + (float)CurrentAmmo * transform.GetChild(0).GetComponent<RectTransform>().rect.width);
        }
    }

    //todo: rework following Method; JM (08.04.2024)
    // Link to the Shoot Function from the PlayerShoot script
    /// <summary>
    /// Decreases the <see cref="CurrentAmmo"/> Value and disables the BulletSprites in the Ammo-UI.
    /// </summary>
    public void DecreaseAmmo(int updatedBulletCount)
    {
        CurrentAmmo = updatedBulletCount;

        if (!_isReloading && CurrentAmmo > 0)
        {
            _activeSprites[CurrentAmmo - 1].gameObject.SetActive(false);    
            
            // todo: continue here with fixed of the 'out of range' exception on fireing
            if (_inactiveSprites.Count > CurrentAmmo - 1)
            {
                _inactiveSprites.Insert(CurrentAmmo - 1, _activeSprites[CurrentAmmo - 1]);
                Debug.Log($"inserted BulletSprite '<color=orange>{_activeSprites[CurrentAmmo - 1].name}</color>' at Idx: '<color=orange>{CurrentAmmo - 1}</color>' to _inactiveSprites-List: ('<color=orange>{_inactiveSprites[CurrentAmmo - 1].name}</color>', Idx: '<color=orange>{CurrentAmmo - 1}</color>')");
            }
            else
            {
                _inactiveSprites.Add(_activeSprites[CurrentAmmo - 1]);
                Debug.Log($"_inactiveSprites-List did not contain element at Idx: '<color=orange>{_inactiveSprites[CurrentAmmo - 1]}</color>' thus '<color=orange>{_activeSprites[CurrentAmmo - 1].name}</color>' was added at the end of the list");
            }
        }
    }

    // Link to the Reload function from the PlayerShoot Script
    public void Reload()
    {
        if (CurrentAmmo < MagazineSize && !_isReloading)
        {
            StartCoroutine(EnableBulletsDuringReload());
        }
    }

    IEnumerator EnableBulletsDuringReload()
    {
        _isReloading = true; // Set reloading flag to true
        float timePerBullet = 1.0f / (float)MagazineSize; // Time for each bullet to enable
        for (int i = CurrentAmmo; i < MagazineSize; i++)
        {
            _bulletSprites[i].gameObject.SetActive(true);
            CurrentAmmo++;
            yield return new WaitForSeconds(timePerBullet);
        }
        _isReloading = false; // Set reloading flag to false when the reload is complete
    }
}
