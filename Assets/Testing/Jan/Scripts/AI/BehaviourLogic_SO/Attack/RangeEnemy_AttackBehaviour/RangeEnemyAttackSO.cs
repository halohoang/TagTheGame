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

            // setting References if not manually set inthe inspector
            if (_bulletPrefab == null)
            {
                _bulletPrefab = Resources.Load("Prefabs/Bullet/EnemyBullet") as GameObject;
                Debug.LogWarning($"<color=yellow>Caution!</color>: There was no Reference set to 'BulletPrefab in inspector of '{this}', so it was automatically set to {_bulletPrefab.name}." +
                    $"If that is not correct, please set the according referenc manually.");
            }
            
            _boxCollider2D = _baseEnemyBehaviour.gameObject.GetComponent<BoxCollider2D>();
            _bulletSpawnPoint = _baseEnemyBehaviour.gameObject.GetComponent<Transform>();


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

            // Play shooting animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", IsAttacking);
        }

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();

            IsAttacking = false;

            // Stop shooting animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", IsAttacking);

            // reset boxcollider size
            _boxCollider2D.size = _colliderSizeCache;
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
            }

            // 3) Bulletinstantiation + calling bullet beahviour (moving towards player)
            //shoot bullet
            // Calculate Deviation during the shooting
            #region BulletDeviation (not used currently)
            //float deviation = CalculateDeviation();
            //Quaternion bulletRotation = _bulletSpawnPoint.rotation; // Apply deviation to the bullet's rotation
            //float randomAngle = UnityEngine.Random.Range(-deviation, deviation); // Randomize the deviation angle
            //bulletRotation *= Quaternion.Euler(0f, 0f, randomAngle); // Apply rotation around the Z-axis
            #endregion
            /*GameObject bullet =*/ Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);            

            //Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();


            // 3.1) creating objectpool for bullets and bullet cases

            // 4) reloading sequence
        }
        //private float CalculateDeviation()
        //{
        //    float holdTriggerDuration = Mathf.Clamp01((Time.time - _mouseButtonReleaseTime) / _whenDeviationKicksIn); // Normalize the duration between 0 and 1, with a maximum of 5 seconds
        //    return _maxDeviationAngle * holdTriggerDuration;
        //}

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
        }
    }
}