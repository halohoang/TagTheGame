using Enemies;
using EnumLibrary;
using UnityEngine;

public class BaseEnemyInvestigationStateSO : MonoBehaviour
{
    #region Variables
    //--------------------------------------
    // - - - - -  V A R I A B L E S  - - - - 
    //--------------------------------------

    protected NPCBehaviourController _behaviourCtrl;
    //protected MeleeEnemyBehaviour _meleeEnemyBehaviour;
    //protected RangeEnemyBehaviour _rangeEnemyBehaviour;
    protected Transform _transform;
    protected GameObject _gameObject;

    protected Transform _playerTransform;

    private float _investigationTime;
    private float _timer;
    private Vector3 _previousEventPosition;
    #endregion


    #region Methods
    //----------------------------------
    // - - - - -  M E T H O D S  - - - - 
    //----------------------------------

    public virtual void Initialize(GameObject enemyObj, NPCBehaviourController enemyBehav)
    {
        this._gameObject = enemyObj;
        this._transform = enemyObj.transform;
        this._behaviourCtrl = enemyBehav;

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

    public virtual void ExecuteOnE﻿nterState()
    {
        //Todo: Message to UI-Managr for BehaviourFeedback
        _investigationTime = Random.Range(3.0f, 8.0f);
    }

    public virtual void ExecuteOnExitState()
    {
        ResetValues();
    }

    public virtual void Execute﻿FrameUpdate()
    {
        // Transition-Condition-Check
        if (_behaviourCtrl.IsTargetDetected)
        {
            //Todo: Message to UI-Managr for BehaviourFeedback
            _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.ChaseState);
            Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Chase</color>' should have been happend now!");
            return;
        }
        else // start investigatino behaviour
        {
            //Todo: Message to UI-Managr for BehaviourFeedback

            // set timer for investigation-behaviour
            _timer += Time.deltaTime;

            if (_timer == _investigationTime)
            {

                // after timer runs out send event to inform, that state transition is coming from investigation State and transit to back to normal behaviour -> NPC walks to his last Idle-Position or last Waypoint
                return;
            }
        }
    }

    public virtual void ExecutePhysicsUpdate() { }
    public virtual void ExecuteOnAnim﻿ationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerTyoe) { }

    public virtual void ResetValues()
    {
        _behaviourCtrl.SetIsSomethingAlarmingHappening(false);  // reset value to false on leaving state
    }

    #endregion
}
