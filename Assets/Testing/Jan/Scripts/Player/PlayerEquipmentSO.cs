using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player/Equipment", fileName = "PlayerEquipment")]
    public class PlayerEquipmentSO : ScriptableObject
    {
        // ---------- Fields ----------
        [SerializeField] private BaseWeapon _weaponInHeand;
        [SerializeField] private BaseWeapon _holsteredWeapon;
        [SerializeField, ReadOnly] private List<BaseWeapon> _equipedWeapons = new List<BaseWeapon>();


        // ---------- Methods ----------

        /// <summary>
        /// Sets the <see cref="_equipedWeapons"/> with new values
        /// </summary>
        internal void OnWeaponPickup()
        {
            // fill with Logic
        }

        /// <summary>
        /// Updates the the Value for <see cref="BaseWeapon._currentRoundsInMag"/> according to transmitted values
        /// </summary>
        internal void UpdateRoundsInMag(int newRoundsInMag)
        {
            
        }
    }
}