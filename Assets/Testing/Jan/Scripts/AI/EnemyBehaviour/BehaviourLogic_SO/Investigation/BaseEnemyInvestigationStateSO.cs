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
            _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.ChaseState);
            Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Chase</color>' should have been happend now!");
            return;
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
