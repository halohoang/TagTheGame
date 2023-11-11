using Enemies;
using EnumLibrary;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RangeEnemy_Attack_Shooting", menuName = "Scriptable Objects/Enemy Logic/Attack Logic/RangeEnemy Attack Shooting")]
    public class RangeEnemyAttackSO : BaseEnemyAttackSO
    {
        [Header("References")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField, ReadOnly] private BoxCollider2D _boxCollider2D;
        [SerializeField, ReadOnly] private Transform _bulletSpawnPoint;

        [Header("Settings")]
        [Tooltip("The Size of the GameObjects Collizer adapted to the Shooting Sprite when entering the RangeAttack-Behaviour")]
        [SerializeField] private Vector2 _colliderSizeOfShootSprite;
        [Tooltip("The amount of bullets a shooting salve shalt contain.")]
        [SerializeField, Range(1, 100)] private int _maxBulletsShotAtAnInterval = 3;
        [Tooltip("The interval in which bullet salves shall be shot (in sec.)")]
        [SerializeField] private float _bulletSalveShootInteravl = 1.0f;
        [SerializeField] private float _bulletActivationDelay = 0.3f;

        private int _amountOfBulletsShot = 0;
        private float _salveIntervalTimer = 0.0f;
        private float _bulletActivationTimer = 0.0f;
        private bool _isAttacking = false;
        private Vector2 _colliderSizeCache;

        public bool IsAttacking { get => _isAttacking; private set => _isAttacking = value; }

        // todo: look into the initialization again and check if really all three overloads are necessary here; JM (10.11.2023)
        public override void Initialize(GameObject enemyObj, BaseEnemyBehaviour enemyBehav)
        {
            base.Initialize(enemyObj, enemyBehav);
        }
        //public override void Initialize(GameObject enemyObj, MeleeEnemyBehaviour meleeEnemyBehav)
        //{
        //    base.Initialize(enemyObj, meleeEnemyBehav);
        //}
        //public override void Initialize(GameObject enemyObj, RangeEnemyBehaviour rangeEnemyBehav)
        //{
        //    base.Initialize(enemyObj, rangeEnemyBehav);
        //}

        public override void ExecuteEnterLogic()
        {
            base.ExecuteEnterLogic();

            IsAttacking = true;

            SettingReferences();
            SettingValues();

            // Play shooting animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", IsAttacking);
        }        

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();

            IsAttacking = false;

            // Stop shooting animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", IsAttacking);
        }

        public override void ExecuteFrameUpdateLogic()
        {
            base.ExecuteFrameUpdateLogic();

            // shoot logic:
            // 1) Caching PlayerPosition and Transitioncheck (is player still detected, if not -> switch to chase state (running towards last known position of player))
            if (_baseEnemyBehaviour.IsPlayerDetected)
            {
                _baseEnemyBehaviour.CacheLastKnownPlayerPosition();
            }
            else
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.ChaseState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>RangeAttack (Shooting)</color>' to '<color=orange>Chase</color>' should have been happend now!");
                return;
            }

            // 3) Bulletinstantiation + calling bullet beahviour (moving towards player)            
            ShootBulletSalves();

            // 4) reloading sequence (implementation maybe after Showcase on the 22.11.2023)
        }

        public override void ExecutePhysicsUpdateLogic()
        {
            base.ExecutePhysicsUpdateLogic();
        }

        public override void ExecuteAnimationTriggerEventLogic(Enum_Lib.EAnimationTriggerType animTriggerTyoe)
        {
            base.ExecuteAnimationTriggerEventLogic(animTriggerTyoe);
        }

        public override void ResetValues()
        {
            base.ResetValues();

            // reset boxcollider size
            _boxCollider2D.size = _colliderSizeCache;
            Debug.Log($"<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>: The Size of the Boxcollider was set to its original previously cached values. ressetted values: " +
                $"('<color=lime>{_boxCollider2D.size}</color>') | previously cached values: ('<color=lime>{_colliderSizeCache}</color>').");

            // reset timer
            _salveIntervalTimer = 0.0f;

            // reset bulletactivation timer
            _bulletActivationTimer = 0.0f;

            // reset Amount of Bullets that have been shot
            _amountOfBulletsShot = 0;
        }        

        /// <summary>
        /// Null-Checks and if needed sets References for <see cref="_bulletPrefab"/>, <see cref="_boxCollider2D"/>, <see cref="_bulletSpawnPoint"/>.
        /// </summary>
        private void SettingReferences()
        {
            // setting References if not manually set inthe inspector
            if (_bulletPrefab == null)
            {
                _bulletPrefab = Resources.Load("Prefabs/Bullet/EnemyBullet") as GameObject;
                Debug.LogWarning($"<color=yellow>Caution!</color>: There was no Reference set to 'BulletPrefab in inspector of '{this}', so it was automatically set to {_bulletPrefab.name}." +
                    $"If that is not correct, please set the according referenc manually.");
            }

            _boxCollider2D = _baseEnemyBehaviour.gameObject.GetComponent<BoxCollider2D>();

            // Get Bullet Spawn Position
            for (int i = 0; i < _baseEnemyBehaviour.gameObject.transform.childCount; i++)
            {
                GameObject objChild = _baseEnemyBehaviour.gameObject.transform.GetChild(i).gameObject;

                if (objChild.name == "BulletSpawnPoint")
                {
                    _bulletSpawnPoint = objChild.GetComponent<Transform>();
                    break;
                }
            }
        }

        /// <summary>
        /// Null-Checks and if needed sets values for <see cref="_colliderSizeOfShootSprite"/>, <see cref="_colliderSizeCache"/>.
        /// </summary>
        private void SettingValues()
        {
            // setting Setting-Values if not set manually in the inspector
            if (_colliderSizeOfShootSprite == null)
            {
                _colliderSizeOfShootSprite = new Vector2(0.258874f, 0.139925f);
                Debug.LogWarning($"<color=yellow>Caution!</color>: The Size of the BoxCollider when changing Sprite on Entering AttackBehaviour of the RangeEnemy " +
                    $"was not se set to a value in the inspector of '{this}', so it was automatically set to {_colliderSizeOfShootSprite}." +
                    $"If that is not correct, please set the according values manually.");
            }

            // caching actual Size-Values of Boxcollider
            _colliderSizeCache = _boxCollider2D.size;
            Debug.Log($"<color=cyan>{_baseEnemyBehaviour.gameObject.name}</color>: The Size of the Boxcollider was cached. Cached Value: ('<color=lime>{_colliderSizeCache}</color>').");
        }

        /// <summary>
        /// Activates a BulletPrefab from the <see cref="EnemyBulletObjectPool"/> to a certain amount at a time while considering an bullet activation delay so the bullet prefabs are not
        /// activated in a to short time interval.
        /// </summary>
        private void ShootBulletSalves()
        {
            GameObject EnemyBullet = EnemyBulletObjectPool.Instance.GetInactivePooledObject();

            if (EnemyBullet == null)    // shoot a warning if no bullet can be activated
            {
                Debug.LogWarning($"<color=yellow>CAUTION!</color> no more inactive {EnemyBulletObjectPool.Instance.ObjectToPool.name} left in the {EnemyBulletObjectPool.Instance.gameObject.name}. You porbably should consider to increase the amount of Objects to pool in the Inspector of {EnemyBulletObjectPool.Instance.ObjectToPool.name}.");
            }
            // only activate a bullet prefab as long as the max bullet salve amount was not reached and the BulletActivationTimer is greater than 0 (means the Activation DelayTimer did not yet ended)
            else if (EnemyBullet != null && _amountOfBulletsShot < _maxBulletsShotAtAnInterval && _bulletActivationTimer <= 0.0f)
            {
                EnemyBullet.transform.position = _bulletSpawnPoint.position;
                EnemyBullet.transform.rotation = _bulletSpawnPoint.rotation;
                EnemyBullet.SetActive(true);

                // count bullets that have been activated
                _amountOfBulletsShot += 1;

                // bullet activateion timer tick
                _bulletActivationTimer = _bulletActivationDelay;

                Debug.Log($"<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>: Bullet Obj from EnemyBulletObjectPool was activated. (Amounts of Bullets activated: " +
                    $"'<color=cyan>{_amountOfBulletsShot}</color>'. Bullet Activation Timer: '<color=cyan>{_bulletActivationTimer}'</color>\")");
            }
            // tick the Bullet salve interval timer when more than or equal to the max Bullet Salve amount was activated and the salve bullet timer is stil running
            else if (_amountOfBulletsShot >= _maxBulletsShotAtAnInterval && _salveIntervalTimer <= _bulletSalveShootInteravl)
            {
                // timer tick
                _salveIntervalTimer += Time.deltaTime;
                Debug.Log($"<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>: Shooting Interval Timer: '<color=cyan>{_salveIntervalTimer}'</color>");
            }
            // if the salve interval timer reached its end reset it to 0 (for anew ticking possibility)
            else if (_salveIntervalTimer > _bulletSalveShootInteravl)
            {
                // reset timer
                _salveIntervalTimer = 0.0f;

                // reset amount of Bullets that where shot
                _amountOfBulletsShot = 0;
                Debug.Log($"<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>: Timer for Salve Interval was reset to: '{_salveIntervalTimer}' and Amounts of Bullets shot was reset to: " +
                    $"{_amountOfBulletsShot}");
            }
            // tick the bullet acitvation timer when it is less equal than the bullet activation delay it is set to (so basically this condition is allways true atm); since this conditionis allways true check the whole query for its logic regarding the single conditions again for a proper integrity. so far the behaviour works as intended so at this point I don't see any need to change the query logic but nonetheless it seems fishy, that this condition is actually allways true but I'm also kinda tired right now (today is saturday and I'm sitting on it since like straight 5 hours again but I also had a 13 h work day yesterday already so I actually just want to quit for today and get a bit freetime on the Weekend); JM (11.11.2023)
            else if (_bulletActivationTimer <= _bulletActivationDelay)
            {
                // reset bulletactivation timer
                _bulletActivationTimer -= Time.deltaTime;

                Debug.Log($"<color=orange>{_baseEnemyBehaviour.gameObject.name}</color>: Bullet Activation Timer: '<color=cyan>{_bulletActivationTimer}'</color>");
            }
        }
    }
}