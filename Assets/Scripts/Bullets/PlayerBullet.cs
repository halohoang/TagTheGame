using UnityEngine;

namespace Projectile
{
    /// <summary>
    /// Class for defining the Behaviour of the Bullet-Projectile shot by the Player Character
    /// </summary>
    public class PlayerBullet : BaseBullet
    {
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
}