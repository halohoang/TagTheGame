using JansLittleHelper;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
        {
            GetObject();
        }

        if (_reloadHintTxt == null)
        {
            for (int i = 0; i < _uITextObjects.Length; i++)
            {
                if (_uITextObjects[i].name == "ReloadHint_Text (TMP)")
                {
                    _ammoDisplayTxt = _uITextObjects[i].GetComponent<TextMeshProUGUI>();
                }
            }
        }

        if (_uIWeaponImg == null)
        {

        }
    }

    private void GetObject()
    {
        for (int i = 0; i < _uITextObjects.Length; i++)
        {
            if (_uITextObjects[i].name == "AmmoDisplay_Text (TMP)")
            {
                _ammoDisplayTxt = _uITextObjects[i].GetComponent<TextMeshProUGUI>();
            }
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
    #endregion

    #region Custom Methods

    #endregion

    #endregion
}