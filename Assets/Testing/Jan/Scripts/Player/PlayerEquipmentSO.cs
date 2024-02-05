using EnumLibrary;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player/Equipment", fileName = "PlayerEquipment")]
    public class PlayerEquipmentSO : ScriptableObject
    {
        // ---------- Fields ----------
        [SerializeField] private WeaponTypeValues[] _weaponValues;

        private BaseWeapon _weaponInHeand;
        private BaseWeapon _holsteredWeapon;

        private SubMachineGun SMG;
        private ShotGun Shotgun;
        private HandCannon Handgun;
        private EnergyLauncher ELauncher;

        private List<BaseWeapon> _weapons;


        // --- Properties ---
        internal BaseWeapon WeaponInHeand { get => _weaponInHeand; private set => _weaponInHeand = value; }

        // ---------- Structs ----------
        [System.Serializable]
        private struct WeaponTypeValues
        {
            // Fields
            [Tooltip("The name of the weapon (ideally should be equal to the WeaponType).")]
            [SerializeField, ReadOnly, AllowNesting] private string _weaponName;
            [Tooltip("The type of the weapon.")]
            [SerializeField, ReadOnly, AllowNesting] private Enum_Lib.EWeaponType _weaponType;
            [Tooltip("The damage dealt by the weapon.")]
            [SerializeField] private float _weaponDamage;
            [Tooltip("The rate of bullets spawning")]
            [SerializeField] private float _fireRate;
            [Tooltip("The amount of rounds the weapons magazine can store.")]
            [SerializeField] private int _magazineSize;
            [Tooltip("The inital amount of rounds in the magazine.")]
            [SerializeField] private int _currentRoundsInMag;
            [Tooltip("The Amount of Bullets that will be spawned simultaneously (e.g. for the Shotgun it might be '5' and for the Handgun '1').")]
            [SerializeField] private int _spawnedBullets;


            // Properties
            internal string WeaponName { get => _weaponName; set => _weaponName = value; }
            internal Enum_Lib.EWeaponType WeaponType { get => _weaponType; set => _weaponType = value; }
            internal float WeaponDamage { get => _weaponDamage; set => _weaponDamage = value; }
            internal float FireRate { get => _fireRate; set => _fireRate = value; }
            internal int MagazineSize { get => _magazineSize; set => _magazineSize = value; }
            internal int CurrentRoundsInMag { get => _currentRoundsInMag; set => _currentRoundsInMag = value; }
            internal int SpawnedBullets { get => _spawnedBullets; set => _spawnedBullets = value; }
        }

        // ---------- Methods ----------
        private void OnEnable()
        {
            // 1. initializing single weapon objects with standard values automatically
            Handgun = new HandCannon("Handgun", Enum_Lib.EWeaponType.Handgun, 1, 1.5f, 6, 6, 1);
            SMG = new SubMachineGun("SMG", Enum_Lib.EWeaponType.SMG, 1, 0.5f, 30, 30, 1);
            Shotgun = new ShotGun("Shotgun", Enum_Lib.EWeaponType.Shotgun, 1, 1, 8, 8, 5);
            ELauncher = new EnergyLauncher("Energy Launcher", Enum_Lib.EWeaponType.EnergyLauncher, 1, 1, 1, 1, 1);            

            // 2. initializing the List with the specific weapon objects and their standard values
            _weapons = new List<BaseWeapon>() { Handgun, SMG, Shotgun, ELauncher };

            // 3. Setting the Weapons Name and Type to the weaponValues array (in the inspector accordingly to the previously instantiated List '_weaops')            
            for (int i = 0; i < _weaponValues.Length; i++) // Sets the name and weapon type of the specific array-fields in the Inspector for the Weapon
            {
                _weaponValues[i].WeaponName = $"{_weapons[i].WeaponName} Values";
                _weaponValues[i].WeaponType = _weapons[i].WeaponType;
            }

            // 4. Set the numerical values (damage, fire rate, mag size, rounds in mag and spawned bullets) accordingly to the manually set values in the inspector of the 'PlayerEquipment' Asset
            for (int i = 0; i < _weaponValues.Length; i++)
            {
                // todo: see if that can be optimized yet, does not look soo nice; (JM, 05.02.2024)
                if (_weaponValues[i].WeaponType == Enum_Lib.EWeaponType.Handgun)
                {
                    Handgun.SetWeaponValues(_weaponValues[i].WeaponDamage, _weaponValues[i].FireRate, _weaponValues[i].MagazineSize, _weaponValues[i].CurrentRoundsInMag, _weaponValues[i].SpawnedBullets);
                }
                else if (_weaponValues[i].WeaponType == Enum_Lib.EWeaponType.SMG)
                {
                    SMG.SetWeaponValues(_weaponValues[i].WeaponDamage, _weaponValues[i].FireRate, _weaponValues[i].MagazineSize, _weaponValues[i].CurrentRoundsInMag, _weaponValues[i].SpawnedBullets);
                }
                else if (_weaponValues[i].WeaponType == Enum_Lib.EWeaponType.Shotgun)
                {
                    Shotgun.SetWeaponValues(_weaponValues[i].WeaponDamage, _weaponValues[i].FireRate, _weaponValues[i].MagazineSize, _weaponValues[i].CurrentRoundsInMag, _weaponValues[i].SpawnedBullets);
                }
                else if (_weaponValues[i].WeaponType == Enum_Lib.EWeaponType.EnergyLauncher)
                {
                    ELauncher.SetWeaponValues(_weaponValues[i].WeaponDamage, _weaponValues[i].FireRate, _weaponValues[i].MagazineSize, _weaponValues[i].CurrentRoundsInMag, _weaponValues[i].SpawnedBullets);
                }
            }

            // monitoring for deugging reasons
            for (int i = 0; i < _weapons.Count; i++)
                Debug.Log($"Content´and element values of '_weapons' List in '{this}': Idx:'<color=lime>{i}</color>', \nContent: '<color=lime>{_weapons[i].WeaponType}</color>, \n" +
                    $"weapon damage: '<color=lime>{_weaponValues[i].WeaponDamage}</color>', \nfire rate: '<color=lime>{_weaponValues[i].FireRate}</color>', \nmagazine size: '<color=lime>{_weaponValues[i].MagazineSize}</color>', \nrounds in magazine: '<color=lime>{_weaponValues[i].CurrentRoundsInMag}</color>', \nsimulaneously spawned bullets: '<color=lime>{_weaponValues[i].SpawnedBullets}</color>'");
        }


        /// <summary>
        /// Sets the <see cref="WeaponInHeand"/> with new values according to the picked up weapon.
        /// </summary>
        internal void OnWeaponPickup(Enum_Lib.EWeaponType typeOfPickedupWeapon)
        {
            //todo: re´work this method with an more elegant queue than this mess when it's maybe less late (should work for now though); JM (29.01.2024) 
            //debugging check
            if (typeOfPickedupWeapon != _weapons[0].WeaponType || typeOfPickedupWeapon != _weapons[1].WeaponType ||
                typeOfPickedupWeapon != _weapons[2].WeaponType || typeOfPickedupWeapon != _weapons[3].WeaponType)
            {
                Debug.LogError($"CAUTION! '<color=orange>{typeOfPickedupWeapon}</color>' does not fit any of the weapon object names in the List '<color=silver>{_weapons}</color>'!");
                return;
            }

            // if debugging check was passed without bug check List for names
            for (int i = 0; i < _weapons.Count; i++)
            {
                if (typeOfPickedupWeapon == _weapons[i].WeaponType)
                {
                    WeaponInHeand = _weapons[i];
                    Debug.Log($"Weapon in Hand was set to '<color=cyan>{WeaponInHeand.WeaponType}</color>' 'cause '<color=lime>{_weapons[i].WeaponType}</color>' was picked up.");
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