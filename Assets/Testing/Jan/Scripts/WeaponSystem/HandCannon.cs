using EnumLibrary;

public class HandCannon : BaseWeapon
{
    // Todo: creaete Base-Class Constructor and do the Value-Initializing via the ': base(...)'
    public HandCannon(string weaponName, Enum_Lib.EWeaponType weaponType, float weaponDamage, float fireRate, int magazineSize, int currentRoundsInMag, int storedAmmo, int spawnedBullets, int reloadHintThreshold) 
        //: base(weaponName, weaponType, weaponDamage, fireRate, magazineSize, currentRoundsInMag, storedAmmo, spawnedBullets, reloadHintThreshold)
    {
        _weaponName = weaponName;
        _weaponType = weaponType;
        _weaponDamage = weaponDamage;
        _fireRate = fireRate;
        _magazineSize = magazineSize;
        _currentRoundsInMag = currentRoundsInMag;
        _storedAmmo = storedAmmo;
        _spawnedBullets = spawnedBullets;
        _reloadHintThreshhold = reloadHintThreshold;
    }
}
