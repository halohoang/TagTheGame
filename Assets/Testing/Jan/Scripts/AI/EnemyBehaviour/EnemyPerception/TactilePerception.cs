using NaughtyAttributes;
using Player;
using UnityEngine;
using UnityEngine.Events;

public class TactilePerception : MonoBehaviour
{
    #region Events
    //--------------------------------
    // - - - - -  E V E N T S  - - - - 
    //--------------------------------
    public static event UnityAction<bool, GameObject> OnMeleeAttack;
    public static event UnityAction<bool, GameObject> OnCollidingWithOtherEnemy;

    #endregion

    #region Variables
    //--------------------------------------
    // - - - - -  V A R I A B L E S  - - - - 
    //--------------------------------------

    [Header("Monitoring Values")]
    [SerializeField, ReadOnly] private bool _isInAttackRange;
    [SerializeField, ReadOnly] private bool _isPlayerDead;
    [SerializeField, ReadOnly] private bool _isCollidingWithOtherEnemy;

    // - - - Properties - - -
    public bool IsInAttackRange { get => _isInAttackRange; private set => _isInAttackRange = value; }

    #endregion

    #region Methods
    //----------------------------------
    // - - - - -  M E T H O D S  - - - - 
    //----------------------------------
    private void OnEnable()
    {
        PlayerStats.OnPlayerDeath += SetIsPlayerDead;
    }
    private void OnDisable()
    {
        PlayerStats.OnPlayerDeath -= SetIsPlayerDead;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _isCollidingWithOtherEnemy = true;
            //NavAgent.isStopped = true;
            //CollisionObjectPos = collision.transform.position;

            OnCollidingWithOtherEnemy?.Invoke(_isCollidingWithOtherEnemy, collision.gameObject);

            Debug.Log($"'<color=lime>{gameObject.name}</color>': collided with '{collision.gameObject.name}'");
        }
        else
            _isCollidingWithOtherEnemy = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    // ---------- Methods ----------
    private void OnTriggerStay2D(Collider2D collision)
    {
        // when Player is alive invoke MeleeAttack Event for informing, that Player is in Attack Range
        if (collision.TryGetComponent(out PlayerHealth playerHealth) && !_isPlayerDead)
        {
            IsInAttackRange = true;
            OnMeleeAttack?.Invoke(IsInAttackRange, collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // when Player is alive invoke MeleeAttack Event for informing, that Player is not in Attack Range anymore
        if (collision.TryGetComponent(out PlayerHealth playerHealth) && !_isPlayerDead)
        {
            IsInAttackRange = false;
            OnMeleeAttack?.Invoke(IsInAttackRange, collision.gameObject);
        }
    }

    private void SetIsPlayerDead(bool playerDeadStatus)
    {
        _isPlayerDead = playerDeadStatus;
    }
    #endregion
}