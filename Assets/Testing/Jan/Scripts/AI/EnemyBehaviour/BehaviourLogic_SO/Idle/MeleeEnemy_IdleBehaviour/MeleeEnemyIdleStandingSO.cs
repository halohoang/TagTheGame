using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MeleeEnemy_Idle_Standing", menuName = "Scriptable Objects/Enemy Logic/Idle Logic/MeleeEnemy Standing (Just Standing Idle)")]
    public class MeleeEnemyIdleStandingSO : BaseEnemyIdleSO
    {
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

        public override void ExecuteOnE﻿nterState()
        {
            base.ExecuteOnE﻿nterState();
        }

        public override void ExecuteOnExitState()
        {
            base.ExecuteOnExitState();
        }

        public override void ExecuteFrameUpdate()
        {
            base.ExecuteFrameUpdate();            
        }

        public override void ExecutePhysicsUpdate()
        {
            base.ExecutePhysicsUpdate();
        }        

        public override void ExecuteOnAnim﻿﻿ationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerTyoe)
        {
            base.ExecuteOnAnim﻿﻿ationTriggerEvent(animTriggerTyoe);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }
        #endregion
    }
}