using UnityEngine;

public interface IDamageable
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }

    public void TakeDamge(float damage);

    public void Die();
}
