using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{    
    public class BaseEnemyAlertSO : ScriptableObject
    {
        protected BaseEnemyBehaviour _baseEnemyBehaviour;
        //protected MeleeEnemyBehaviour _meleeEnemyBehaviour;
        //protected RangeEnemyBehaviour _rangeEnemyBehaviour;
        protected Transform _transform;
        protected GameObject _gameObject;

        protected Transform _playerTransform;

        public virtual void Initialize(GameObject enemyObj, BaseEnemyBehaviour enemyBehav)
        {
            this._gameObject = enemyObj;
            this._transform = enemyObj.transform;
            this._baseEnemyBehaviour = enemyBehav;

            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        //public virtual void Initialize(GameObject enemyObj, MeleeEnemyBehaviour meleeEnemyBehav)
        //{
        //    this._gameObject = enemyObj;
        //    this._transform = enemyObj.transform;
        //    this._meleeEnemyBehaviour = meleeEnemyBehav;

        //    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //}
        //public virtual void Initialize(GameObject enemyObj, RangeEnemyBehaviour rangeEnemyBehav)
        //{
        //    this._gameObject = enemyObj;
        //    this._transform = enemyObj.transform;
        //    this._rangeEnemyBehaviour = rangeEnemyBehav;

        //    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //}

        public virtual void ExecuteEnterLogic() { }
        public virtual void ExecuteExitLogic()
        {
            _baseEnemyBehaviour.SetIsSomethingAlarmingHappening(false);

            ResetValues();
        }

        public virtual void ExecuteFrameUpdateLogic()
        {
            // todo: if implementing more ALert-Behaviour maybe move following logic to 'EnemyAlertStandingSO'; JM (02.11.2023)
            FaceAgentTowardsAlarmingEvent(_baseEnemyBehaviour.PositionOfAlarmingEvent, _baseEnemyBehaviour.NoiseRangeOfAlarmingEvent);
        }

        public virtual void ExecutePhysicsUpdateLogic() { }
        public virtual void ExecuteAnimationTriggerEventLogic(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }
        public virtual void ResetValues() { }

        // todo: if implementing more ALert-Behaviour maybe move following logic to 'EnemyAlertStandingSO'; JM (02.11.2023)
        /// <summary>
        /// Sets the Facing direction of the enemy-object towards the position of an alarming Event that is happening (e.g. door kick in)
        /// if the enemy-object is within the noise-range of the alarming event
        /// </summary>
        /// <param name="positionOfAlarmingEvent"></param>
        /// <param name="noiseRangeOfAlarmingEvent"></param>
        private void FaceAgentTowardsAlarmingEvent(Vector3 positionOfAlarmingEvent, float noiseRangeOfAlarmingEvent)
        {
            // Rotate the Enemy-Object so it's facing the Kicked in Door Object when Door was kicked in 
            Collider2D[] enemieColliders = Physics2D.OverlapCircleAll(positionOfAlarmingEvent, noiseRangeOfAlarmingEvent, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < enemieColliders.Length; i++)
            {
                //enemieColliders[i].gameObject.transform.right = positionOfAlarmingEvent - enemieColliders[i].gameObject.transform.position;

                // setting facing to walk direction if walking timer has ended and was setup anew
                Vector2 direction = (positionOfAlarmingEvent - enemieColliders[i].gameObject.transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                enemieColliders[i].gameObject.GetComponent<Rigidbody2D>().rotation = angle;
            }
        }
    }
}
