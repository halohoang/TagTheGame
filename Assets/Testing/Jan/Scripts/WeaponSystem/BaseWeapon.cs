using EnumLibrary;

/// <summary>
/// BaseClass to all specific Weapontypes ike HandCannon.cs, Shotgun.cs, SubMachineGun.cs, EnergyLauncher.cs
/// </summary>
public class BaseWeapon
{
    // ---------- Variables ----------  
    protected string _weaponName;                                   // The name of the weapon (ideally should be equal to the WeaponType)
    protected Enum_Lib.EWeaponType _weaponType;                     // The type of the weapon
    protected float _weaponDamage;                                  // The damage dealt by the weapon
    protected float _fireRate;                                      // The rate of bullets spawning
    protected int _magazineSize;                                    // The amount of rounds the weapons magazine can store
    protected int _currentRoundsInMag;                              // The current amount of rounds in the magazine
    protected int _storedAmmo;                               // The amount of magazines the player currently bears for this weapon
    protected int _spawnedBullets;                                  // The Amount of Bullets that will be spawned simultaneously (e.g. for the Shotgun it might be 5 and for the Handcannon 1)
    protected int _reloadHintThreshhold;                            // The Threshold for showing the UI-Reloadhint to inform Player to reload the Weapon


    // --- Properties ---
    internal string WeaponName { get => _weaponName; set => _weaponName = value; }
    internal Enum_Lib.EWeaponType WeaponType { get => _weaponType; private set => _weaponType = value; }    // for getting the Name of the specific weapon object
    internal float WeaponDamage { get => _weaponDamage; set => _weaponDamage = value; }
    internal float FireRate { get => _fireRate; set => _fireRate = value; }
    internal int MagazineSize { get => _magazineSize; set => _magazineSize = value; }    
    internal int CurrentRoundsInMag { get => _currentRoundsInMag; set => _currentRoundsInMag = value; }     // for getting and setting the actual current rounds in the magazine of the specific weapon object
    internal int StoredAmmo { get => _storedAmmo; set => _storedAmmo = value; }
    internal int SpawnedBullets { get => _spawnedBullets; set => _spawnedBullets = value; }
    internal int ReloadHintThreshhold { get => _reloadHintThreshhold; set => _reloadHintThreshhold = value; }


    // ---------- Constructor ----------
    // Todo: creaete Base-Class Constructor and do the Value-Initializing via the ': base(...)'
    //public BaseWeapon(string weaponName, Enum_Lib.EWeaponType weaponType, float weaponDamage, float fireRate, int magazineSize, int currentRoundsInMag, int storedAmmo, int spawnedBullets, int reloadHintThreshold)
    //{
    //    _weaponName = weaponName;
    //    _weaponType = weaponType;
    //    _weaponDamage = weaponDamage;
    //    _fireRate = fireRate;
    //    _magazineSize = magazineSize;
    //    _currentRoundsInMag = currentRoundsInMag;
    //    _storedAmmo = storedAmmo;
    //    _spawnedBullets = spawnedBullets;
    //    _reloadHintThreshhold = reloadHintThreshold;
    //}

    // ---------- Methods ----------
    /// <summary>
    /// Sets the Values for weapon name and weapon type
    /// </summary>
    /// <param name="weaponName"></param>
    /// <param name="weaponType"></param>
    internal void SetWeaponNameAndType(string weaponName, Enum_Lib.EWeaponType weaponType)
    {
        WeaponName = weaponName;
        WeaponType = weaponType;
    }

    /// <summary>
    /// Sets the values for the weapon damage, fire rate, max. size of the magazine, the amount of the current rounds insige the magazine and the amount of simultaneously spawned bullets.
    /// </summary>
    /// <param name="damage">weapon damage</param>
    /// <param name="fireRate">fire rate of the weapon</param>
    /// <param name="magazineSize">size of the magazine</param>
    /// <param name="currentRoundsInMag">the amount of rounds that are currently inside tha magazine</param>
    /// <param name="spawnedBullets">the amount of simultaneously spawned bullets</param>
    internal void SetWeaponValues(float damage, float fireRate, int magazineSize, int currentRoundsInMag, int storedAmmo, int spawnedBullets, int reloadHintThreshold)
    {
        WeaponDamage = damage;
        FireRate = fireRate;
        MagazineSize = magazineSize;
        CurrentRoundsInMag = currentRoundsInMag;
        StoredAmmo = storedAmmo;
        SpawnedBullets = spawnedBullets;
        ReloadHintThreshhold = reloadHintThreshold;
    }
}
