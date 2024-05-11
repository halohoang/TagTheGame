using EnumLibrary;
using UnityEngine;

public class WeaponTypeObject : MonoBehaviour
{
    // ---------- Variables ----------  
    [SerializeField] private Enum_Lib.EWeaponType _weaponType;    // The type of the weapon

    // --- Properties ---
    internal Enum_Lib.EWeaponType WeaponType { get => _weaponType; private set => _weaponType = value; }    // for getting the Name of the specific weapon object
}
