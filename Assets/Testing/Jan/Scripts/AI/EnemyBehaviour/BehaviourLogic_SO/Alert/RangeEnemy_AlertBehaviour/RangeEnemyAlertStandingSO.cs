using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RangeEnemy_Alert_Stading", menuName = "Scriptable Objects/Enemy Logic/Alert Logic/RangeEnemy Alert Standing")]
    public class RangeEnemyAlertStandingSO : BaseEnemyAlertSO
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

            // Transition-Check; If Player is detected change to Attack State (shooting)
            if (_behaviourCtrl.IsTargetDetected)
            {
                _behaviourCtrl.StateMachine.Transition(_behaviourCtrl.AttackState);
                Debug.Log($"{_behaviourCtrl.gameObject.name}: State-Transition from '<color=orange>Idle</color>' to '<color=orange>Attack (Shooting)</color>' should have been happend now!");
                return;
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
    }
}