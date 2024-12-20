using JansLittleHelper;
using NaughtyAttributes;
using Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
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
        #region Tooltip
        [Tooltip("The TextMeshPro-Component of the RoundsDisplay UI (Found in Hierarchy -> /UI/Ingame-UI_Canvas/WeaponUI_Panel/AmmoDisplay_Panel/RoundsDisplay_Text (TMP))")]
        #endregion
        [SerializeField] private TextMeshProUGUI _roundsInMagDisplayTxt;
        #region Tooltip
        [Tooltip("The TextMeshPro-Component of the StoredAmmoDisplay UI (Found in Hierarchy -> /UI/Ingame-UI_Canvas/WeaponUI_Panel/AmmoDisplay_Panel/StoredAmmoDisplay_Text (TMP))")]
        #endregion
        [SerializeField] private TextMeshProUGUI _storedAmmoDisplayTxt;
        #region Tooltip
        [Tooltip("The TextMeshPro-Component of the ReloadHint UI (Found in Hierarchy -> /UI/Ingame-UI_Canvas/WeaponUI_Panel/AmmoDisplay_Panel/ReloadHint_Text (TMP))")]
        #endregion
        [SerializeField] private TextMeshProUGUI _reloadHintTxt;
        #region Tooltip
        [Tooltip("The Image-Component of the WeaponDepiction-UI (Found in Hierarchy -> /UI/Ingame-UI_Canvas/WeaponUI_Panel/WeaponDepiction_Panel_Panel/WeaponDepiction_Img)")]
        #endregion
        [SerializeField] private Image _uIWeaponImg;
        #region Tooltip
        [Tooltip("GodMode TMP (Found in Hierarchy -> /UI/Ingame-UI_Canvas/Testing_&_Debug_Panel/ActiveCheats_Panel/GodMode_Text (TMP))")]
        #endregion
        [SerializeField] private TextMeshProUGUI _godeModeTxt;
        #region Tooltip
        [Tooltip("timeScale TMP (Found in Hierarchy -> /UI/Ingame-UI_Canvas/Testing_&_Debug_Panel/ActiveCheats_Panel/TimeScaleInfo_Text (TMP))")]
        #endregion
        [SerializeField] private TextMeshProUGUI _timeScleTxt;
        [Space(5)]

        [Header("Monitoring Values")]
        #region Tooltip
        [Tooltip("The actual current ammount of rounds in the selected guns magazine, should be updated according to when the player is shooting or reloading.")]
        #endregion
        [SerializeField, ReadOnly] private int _currentRoundsInMag;
        #region Tooltip
        [Tooltip("The actual current ammount of ammo, which is stored in the players inventory for the currently selected Gun. Should be updated according to when the player is shooting or reloading.")]
        #endregion
        [SerializeField, ReadOnly] private int _currentStoredAmmo;
        #region Tooltip
        [Tooltip("The maximum Ammo should be equal to the Magazine Size of the currently selected Weapon.")]
        #endregion
        [SerializeField, ReadOnly] private int _maxAmmo;
        #region Tooltip
        [Tooltip("Is the player currently reloading?")]
        #endregion
        [SerializeField, ReadOnly] private bool _isReloading;
        #region Tooltip
        [Tooltip("The threshold of the currently selected weapon for when to show the reload hint to inform the palyer that reloading is reccomended.")]
        #endregion
        [SerializeField, ReadOnly] private int _selectedWeaponReloadThreshold;
        #region Tooltip
        [Tooltip("Is the god mode currently enabled and player invincible?")]
        #endregion
        [SerializeField, ReadOnly] private bool _isGodModeEnabled;
        #region Tooltip
        [Tooltip("Is the TimeScale currently manipulated (increased or decreased)?")]
        #endregion
        [SerializeField, ReadOnly] private bool _isTimeScaleManipulated;

        private GameObject[] _uITextObjects;                                        // stores the UI-Text-Objects for quicker referencing and access
        [SerializeField, ReadOnly] private Sprite[] _weaponSprites;                 // stores the Weapon Sprites for quicker referencing and access

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

            // instantiate and load Weapon Textures for UI-Weapondisplay
            _weaponSprites = Resources.LoadAll<Sprite>("Sprites/WeaponSprites");

            // Autoreferencing
            if (_roundsInMagDisplayTxt == null)
                _roundsInMagDisplayTxt = NullChecksAndAutoReferencing.GetTMPFromTagList(_uITextObjects, "RoundsInMagDisplay_Text (TMP)");

            if (_storedAmmoDisplayTxt == null)
                _storedAmmoDisplayTxt = NullChecksAndAutoReferencing.GetTMPFromTagList(_uITextObjects, "StoredAmmoDisplay_Text (TMP)");

            if (_reloadHintTxt == null)
                _reloadHintTxt = NullChecksAndAutoReferencing.GetTMPFromTagList(_uITextObjects, "ReloadHint_Text (TMP)");
            //todo: rework in case more than just one variable Image should be used in UI; JM (09.05.24)

            if (_uIWeaponImg == null)
                _uIWeaponImg = GameObject.FindGameObjectWithTag("UI_variableImage").GetComponent<Image>();
            //todo: rework in case more than just one variable Image should be used in UI; JM (09.05.24)
        }

        private void OnEnable()
        {
            PlayerWeaponHandling.OnSetBulletCount += UpdateRoundsDisplay;
            PlayerWeaponHandling.OnBulletsInstantiated += UpdateRoundsDisplay;
            PlayerWeaponHandling.OnSetStoredAmmoCount += UpdateStoredAmmoDisplay;
            PlayerWeaponHandling.OnReload += UpdateRoundsAndStoredAmmoDisplay;
            PlayerWeaponHandling.OnWeaponEquip += UpdateWeaponDisplay;

            // Cheat Panel related
            CheatInput.OnSetGodMode += SetGodModeHint;
            CheatInput.OnTimeScaleChange += SetTimeScaleHint;
        }
        private void OnDisable()
        {
            PlayerWeaponHandling.OnSetBulletCount -= UpdateRoundsDisplay;
            PlayerWeaponHandling.OnBulletsInstantiated -= UpdateRoundsDisplay;
            PlayerWeaponHandling.OnSetStoredAmmoCount -= UpdateStoredAmmoDisplay;
            PlayerWeaponHandling.OnReload -= UpdateRoundsAndStoredAmmoDisplay;
            PlayerWeaponHandling.OnWeaponEquip -= UpdateWeaponDisplay;

            // Cheat Panel related
            CheatInput.OnSetGodMode -= SetGodModeHint;
            CheatInput.OnTimeScaleChange -= SetTimeScaleHint;
        }

        void Start()
        {
            // Set god mode hint
            _godeModeTxt.enabled = _isGodModeEnabled;

            // disable Reaload Hint Object
            _reloadHintTxt.enabled = false;
        }
        #endregion


        #region Custom Methods

        /// <summary>
        /// Updates the Ammo Display in the ingame UI-Panel respectively to the transmitted 'currentAmmo'-value.
        /// Also checks on Updating if ReloadHintThereshold was reached.
        /// </summary>
        /// <param name="currentRounds"></param>
        private void UpdateRoundsDisplay(int currentRounds)
        {
            _currentRoundsInMag = currentRounds;
            _roundsInMagDisplayTxt.text = $"{_currentRoundsInMag}";

            EnableDisableRealoadHintObj(_selectedWeaponReloadThreshold);
        }

        /// <summary>
        /// Updates the Ammo Display in the ingame UI-Panel respectively to the transmitted '_currentStoredAmmo'-value.
        /// Also checks on Updating if ReloadHintThereshold was reached.
        /// </summary>
        /// <param name="currentAmmo"></param>
        private void UpdateStoredAmmoDisplay(int currentAmmo)
        {
            _currentStoredAmmo = currentAmmo;
            _storedAmmoDisplayTxt.text = $"{_currentStoredAmmo}";
        }

        /// <summary>
        /// Updates the Ammo Display in the ingame UI-Panel respectively to the transmitted 'currentAmmo'-value.
        /// Also checks on Updating if ReloadHintThereshold was reached.
        /// Updates the Ammo Display in the ingame UI-Panel respectively to the transmitted '_currentStoredAmmo'-value.
        /// </summary>
        /// <param name="currentRounds"></param>
        /// <param name="currentAmmo"></param>
        private void UpdateRoundsAndStoredAmmoDisplay(int currentRounds, int currentAmmo)
        {
            _currentRoundsInMag = currentRounds;
            _roundsInMagDisplayTxt.text = $"{_currentRoundsInMag}";
            EnableDisableRealoadHintObj(_selectedWeaponReloadThreshold);

            _currentStoredAmmo = currentAmmo;
            _storedAmmoDisplayTxt.text = $"{_currentStoredAmmo}";
        }

        /// <summary>
        /// Updates image in ingame UI weapon disply panel respectively to the transmitted tape of the weapon.
        /// Also checks on Updating if ReloadHintThereshold was reached.
        /// </summary>
        /// <param name="weaponType"></param>
        private void UpdateWeaponDisplay(BaseWeapon weapon)
        {
            for (int i = 0; i < _weaponSprites.Length; i++)
            {
                if (_weaponSprites[i].name == $"Sprite{weapon.WeaponType}")
                {
                    _uIWeaponImg.sprite = _weaponSprites[i];
                }
            }
            _selectedWeaponReloadThreshold = weapon.ReloadHintThreshhold;              // to get access to the ReloadTHreshold of the currently selected weaopn

            EnableDisableRealoadHintObj(weapon.ReloadHintThreshhold);
        }

        /// <summary>
        /// Enables or disables the reload hint TMP-Object in the ingame UI respectively to the transmitted 'reloadHintThreshold'-value
        /// </summary>
        /// <param name="reloadHintThreshold"></param>
        private void EnableDisableRealoadHintObj(int reloadHintThreshold)
        {
            if (_currentRoundsInMag <= reloadHintThreshold)
                _reloadHintTxt.enabled = true;
            else
                _reloadHintTxt.enabled = false;
        }

        /// <summary>
        /// En-/Disables the Text-Hint depicting whether God Mode is active or not
        /// </summary>
        /// <param name="godModeStatus"></param>
        private void SetGodModeHint(bool godModeStatus)
        {
            _godeModeTxt.enabled = godModeStatus;
            _isGodModeEnabled = godModeStatus;
        }

        /// <summary>
        /// Enables TimeScaleHint-Text in Cheat-Console-UI to inform about current TimeScaleSetting on TimeScale-Manipulation
        /// </summary>
        /// <param name="arg0"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SetTimeScaleHint(float curTSSetting)
        {
            _timeScleTxt.enabled = true;
            _isTimeScaleManipulated = true;
            _timeScleTxt.text = $"TimeScale: {curTSSetting}";

            if (curTSSetting == 1)
                _isTimeScaleManipulated = false;
        }

        #endregion

        #endregion
    }
}