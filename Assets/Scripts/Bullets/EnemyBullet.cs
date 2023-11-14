using UnityEngine;
using NaughtyAttributes;

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

    private new void Start()
    {
        base.Start();

        //// enemy bullets ignoring the enemy objects
        //foreach (GameObject enemyObj in _enemyObjects)
        //{
        //    Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), enemyObj.GetComponent<Collider2D>());
        //}
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
