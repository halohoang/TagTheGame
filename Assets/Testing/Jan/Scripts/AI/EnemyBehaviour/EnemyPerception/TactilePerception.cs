using Enemies;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(BaseEnemyBehaviour))]
public class TactilePerception : MonoBehaviour
{
    [SerializeField, ReadOnly] private BaseEnemyBehaviour _enemyBehav;

    private void Awake()
    {
        _enemyBehav = GetComponent<BaseEnemyBehaviour>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
