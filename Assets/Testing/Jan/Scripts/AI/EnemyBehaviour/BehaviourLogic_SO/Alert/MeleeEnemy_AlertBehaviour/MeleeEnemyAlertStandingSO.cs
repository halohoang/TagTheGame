using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MeleeEnemy_Alert_Stading", menuName = "Scriptable Objects/Enemy Logic/Alert Logic/MeleeEnemy Alert Standing")]
    public class MeleeEnemyAlertStandingSO : BaseEnemyAlertSO
    {
        #region Variables
        //--------------------------------------
        // - - - - -  V A R I A B L E S  - - - - 
        //--------------------------------------

        //[SerializeField, Range(0.5f, 10.0f)] private float _stayAlertTime;

        //private float _timer = 0.0f;
        #endregion
        

        #region Methods
        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------

        public override void Initialize(GameObject enemyObj, NPCBehaviourController enemyBehav)
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
        }

        public override void ExecuteExitLogic()
        {
            base.ExecuteExitLogic();
        }

        public override void ExecuteFrameUpdateLogic()
        {
            base.ExecuteFrameUpdateLogic();
                       
            // Timer to return to Idle State after certain time has passed
            //_timer += Time.deltaTime;            

            //if (_timer > _stayAlertTime)
            //{
            //    _timer = 0.0f;
            //    _baseEnemyBehaviour.StateMachine.Transition(_baseEnemyBehaviour.IdleState);
            //    Debug.Log($"{_baseEnemyBehaviour.gameObject.name}: State-Transition from '<color=orange>Alert</color>' to '<color=orange>Idle</color>' should have been happend now!");
            //}
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
        #endregion
    }
}