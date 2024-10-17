using EnumLibrary;

public class SubMachineGun : BaseWeapon
{
    public SubMachineGun(string weaponName, Enum_Lib.EWeaponType weaponType, float weaponDamage, float fireRate, int magazineSize, int currentRoundsInMag, int storedAmmo, int spawnedBullets, int reloadHintThreshold)
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
