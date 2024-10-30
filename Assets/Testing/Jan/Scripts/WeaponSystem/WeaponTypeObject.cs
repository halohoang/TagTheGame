using EnumLibrary;
using NaughtyAttributes;
using UnityEngine;

public class WeaponTypeObject : MonoBehaviour
{
    // ---------- Variables ----------  
    #region Tooltip
    [Tooltip("Defines which weapon type this object shall be or for which weapon type this ammo object shall be for.")]
    #endregion
    [SerializeField] private Enum_Lib.EWeaponType _weaponType;    // The type of the weapon
    #region Tooltip
    [Tooltip("Defines if this object shall be a pickupable Ammo object. If this is unchecked this object will be recognized as an actual weapon of the above specified type.")]
    #endregion
    [SerializeField] private bool _isAmmo;
    [Tooltip("The amount of ammo that shall be pickedup by colliding with this object if 'isAmmo' is checked.")]
    [SerializeField, EnableIf("_isAmmo")] private int _amountOfAmmo = 5;



    // --- Properties ---
    internal Enum_Lib.EWeaponType WeaponType { get => _weaponType; private set => _weaponType = value; }    // for getting the Name of the specific weapon object
    internal bool IsAmmo { get => _isAmmo; private set => _isAmmo = value; }
    internal int AmountOfAmmo { get => _amountOfAmmo; private set => _amountOfAmmo = value; }
}
