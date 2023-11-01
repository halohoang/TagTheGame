using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Idle-Radom-Wander State", menuName = "Scriptable Objects/Enemy Logic/Idle Logic/Random Wander")]
    public class EnemyIdleRandomWanderSO : BaseEnemyIdleSO
    {
        // this are just Placeholder Values at the moment (will be replaced by proper logic later); JM (01.11.2023)
        [SerializeField] private float RandomMovementRange = 5.0f;
        [SerializeField] private float RandomMovementSpeed = 1.0f;

        private Vector3 _targedPos;
        private Vector3 _direction;
        private float timer = 0.0f;

        public override void Initialize(GameObject enemyObj, BaseEnemyBehaviour enemyBehav)
        {
            base.Initialize(enemyObj, enemyBehav);
        }

        public override void ExecuteEnterLogic()
        {
            base.ExecuteEnterLogic();

            _targedPos = GetRandomPointInCircle();
            _baseEnemyBehaviour.NavAgent.speed = RandomMovementSpeed;

            // setup walking animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", true);
        }

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();

            // setup walking animation
            _baseEnemyBehaviour.Animator.SetBool("Engage", false);
        }

        public override void ExecuteFrameUpdateLogic()
        {
            base.ExecuteFrameUpdateLogic();
            
            timer += Time.deltaTime;

            // todo: replace logic by better logic for random movement; JM (01.11.2023)
            _direction = (_targedPos - _baseEnemyBehaviour.transform.position).normalized;

            // setting facing to walk direction
            _baseEnemyBehaviour.transform.right = _direction;

            // actual walking
            _baseEnemyBehaviour.NavAgent.SetDestination(_targedPos);

            float rndWalktime = Random.Range(2.0f, 6.0f);

            if (timer > rndWalktime || (_baseEnemyBehaviour.transform.position - _targedPos).sqrMagnitude < 0.01f)
            {
                _targedPos = GetRandomPointInCircle();
                timer = 0.0f;
            }
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
        }

        private Vector3 GetRandomPointInCircle()
        {
            return _baseEnemyBehaviour.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * RandomMovementRange;
        }
    }
}