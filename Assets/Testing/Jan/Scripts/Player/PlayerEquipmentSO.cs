using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player/Equipment", fileName = "PlayerEquipment")]
    public class PlayerEquipmentSO : ScriptableObject
    {
        // ---------- Fields ----------
        private BaseWeapon _weaponInHeand;
        private BaseWeapon _holsteredWeapon;

        private SubMachineGun SMG;
        private ShotGun Shotgun;
        private HandCannon Handgun;
        private EnergyLauncher ELauncher;

        private List<BaseWeapon> _weapons;

        // --- Properties ---
       

        // ---------- Methods ----------
        private void OnEnable()
        {
            // initializing the List with the specific Weapons
            SMG = new SubMachineGun("SMG", 1, 0.5f, 30, 30, 1);   // Values => Name, damage, fire rate, mag size, current rounds in mag, spawned projectiles on shoot
            Shotgun = new ShotGun("Shotgun", 1, 1, 8, 8, 5);
            Handgun = new HandCannon("Handgun", 1, 1.5f, 6, 6, 1);
            ELauncher = new EnergyLauncher("Launcher", 1, 1, 1, 1, 1);

            _weapons = new List<BaseWeapon>() { SMG, Shotgun, Handgun, ELauncher };

            for (int i = 0; i < _weapons.Count; i++) 
                Debug.Log($"Contents of equipedWeapos List in '{this}': Idx:'<color=lime>{i}</color>', Content: '<color=lime>{_weapons[i].WeaponName}</color>");
        }


        /// <summary>
        /// Sets the <see cref="_weapons"/> with new values
        /// </summary>
        internal void OnWeaponPickup()
        {
            // fill with Logic
        }

        /// <summary>
        /// Updates the the Value for <see cref="BaseWeapon.CurrentRoundsInMag"/> according to transmitted values.
        /// </summary>
        /// <param name="newRoundsInMag">The actual amount of round to be in the mag</param>
        internal void UpdateRoundsInMag(int newRoundsInMag)
        {
            _weaponInHeand.CurrentRoundsInMag = newRoundsInMag;
        }

        /// <summary>
        /// Switches between the two equiped Weapon
        /// </summary>
        internal void SwitchWeapon()
        {
            BaseWeapon cacheWeapon = _weaponInHeand;
            _weaponInHeand = _holsteredWeapon;
            _holsteredWeapon = cacheWeapon;
            //Debug.Log($"Cache Weapon should be _weapon in Hand (currently SMG) -> actual weapon cached = <color=magenta>'{cacheWeapon.WeaponName}'</color>");
        }
    }
}