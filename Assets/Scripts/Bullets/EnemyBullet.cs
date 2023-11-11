using UnityEngine;

public class EnemyBullet : BaseBullet
{
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
