using UnityEngine;

/// <summary>
/// Class for defining the behaviour of the bouncing bullet-projectile currently only shot by the player character
/// </summary>
public class BouncingBullet : BaseBullet
{
	// Variables
	//[SerializeField] private float _speed = 10f;
	[SerializeField] private int _maxBounce;
	private int _currentBounce;
	//private Rigidbody2D _rb2D;
	private Vector2 _direction;

	//Functions
	private new void Start()
	{
		base.Start();
		//BulletRB2D = GetComponent<Rigidbody2D>();
        BulletRB2D.velocity = transform.right * BulletSpeed;
		_direction = BulletRB2D.velocity.normalized;
		_currentBounce = 0;

	}

	private new void Update()
	{
		base.Update();	// deactivate bullet after '_max bullet alive time' (see base class) is reached
	}

	private new void OnCollisionEnter2D(Collision2D collision)
	{
		// Check for target collision
        TargetCollisionCheck(collision, "Enemy");

		// Bounce calculation
		Vector2 _normal = collision.contacts[0].normal;		// Get the normal of the collision
		_direction = Vector2.Reflect(_direction, _normal);	// Calculate the new direction of the bullet
		BulletRB2D.velocity = _direction * BulletSpeed;		// the speed of the bullet will increase exponentially after each bounce.
		_currentBounce += 1;

		// deactivate buller on reaching max amount of bouncings
        if (_currentBounce >= _maxBounce)        
            Destroy(gameObject);
        

        //// Blood spawning on collision
        //      if (collision.gameObject.CompareTag("Enemy"))
        //      {
        //          /* Contact points spawning blood */
        //          ContactPoint2D[] contacts = collision.contacts;
        //          Vector2 collisionPoint = contacts[0].point;
        //          Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
        //          Instantiate(_bloodPrefab, collisionPoint, bloodRotation);
        //      }
    }
}
