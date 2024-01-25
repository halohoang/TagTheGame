using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwapWeaopn : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReaderSO _inputReaderSO;

    private void Awake()
    {
        if (_inputReaderSO == null)
        {
            _inputReaderSO = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;
            Debug.Log($"<color=yellow>Caution!</color>: Reference for InputReader in Inspector of {this} was not set. So it was Set automatically, if you want or need to set a specific " +
                $"InputReader Asset, set it manually instead.");
        }
    }

    private void OnEnable()
    {
        _inputReaderSO.OnWeaponSwitch += SwitchWeapon;
    }
    private void OnDisable()
    {
        _inputReaderSO.OnWeaponSwitch -= SwitchWeapon;
    }

    private void SwitchWeapon()
    {
        // Update UI

        // Update PlayerEquipmentSO
    }
}
