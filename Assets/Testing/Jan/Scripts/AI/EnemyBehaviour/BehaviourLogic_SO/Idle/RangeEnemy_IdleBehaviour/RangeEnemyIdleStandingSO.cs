using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RangeEnemy_Idle_Standing", menuName = "Scriptable Objects/Enemy Logic/Idle Logic/RangeEnemy Standing (Just Standing Idle)")]
    public class RangeEnemyIdleStandingSO : BaseEnemyIdleSO
    {
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

            // Transitionchecks 
            // Switch State from Idle to AttackState (Shooting) when Player is Detected
            if (_behaviourCtrl.IsTargetDetected)
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.AttackState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Attack (Shooting/RangeAttack)</color>' should have been happend now!");
                return;
            }
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
    }
}