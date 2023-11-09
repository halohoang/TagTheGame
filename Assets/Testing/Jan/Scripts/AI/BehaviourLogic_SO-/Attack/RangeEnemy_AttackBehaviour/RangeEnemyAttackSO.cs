using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RangeEnemy_Attack_Shooting", menuName = "Scriptable Objects/Enemy Logic/Attack Logic/RangeEnemy Attack Shooting")]
    public class RangeEnemyAttackSO : BaseEnemyAttackSO
    {
        private bool _isAttacking = false;

        public bool IsAttacking { get => _isAttacking; private set => _isAttacking = value; }

        public override void Initialize(GameObject enemyObj, BaseEnemyBehaviour enemyBehav)
        {
            base.Initialize(enemyObj, enemyBehav);
        }

        public override void ExecuteEnterLogic()
        {
            base.ExecuteEnterLogic();

            IsAttacking = true;

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
            }

            // 3) Bulletinstantiation + calling bullet beahviour (moving towards player)
            ////shoot bullet
            //// Calculate Deviation during the shooting
            //float deviation = CalculateDeviation();

            //Quaternion bulletRotation = _bulletSpawnPoint.rotation; // Apply deviation to the bullet's rotation
            //float randomAngle = UnityEngine.Random.Range(-deviation, deviation); // Randomize the deviation angle

            //bulletRotation *= Quaternion.Euler(0f, 0f, randomAngle); // Apply rotation around the Z-axis

            //GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, bulletRotation);
            //Rigidbody2D bulletRigidBody2D = bullet.GetComponent<Rigidbody2D>();
            //_ammoCounterScript.DecreaseAmmo(); //Call the Decrease Ammo function from the AmmoCounter script;
            //_animator.SetBool("Firing", true);
            //_nextFireTime = Time.time + _firerate;


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