using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    // Variables

    [Header("Bullet settings")]
    /* Bullet Properties */
    [SerializeField] internal float _bulletSpeed = 40.0f;
    //[Tooltip("The Objects the Bullet shall detect as obstalce and be disabled on collision with")]
    //[SerializeField] private LayerMask _obstacleCollisionMask;
    //[Tooltip("The Objects the Bullet shall detect as Player or Enemy so that the TakeDamage() can be called on that CollisionObject and the BulletObject can be disabled on collssion")]
    //[SerializeField] private LayerMask _targedMask;
    [Space(5)]

    [Header("References")]
    /* Spawning Blood */
    [SerializeField] protected GameObject _bloodPrefab;


    /*Bounce Properties */
    internal Rigidbody2D _bulletRB2D;
    [SerializeField] internal float _maxBulletAliveTime = 1.0f; // Maximum of time until the bullet is destroyed
    internal float _currentBulletLiveTime; // Current time until the bullet is destroyed
    private GameObject _bullet;

    // Function
    protected virtual void Start()
    {
        _bullet = GetComponent<GameObject>();
        _bulletRB2D = GetComponent<Rigidbody2D>();
        _currentBulletLiveTime = _maxBulletAliveTime;

        //// ignoring the bullet casings objects
        //GameObject bulletCasingPrefab = Resources.Load("Prefabs/Bullet/BulletCasing") as GameObject;
        //Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), bulletCasingPrefab.GetComponent<Collider2D>(), true);
        // todo: check why the colliders won't be ignored. according to 'https://docs.unity3d.com/ScriptReference/Physics2D.IgnoreCollision.html' it actually should work; JM (11.11.2023)
    }

    protected virtual void Update()
    {
        BulletDeactive();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // todo: (!) if time read more into bitshifting to really understand whats happening in the query until then stick to the tag solution of hoang; JM (10.11.23)
        //// reworkd by Jan (09.11.2023)
        //ObstacleCollisionCheck(collision);
        //TargetCollisionDetection(collision);
        
        // checking for collision with obstacle objects
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Door")/* || collision.gameObject.CompareTag("Bullet")*/)
        {
            gameObject.SetActive(false);
            Debug.Log($"'<color=mageta>{gameObject.name}</color>': was disabled due to collision with '{collision.gameObject.name}'");
        }        
        #region Original Code by Hoang
        //// origianl Code by Hoang
        //if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Door") || collision.gameObject.CompareTag("Bullet"))
        //{
        //    gameObject.SetActive(false);
        //}

        //// Original Code by Hoang
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    PlayerHealth playerHealth;
        //    playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        //    /* Contact points spawning blood */
        //    ContactPoint2D[] contacts = collision.contacts;
        //    Vector2 collisionPoint = contacts[0].point;
        //    Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
        //    Instantiate(_bloodPrefab, collisionPoint, bloodRotation);
        //    if (playerHealth != null)
        //    {
        //        /* Deal Damage */
        //        playerHealth.GetDamage();
        //        Debug.Log(playerHealth._currentHealth);
        //        gameObject.SetActive(false);
        //    }
        //    // Call deal damage function
        //    //}
        //}

        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    EnemyHealth enemyHealth;
        //    enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

        //    /* Contact points spawning blood */
        //    ContactPoint2D[] contacts = collision.contacts;
        //    Vector2 collisionPoint = contacts[0].point;
        //    Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
        //    Instantiate(_bloodPrefab, collisionPoint, bloodRotation);
        //    if (enemyHealth != null)
        //    {
        //        enemyHealth.GetDamage();
        //        Debug.Log(enemyHealth._currentHealth);
        //        gameObject.SetActive(false);
        //    }
        //}
        #endregion
    }

    /// <summary>
    /// Calls the <see cref="DealingDamage(EnemyHealth)"/> or <see cref="DealingDamage(PlayerHealth)"/> respective to the transmitted parameter 'tagOfTargetObj'.
    /// </summary>
    /// <param name="collision"></param>
    /// <param name="tagOfTargetObj">the Tag of the target Object to check for Damage dealing ("Player" or "Enemy")</param>
    protected void TargetCollisionCheck(Collision2D collision, string tagOfTargetObj)
    {
        // todo: maybe rather call this methon din the this.OnCollisionEnter2D() and check for simple collision.gameObject.CompareTag("Plaser"/"Enemy") and execute accordingly DealingDamage(plyerHealth/EnemyHealth); JM (11.11.2023)

        // if transmitted tag is 'Player' continue with calling TakeDamage() of the PlayerHealth.cs
        if (tagOfTargetObj == "Player")
        {
            collision.gameObject.TryGetComponent(out PlayerHealth playerHealth);
            DealingDamage(playerHealth);
        }
        // else if transmitted tag is 'Enemy' continue with calling TakeDamage() of the EnemyHealth.cs instead
        else if (tagOfTargetObj == "Enemy")
        {
            collision.gameObject.TryGetComponent(out EnemyHealth enemyHealth);
            DealingDamage(enemyHealth);
        }
    }

    #region Used in LayerMaskCheck
    // Following methods are only used in the OnCollisionEnter2D() rework of Jan, that is not used currently; JM (10.11.2023)
    /// <summary>
    /// Check if this gameobject collided with an obstacle
    /// </summary>
    /// <param name="collision"></param>
    private void ObstacleCollisionCheck(Collision2D collision)
    {
        //// todo: (!) if time read more into bitshifting to really understand whats happening in the query until then stick to the tag solution of hoang; JM (10.11.23)
        //if (((1 << collision.gameObject.layer) & _obstacleCollisionMask.value) != 0)
        //{
        //    gameObject.SetActive(false);

        //    Debug.Log($"<color=cyan>Bullet was deactivated on collision with {collision.gameObject.name}</color>");
        //}
    }

    /// <summary>
    /// Handles the Logic that shall happen when a collision of a bullet with the player is detected (like damage dealing to the player etc.)
    /// </summary>
    /// <param name="collision"></param>
    private void TargetCollisionDetection(Collision2D collision)
    {
        // todo: (!) if time read more into bitshifting to really understand whats happening in the query until then stick to the tag solution of hoang; JM (10.11.23)
        //if (((1 << collision.gameObject.layer) & _targedMask.value) != 0)
        //{
        //    /* Contact points spawning blood */
        //    ContactPoint2D[] contacts = collision.contacts;
        //    Vector2 collisionPoint = contacts[0].point;
        //    Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
        //    Instantiate(_bloodPrefab, collisionPoint, bloodRotation);


        //    if (collision.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        //        DealingDamage(playerHealth);
        //    else if (collision.gameObject.TryGetComponent(out EnemyHealth enemyHealth))
        //        DealingDamage(enemyHealth);
        //}
    }
    #endregion

    /// <summary>
    /// Executing <see cref="PlayerHealth.GetDamage()"/> if transmitted Type is not null and deactivates this gameobject
    /// </summary>
    /// <param name="healthScript"></param>
    private void DealingDamage(PlayerHealth healthScript)
    {
        if (healthScript != null)
        {
            /* Deal Damage */
            healthScript.GetDamage();
            Debug.Log(healthScript._currentHealth);
            gameObject.SetActive(false);

            Debug.Log($"'<color=mageta>{gameObject.name}</color>: was disabled due to collision with '{healthScript.gameObject.name}'");
        }
    }

    /// <summary>
    /// Executing <see cref="EnemyHealth.GetDamage()"/> if transmitted Type is not null and deactivates this gameobject
    /// </summary>
    /// <param name="healthScript"></param>
    private void DealingDamage(EnemyHealth healthScript)
    {
        if (healthScript != null)
        {
            /* Deal Damage */
            healthScript.GetDamage();
            Debug.Log(healthScript._currentHealth);
            gameObject.SetActive(false);

            Debug.Log($"'<color=mageta>{gameObject.name}</color>: was disabled due to collision with '{healthScript.gameObject.name}'");
        }
    }

    private void BulletDeactive()
    {
        if (_bulletRB2D != null)
        {
            _currentBulletLiveTime -= Time.deltaTime;

            if (_currentBulletLiveTime <= 0)
            {
                gameObject.SetActive(false);
                Debug.Log($"'<color=mageta>{gameObject.name}</color>: was disabled due to current Bullet Live Time ('{_currentBulletLiveTime}') run out.");

                // reset Bullet LiveTime
                _currentBulletLiveTime = _maxBulletAliveTime;

                Debug.Log($"'<color=mageta>{gameObject.name}</color>: current Bullet Live Time was reset to max Bullet Live Time again: Current Bullet Live TIme: ('<color=lime>{_currentBulletLiveTime}</color>')");
            }
        }
    }
}
