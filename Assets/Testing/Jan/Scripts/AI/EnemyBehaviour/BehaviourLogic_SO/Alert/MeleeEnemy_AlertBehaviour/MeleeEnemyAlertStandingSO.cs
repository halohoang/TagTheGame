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

        #region Tooltip
        [Tooltip("Defines the amount of time (in seconds) how long the NPC shall stay alerted. Standard value is 5 seconds.")]
        #endregion
        [SerializeField] private float _stayAlertTime = 5.0f;

        private float _timer = 0.0f;
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

        public override void ExecuteOnEnterState()
        {
            base.ExecuteOnEnterState();
        }

        public override void ExecuteOnExitState()
        {
            base.ExecuteOnExitState();
        }

        public override void Execute﻿FrameUpdate()
        {
            base.Execute﻿FrameUpdate();

            //Timer to return to Idle State after certain time has passed
            _timer += Time.deltaTime;

            if (_timer >= _stayAlertTime)
            {
                _timer = 0.0f;

                // actual state-transition respective to whether this is an standing Idle ore moving (e.g. patroling NPC)
                if (_behaviourCtrl.IsStandingIdle)
                    _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.IdleState);
                else
                    _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.MovementState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Alert</color>' to '<color=orange>{_behaviourCtrl.StateMachine.CurrentState.StateName}</color>' should have been happend now!");
            }
        }

        public override void ExecutePhysicsUpdate()
        {
            base.ExecutePhysicsUpdate();
        }

        public override void ExecuteOnAnim﻿ationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerTyoe)
        {
            base.ExecuteOnAnim﻿ationTriggerEvent(animTriggerTyoe);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }
        #endregion
    }
}