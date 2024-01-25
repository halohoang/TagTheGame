public class SubMachineGun : BaseWeapon
{
    SubMachineGun(string weaponName, float weaponDamage, int magazineSize, int currentRoundsInMag)
    {
        _weaponName = weaponName;
        _weaponDamage = weaponDamage;
        _magazineSize = magazineSize;
        _currentRoundsInMag = currentRoundsInMag;
    }

}
