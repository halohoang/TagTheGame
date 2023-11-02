using Enemies;
using EnumLibrary;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Enemy-Alert-Stading", menuName = "Scriptable Objects/Enemy Logic/Alert Logic/Standing Alert")]
    public class EnemyAlertStandingSO : BaseEnemyAlertSO
    {
        //[SerializeField, Range(0.5f, 10.0f)] private float _stayAlertTime;

        //private float _timer = 0.0f;

        public override void Initialize(GameObject enemyObj, BaseEnemyBehaviour enemyBehav)
        {
            base.Initialize(enemyObj, enemyBehav);
        }

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
    }
}