using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMashine;
using Enemies;
using UnityEngine.Rendering.Universal;

public class EnemyHealth : MonoBehaviour
{
    // Variables
    [SerializeField] private float _maximumHealth;
    [SerializeField] internal float _currentHealth;
    [SerializeField] private float _takenDamage;
    private TakingDamage _takingDamageScript;

    // Light & Shadow //
    [SerializeField] GameObject _light2d;
    ShadowCaster2D _shadowCaster2D;

	/* Animation */
	[SerializeField] Animator _animator;
    private BoxCollider2D _boxCollider2D;
    private bool _isDead = false;
    SpriteRenderer _spriteRenderer;

    /* Spawning Blood Variables */
    [SerializeField] List<GameObject> _bloodPoolSpawn; //Spawning blood pool when hit
    [SerializeField] GameObject _bloodDeadPrefab; //Spawning blood pool at where enemy die

    // Functions
    void Start()
    {
        _currentHealth = _maximumHealth;
        _takingDamageScript = GetComponent<TakingDamage>();
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        SpriteRenderer _spriteRenderer = GetComponent<SpriteRenderer>();
         _shadowCaster2D = GetComponent<ShadowCaster2D>();
      
    }

    internal void GetDamage()
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= _takenDamage;
            if (_takingDamageScript != null)
            {
                _takingDamageScript.FlashOnce();

                /* Spawn blood and stay on the ground while enemy moving*/
                int randomIndex = Random.Range(0, _bloodPoolSpawn.Count);
                Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
                Instantiate(_bloodPoolSpawn[randomIndex], transform.position, bloodRotation);
            }
        }
        if (_currentHealth <= 0 && _isDead == false)
        {
            // Randomize dead rotation
            this.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
            _animator.SetTrigger("Dead");
            _isDead = true;
            _boxCollider2D.isTrigger = true;
            _light2d.SetActive(false);
            _shadowCaster2D.enabled = false;

            // Setup Enemy-Behaviour to EnemyDead
            // todo: if AI-Logic/StateMachine is fully implemented, adjust following Logic accordingly; JM (30.10.23)
            if (gameObject.TryGetComponent(out EnemyQuickfixBehaviour_ForTesting enemyQuickFixBehav))
            {
                // todo: delete this Query if StateMachine and according new AI-Logic is fully set up and functioning; JM (30.10.23)
                enemyQuickFixBehav.IsEnemyDead = _isDead;
                enemyQuickFixBehav.enabled = false;
            }
            else
            {
                Debug.Log($"<color=lime>No Quickfix behaviour cold be found on {this.gameObject.name} so ohter Behavióur-Scripts will be disabled</color>");
                gameObject.GetComponent<ConditionPlayerDetectionCheck>().SetIsEnemyDead(_isDead);
                gameObject.GetComponent<ConditionPlayerDetectionCheck>().enabled = false;
                gameObject.GetComponent<BaseEnemyBehaviour>().enabled = false;
            }

            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* Spawn blood where enemy die */
        Quaternion bloodRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
        Instantiate(_bloodDeadPrefab, transform.position, bloodRotation);
    }
}