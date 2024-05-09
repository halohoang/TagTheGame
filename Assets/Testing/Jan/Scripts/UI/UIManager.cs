using JansLittleHelper;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Events
    //--------------------------------
    // - - - - -  E V E N T S  - - - - 
    //--------------------------------


    #endregion


    #region Variables
    //--------------------------------------
    // - - - - -  V A R I A B L E S  - - - - 
    //--------------------------------------
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _ammoDisplayTxt;
    [SerializeField] private TextMeshProUGUI _reloadHintTxt;
    [SerializeField] private Image _uIWeaponImg;
    [Space(5)]

    [Header("Settings")]
    [SerializeField] private int _reloadHintThreshhold = 5;
    [Space(5)]

    [Header("Monitoring Values")]
    [SerializeField, ReadOnly] private int _currentAmmo;
    [SerializeField, ReadOnly] private int _maxAmmo;
    [SerializeField, ReadOnly] private bool _isReloading;

    private GameObject[] _uITextObjects;

    #endregion

    #region Methods
    //----------------------------------
    // - - - - -  M E T H O D S  - - - - 
    //----------------------------------

    #region Unity-provided Methods

    private void Awake()
    {
        // array instantiation
        _uITextObjects = GameObject.FindGameObjectsWithTag("UI_variableText");

        // Autoreferencing
        if (_ammoDisplayTxt == null)
            _ammoDisplayTxt = NullChecksAndAutoReferencing.GetTMPFromTagList(_uITextObjects, "AmmoDisplay_Text (TMP)");

        if (_reloadHintTxt == null)
            _reloadHintTxt = NullChecksAndAutoReferencing.GetTMPFromTagList(_uITextObjects, "ReloadHint_Text (TMP)");
        //todo: rework in case more than just one variable Image should be used in UI; JM (09.05.24)

        if (_uIWeaponImg == null)
            _uIWeaponImg = GameObject.FindGameObjectWithTag("UI_variableImage").GetComponent<Image>();
        //todo: rework in case more than just one variable Image should be used in UI; JM (09.05.24)
    }

    private void OnEnable()
    {
        PlayerWeaponHandling.OnSetBulletCount += UpdateAmmoDisplay;
        PlayerWeaponHandling.OnBulletsInstantiated += UpdateAmmoDisplay;
        PlayerWeaponHandling.OnReload += UpdateAmmoDisplay;
    }
    private void OnDisable()
    {
        PlayerWeaponHandling.OnSetBulletCount -= UpdateAmmoDisplay;
        PlayerWeaponHandling.OnBulletsInstantiated -= UpdateAmmoDisplay;
        PlayerWeaponHandling.OnReload -= UpdateAmmoDisplay;
    }

    void Start()
    {
        // disable Reaload Hint Object
        _reloadHintTxt.enabled = false;
    }

    void Update()
    {

    }
    #endregion

    #region Custom Methods

    private void UpdateAmmoDisplay(int currentAmmo)
    {
        _currentAmmo = currentAmmo;
        _ammoDisplayTxt.text = $"{_currentAmmo}|{_maxAmmo}";

        EnableDisableRealoadHintObj();
    }
    private void UpdateAmmoDisplay(int currentAmmo, int maxAmmo)
    {
        _currentAmmo = currentAmmo;
        _maxAmmo = maxAmmo;
        _ammoDisplayTxt.text = $"{_currentAmmo}|{_maxAmmo}";

        EnableDisableRealoadHintObj();
    }

    private void EnableDisableRealoadHintObj()
    {
        if (_currentAmmo < _reloadHintThreshhold)
            _reloadHintTxt.enabled = true;
        else
            _reloadHintTxt.enabled = false;
    }

    #endregion

    #endregion
}