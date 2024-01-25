using UnityEngine;

/// <summary>
/// BaseClass to all specific Weapontypes ike Pistol.cs, Shotgun.cs, SMG.cs, EnergyLauncher.cs
/// </summary>
public class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [Tooltip("The damage dealt by the weapon.")]
    [SerializeField] protected float _weaponDamage;
    [Tooltip("The amount of rounds the weapons magazine can store.")]
    [SerializeField] protected float _magazineSize;
    //... more to be declared in the future


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
