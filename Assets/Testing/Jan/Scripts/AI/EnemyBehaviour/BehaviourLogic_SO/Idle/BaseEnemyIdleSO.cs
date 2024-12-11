using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{    
    public class BaseEnemyIdleSO : ScriptableObject
    {
        #region Variables
        //--------------------------------------
        // - - - - -  V A R I A B L E S  - - - - 
        //--------------------------------------

        protected NPCBehaviourController _behaviourCtrl;
        //protected MeleeEnemyBehavCtrl _meleeEnemyBehaviour;
        //protected RangeEnemyBehavCtrl _rangeEnemyBehaviour;
        protected Transform _transform;
        protected GameObject _gameObject;

        protected Transform _playerTransform;
        #endregion

        #region Methods
        //----------------------------------
        // - - - - -  M E T H O D S  - - - - 
        //----------------------------------

        public virtual void Initialize(GameObject enemyObj, NPCBehaviourController behavCtrl)
        {
            this._gameObject = enemyObj;
            this._transform = enemyObj.transform;
            this._behaviourCtrl = behavCtrl;

            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        //public virtual void Initialize(GameObject enemyObj, MeleeEnemyBehavCtrl meleeEnemyBehav)
        //{
        //    this._gameObject = enemyObj;
        //    this._transform = enemyObj.transform;
        //    this._meleeEnemyBehaviour = meleeEnemyBehav;

        //    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //}
        //public virtual void Initialize(GameObject enemyObj, RangeEnemyBehavCtrl rangeEnemyBehav)
        //{
        //    this._gameObject = enemyObj;
        //    this._transform = enemyObj.transform;
        //    this._rangeEnemyBehaviour = rangeEnemyBehav;

        //    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //}

        public virtual void ExecuteOnE﻿nterState() 
        {
            //// initial check if this NPC shall be actually an standing ore moving NPC. If it is not an standing idle NPC -> transit to Movement-State
            //if (!_behaviourCtrl.IsStandingIdle)
            //    _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.MovementState);
        }


        public virtual void ExecuteOnExitState()
        {
            ResetValues();
        }

        public virtual void ExecuteFrameUpdate()
        {
            // Transitionchecks 
            // Switch State from Idle to AlertState when something alarming is happening (e.g. door kick in, player shoots etc.) and Agent is in noise range
            if (_behaviourCtrl.IsSomethingAlarmingHappening || _behaviourCtrl.IsTargetDetected)
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.AlertState);
                Debug.Log($"<color=orange> AI-Behav: {_behaviourCtrl.gameObject.name}</color>: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Alert</color>' should have been happend now!");
            }

            #region old transiton to ChaseState
            //// Switch State from Idle to ChaseState when target object is Detected
            //if (_behaviourCtrl.IsTargetDetected)
            //{
            //    _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.ChaseState);
            //    Debug.Log($"<color=orange> AI-Behav: {_behaviourCtrl.gameObject.name}</color>: State-Transition from '<color=orange>Idle</color>' to '<color=orange>AlertState</color>' should have been happend now!");
            //}
            #endregion
        }

        public virtual void ExecutePhysicsUpdate() { }
        public virtual void ExecuteOnAnim﻿﻿ationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }
        public virtual void ResetValues() { }

        #endregion
    }
}