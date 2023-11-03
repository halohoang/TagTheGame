using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{    
    public class BaseEnemyAlertSO : ScriptableObject
    {
        protected BaseEnemyBehaviour _baseEnemyBehaviour;
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

            // Transition-Condition-Check
            if (_baseEnemyBehaviour.IsPlayerDetected)
            {
                _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.ChaseState);
                Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Chase</color>' should have been happend now!");
            }
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
                enemieColliders[i].gameObject.transform.right = positionOfAlarmingEvent - enemieColliders[i].gameObject.transform.position;

                // todo: use 'Vector3.Angle()/Vector2.Angle()' for calculating the Angle between two Vectors; JM(03.11.2023)                

                #region old code
                //alternate solution (does not work properly)
                //Vector2 directionToDoor = (doorPosition - transform.position).normalized;
                //float alphaAngle = Mathf.Atan2(directionToDoor.x, directionToDoor.y);
                //float angleToRotate = (Mathf.PI - alphaAngle) * Mathf.Rad2Deg;
                //Quaternion quart = Quaternion.AngleAxis(angleToRotate, Vector3.forward);

                //enemieColliders[i].gameObject.transform.rotation = quart;
                //Debug.Log($"<color=orange> {enemieColliders[i].name} was rotated by {angleToRotate}° on its Z-Axis </color>");


                //alternate solution (does not work properly)
                //// calculate rotation angle (does not work as intended yet tho); JM (18.10.2023)
                //Vector2 lookDirection = (doorPosition - transform.position).normalized;
                //float angle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg - _rotationModifier;
                //Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);

                //// rotat enemy-object
                //enemieColliders[i].gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, quart, 360);
                #endregion
            }
        }
    }
}
