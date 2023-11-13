using UnityEngine;
using NaughtyAttributes;

public class EnemyBullet : BaseBullet
{
    [SerializeField, ReadOnly] private GameObject[] _enemyObjects;

    private void Awake()
    {
        
    }

    private new void Start()
    {
        base.Start();

        // enemy bullets ignoring the enemy objects
        GameObject meleeEnemyStandIdlePrefab = Resources.Load("Prefabs/Enemy/enemy_melee_red_standing_Idle") as GameObject;
        GameObject meleeEnemyRandomWanderIdlePrefab = Resources.Load("Prefabs/Enemy/enemy_melee_red_random-wander_Idle") as GameObject;
        GameObject rangeEnemyStandIdkePrefab = Resources.Load("Prefabs/Enemy/enemy_range_yellow_standing_Idle") as GameObject;
        GameObject rangeEnemyRandomWanderIdlePrefab = Resources.Load("Prefabs/Enemy/Bulenemy_range_yellow_random-wander_IdleletCasing") as GameObject;

        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), meleeEnemyStandIdlePrefab.GetComponent<Collider2D>(), true);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), meleeEnemyRandomWanderIdlePrefab.GetComponent<Collider2D>(), true);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), rangeEnemyStandIdkePrefab.GetComponent<Collider2D>(), true);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), rangeEnemyRandomWanderIdlePrefab.GetComponent<Collider2D>(), true);
        // todo: change this to actually logic that checs first if another enemy is obstructing the line of view to the player, if so move to the left or right or something like that; JM (11.11.2023)
    }

    private new void Update()
    {
        _bulletRB2D.velocity = transform.right * _bulletSpeed;

        base.Update();
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        TargetCollisionCheck(collision, "Player");

        if (collision.gameObject.CompareTag("Player"))
        {
            /* Contact points spawning blood */
            ContactPoint2D[] contacts = collision.contacts;
            Vector2 collisionPoint = contacts[0].point;
            Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
            Instantiate(_bloodPrefab, collisionPoint, bloodRotation);
        }
    }
}
