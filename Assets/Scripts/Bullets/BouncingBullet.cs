using UnityEngine;

namespace Projectile
{
    /// <summary>
    /// Class for defining the behaviour of the bouncing bullet-projectile currently only shot by the player character
    /// </summary>
    public class BouncingBullet : BaseBullet
    {
        // Variables
        [SerializeField] private int _maxBounce;

        private int _currentBounce;
        private Vector2 _direction;

        private void OnEnable()
        {
            BulletRB2D = GetComponent<Rigidbody2D>();
            BulletRB2D.velocity = transform.right * BulletSpeed;
            _direction = BulletRB2D.velocity.normalized;
            _currentBounce = 0;
        }

        //Functions
        //private new void Start()
        //{
        //    base.Start();
        //    BulletRB2D.velocity = transform.right * BulletSpeed;
        //    _direction = BulletRB2D.velocity.normalized;
        //    _currentBounce = 0;
        //}    

        private new void OnCollisionEnter2D(Collision2D collision)
        {
            // Check for target collision
            TargetCollisionCheck(collision);

            //// Bounce calculation
            Vector2 _normal = collision.contacts[0].normal;     // Get the normal of the collision
            _direction = Vector2.Reflect(_direction, _normal);  // Calculate the new direction of the bullet
            BulletRB2D.velocity = _direction * BulletSpeed;     // the speed of the bullet will increase exponentially after each bounce.
            _currentBounce += 1;

            // deactivate buller on reaching max amount of bouncings
            if (_currentBounce >= _maxBounce)
                gameObject.SetActive(false);
        }
    }
}