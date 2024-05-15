using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Class for defining the behaviour for the bullet projectile shot by enemy-characters
/// </summary>
public class EnemyBullet : BaseBullet
{
    [SerializeField, ReadOnly] private GameObject[] _enemyObjects;

    private void Awake()
    {
        _enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void OnEnable()
    {
        // enemy bullets ignoring the enemy objects
        foreach (GameObject enemyObj in _enemyObjects)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), enemyObj.GetComponent<Collider2D>());
        }
    }

    private void FixedUpdate()
    {
        BulletRB2D.velocity = transform.right * BulletSpeed;
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        TargetCollisionCheck(collision);
    }
}
