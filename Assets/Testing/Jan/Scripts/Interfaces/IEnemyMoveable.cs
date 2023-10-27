using UnityEngine;
using UnityEngine.AI;

public interface IEnemyMoveable
{
    NavMeshAgent NavAgent { get; set; }

    void EnemyMovement(NavMeshAgent navAgent);

    void EnemyRotation();
}
