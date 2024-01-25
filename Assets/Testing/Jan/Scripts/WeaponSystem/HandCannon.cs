public class HandCannon : BaseWeapon
{
    HandCannon(string weaponName, float weaponDamage, int magazineSize, int currentRoundsInMag)
    {
        _weaponName = weaponName;
        _weaponDamage = weaponDamage;
        _magazineSize = magazineSize;
        _currentRoundsInMag = currentRoundsInMag;
    }
}
