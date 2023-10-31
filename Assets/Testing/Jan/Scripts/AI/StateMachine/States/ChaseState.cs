using Enemies;
using EnumLibrary;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace StateMashine
{
    public class ChaseState : BaseState
    {
        public ChaseState(BaseEnemyBehaviour enemyBehav, EnemyStateMachine enemyStaMa) : base(enemyBehav, enemyStaMa)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            
            // setup NavMeshAgent Properties
            _enemyBehaviour.NavAgent.speed = _enemyBehaviour.ChasingSpeed;

            // Set proper Animation
            _enemyBehaviour.Animator.SetBool("Engage", true);
        }

        public override void ExitState()
        {
            base.ExitState();

            // setup NavMeshAgent Properties
            _enemyBehaviour.NavAgent.speed = _enemyBehaviour.MovementSpeed;

            // Set proper Animation
            _enemyBehaviour.Animator.SetBool("Engage", false);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // Set Movement-Destination for NavMeshAgent
            _enemyBehaviour.NavAgent.SetDestination(_enemyBehaviour.PlayerObject.transform.position);

            // facing Player Position
            _enemyBehaviour.gameObject.transform.right = _enemyBehaviour.PlayerObject.transform.position - _enemyBehaviour.gameObject.transform.position;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void AnimationTriggerEvent(Enum_Lib.EAnimationTriggerType animTriggerType)
        {
            base.AnimationTriggerEvent(animTriggerType);
        }
    }
}