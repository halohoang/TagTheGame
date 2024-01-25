/// <summary>
/// BaseClass to all specific Weapontypes ike HandCannon.cs, Shotgun.cs, SubMachineGun.cs, EnergyLauncher.cs
/// </summary>
public class BaseWeapon
{
    // ---------- Variables ----------  
    protected string _weaponName;           // The name of the weapon
    protected float _weaponDamage;          // The damage dealt by the weapon
    protected int _magazineSize;          // The amount of rounds the weapons magazine can store
    protected int _currentRoundsInMag;    // The current amount of rounds in the magazine


    // ---------- Methods ----------
    //public virtual void DealDamage()
    //{

    //}
}
