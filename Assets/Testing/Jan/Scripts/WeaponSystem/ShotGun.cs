using EnumLibrary;

public class ShotGun : BaseWeapon
{
    public ShotGun(string weaponName, Enum_Lib.EWeaponType weaponType, float weaponDamage, float fireRate, int magazineSize, int currentRoundsInMag, int amountOfMags, int spawnedBullets, int reloadHintThreshold)
    {
        _weaponName = weaponName;
        _weaponType = weaponType;
        _weaponDamage = weaponDamage;
        _fireRate = fireRate;
        _magazineSize = magazineSize;
        _currentRoundsInMag = currentRoundsInMag;
        _amountOfMagazines = amountOfMags;
        _spawnedBullets = spawnedBullets;
        _reloadHintThreshhold = reloadHintThreshold;
    }
}
