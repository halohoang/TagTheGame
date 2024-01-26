public class ShotGun : BaseWeapon
{
    public ShotGun(string weaponName, float weaponDamage, float fireRate, int magazineSize, int currentRoundsInMag, int amountOfSimultaneousSpawnedBullets)
    {
        _weaponName = weaponName;
        _weaponDamage = weaponDamage;
        _fireRate = fireRate;
        _magazineSize = magazineSize;
        _currentRoundsInMag = currentRoundsInMag;
        _amountOfSimultaneouslySpawnedBullets = amountOfSimultaneousSpawnedBullets;
    }
}
