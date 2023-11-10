using UnityEngine;

public class EnemyBullet : BaseBullet
{
    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        TargetCollisionCheck(collision, "Player");
    }
}