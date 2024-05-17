using EnumLibrary;

public class ShotGun : BaseWeapon
{
    public ShotGun(string weaponName, Enum_Lib.EWeaponType weaponType, float weaponDamage, float fireRate, int magazineSize, int currentRoundsInMag, int spawnedBullets)
    {
        _weaponName = weaponName;
        _weaponType = weaponType;
        _weaponDamage = weaponDamage;
        _fireRate = fireRate;
        _magazineSize = magazineSize;
        _currentRoundsInMag = currentRoundsInMag;
        _spawnedBullets = spawnedBullets;
    }
}
