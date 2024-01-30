using EnumLibrary;

/// <summary>
/// BaseClass to all specific Weapontypes ike HandCannon.cs, Shotgun.cs, SubMachineGun.cs, EnergyLauncher.cs
/// </summary>
public class BaseWeapon
{
    // ---------- Variables ----------  
    protected string _weaponName;               // The name of the weapon (ideally should be equal to the WeaponType)
    protected Enum_Lib.EWeaponType _weaponType; // The type of the weapon
    protected float _weaponDamage;              // The damage dealt by the weapon
    protected float _fireRate;                  // The rate of bullets spawning
    protected int _magazineSize;                // The amount of rounds the weapons magazine can store
    protected int _currentRoundsInMag;          // The current amount of rounds in the magazine
    protected int _spawnedBullets;              // The Amount of Bullets that will be spawned simultaneously (e.g. for the Shotgun it might be 5 and for the Handcannon 1)

    // --- Properties ---
    internal string WeaponName { get => _weaponName; set => _weaponName = value; }
    internal Enum_Lib.EWeaponType WeaponType { get => _weaponType; private set => _weaponType = value; }              // for getting the Name of the specific weapon object
    internal int CurrentRoundsInMag { get => _currentRoundsInMag; set => _currentRoundsInMag = value; }
    // for getting and setting the actual current rounds in the magazine of the specific weapon object


    // ---------- Methods ----------  
}
