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
        internal BaseWeapon WeaponInHeand { get => _weaponInHeand; private set => _weaponInHeand = value; }


        // ---------- Methods ----------
        private void OnEnable()
        {
            // initialize the specifiv weapon objects
            SMG = new SubMachineGun("SMG", 1, 0.5f, 30, 30, 1);   // Values => Name, damage, fire rate, mag size, current rounds in mag, spawned projectiles on shoot
            Shotgun = new ShotGun("Shotgun", 1, 1, 8, 8, 5);
            Handgun = new HandCannon("Handgun", 1, 1.5f, 6, 6, 1);
            ELauncher = new EnergyLauncher("Launcher", 1, 1, 1, 1, 1);

            // initializing the List with the specific weapon objects
            _weapons = new List<BaseWeapon>() { SMG, Shotgun, Handgun, ELauncher };

            // monitoring for deugging reasons
            for (int i = 0; i < _weapons.Count; i++) 
                Debug.Log($"Contents of equipedWeapos List in '{this}': Idx:'<color=lime>{i}</color>', Content: '<color=lime>{_weapons[i].WeaponName}</color>");
        }


        /// <summary>
        /// Sets the <see cref="WeaponInHeand"/> with new values according to the picked up weapon Parameter 'nameOfPickedUpWeapon' needs to be one of those: "SMG", "Shotgun", "Handgun",
        /// "Launcher". 
        /// </summary>
        internal void OnWeaponPickup(string nameOfPickedupWeapon)
        {
            //todo: re´work this method with an more elegant queue than this mess when it's maybe less late (should work for now though); JM (29.01.2024) 
            //debugging check
            if (nameOfPickedupWeapon != _weapons[0].WeaponName || nameOfPickedupWeapon != _weapons[1].WeaponName || 
                nameOfPickedupWeapon != _weapons[2].WeaponName || nameOfPickedupWeapon != _weapons[3].WeaponName)
            {
                Debug.LogError($"CAUTION! '<color=orange>{nameOfPickedupWeapon}</color>' does not fit any of the weapon object names in the List '<color=silver>{_weapons}</color>'!");
                return;
            }

            // if debugging check was passed without bug check List for names
            for (int i = 0; i < _weapons.Count; i++)
            {
                if (nameOfPickedupWeapon == _weapons[i].WeaponName)
                {
                    WeaponInHeand = _weapons[i];
                    Debug.Log($"Weapon in Hand was set to '<color=cyan>{WeaponInHeand.WeaponName}</color>' 'cause '<color=lime>{_weapons[i].WeaponName}</color>' was picked up.");
                    return;
                }
            }
        }

        /// <summary>
        /// Updates the the Value for <see cref="BaseWeapon.CurrentRoundsInMag"/> according to transmitted values.
        /// </summary>
        /// <param name="newRoundsInMag">The actual amount of round to be in the mag</param>
        internal void UpdateRoundsInMag(int newRoundsInMag)
        {
            WeaponInHeand.CurrentRoundsInMag = newRoundsInMag;
        }

        /// <summary>
        /// Switches between the two equiped Weapon
        /// </summary>
        internal void SwitchWeapon()
        {
            BaseWeapon cacheWeapon = WeaponInHeand;
            WeaponInHeand = _holsteredWeapon;
            _holsteredWeapon = cacheWeapon;
            //Debug.Log($"Cache Weapon should be _weapon in Hand (currently SMG) -> actual weapon cached = <color=magenta>'{cacheWeapon.WeaponName}'</color>");
        }
    }
}