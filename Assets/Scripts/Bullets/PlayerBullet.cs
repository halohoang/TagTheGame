using UnityEngine;

/// <summary>
/// Class for defining the Behaviour of the Bullet-Projectile shot by the Player Character
/// </summary>
public class PlayerBullet : BaseBullet
{
    private new void Start()
    {
        base.Start();

        BulletRB2D.velocity = transform.right * BulletSpeed;
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        TargetCollisionCheck(collision, "Enemy");

        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    /* Contact points spawning blood */
        //    ContactPoint2D[] contacts = collision.contacts;
        //    Vector2 collisionPoint = contacts[0].point;
        //    Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
        //    Instantiate(_bloodPrefab, collisionPoint, bloodRotation);
        //}
    }
}
