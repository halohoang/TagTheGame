using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
	// --- Events ---
	public static event UnityAction<bool> OnPlayerDeath;

	[Header("References")]
	[SerializeField] private InputReaderSO _inputReader;
	[Space(5)]

	// Variables
	[SerializeField] private float _maxHealth;
	[SerializeField] internal float _currentHealth;
	[SerializeField] GameObject _player;
	[SerializeField] Transform _chargeBarTransform; // Reference to the scale of the bar
	[SerializeField] float _chargeSpeed = 1; // The rate at which bar depletes or charges

	/* Dead Effec */
	private Animator _animator;
	[SerializeField] private List<GameObject> _disableGameObject;

	[Header("Monitoring Values")]
	[SerializeField, ReadOnly] private bool _isPlayerDead;

	/* Taking Damage Effect */
	private TakingDamage _takingDamageScript;

	/* Health System */
	[SerializeField] internal int _takenDamage;
	[SerializeField] private float _regenCooldown = 2f; // Adjust the duration as needed
	private bool _canRegen = true;
	private float _regenTimer = 1f;

	private Rigidbody2D _rb2D;

    public bool IsPlayerDead { get => _isPlayerDead; private set => _isPlayerDead = value; }

    //Functions
    private void Awake()
	{
		if (_inputReader == null)
			_inputReader = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;
	}

	//private void OnEnable()
	//{
	//  _inputReader.OnDashInput += ReduceHP;
	//}

	//private void OnDisable()
	//{
	//  _inputReader.OnDashInput -= ReduceHP;
	//}

	void Start()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		_currentHealth = _maxHealth;
		_takingDamageScript = GetComponent<TakingDamage>();
		_animator = GetComponent<Animator>();
	}

	void Update()
	{
		ReduceHP();
		if (_currentHealth <= 0)	// Logic for Player Death
		{
			IsPlayerDead = true;

			foreach (GameObject gameobject in _disableGameObject)
			{
				gameobject.SetActive(false);
			}
			_animator.SetTrigger("Dead");

			OnPlayerDeath?.Invoke(IsPlayerDead);
		}
		if (_canRegen)
		{
			RegenHP();
		}
		else
		{
			// Update the regen timer
			_regenTimer += Time.deltaTime;
			if (_regenTimer >= _regenCooldown)
			{
				_canRegen = true;
				_regenTimer = 0f; // Reset the timer
			}
		}
	}

	internal void GetDamage()
	{
		_currentHealth = _currentHealth - _takenDamage;
		if (_takingDamageScript != null)
		{
			_takingDamageScript.FlashOnce();
		}
	}
	void ReduceHP()
	{
		if (_player != null)
		{
			// If the player hold down Sandevistan his health bar will start to get depleted
			if (Input.GetKey(KeyCode.Space))
			{
				_currentHealth = Mathf.Max(_currentHealth - 0.5f, 1f);
				ReduceCharge();
			}

			// Ensure _currentHealth does not go below 0
			_currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);



		}
	}

	void RegenHP()
		{
			if (!Input.GetKey(KeyCode.Space) && _currentHealth < _maxHealth) { _currentHealth += 1; RegenCharge(); _canRegen = false; } _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
		}
		

	

	void ReduceCharge()
	{
		if (_chargeBarTransform != null)
		{
			// Calculate the proportional health scale
			float healthScale = _currentHealth / _maxHealth;

			// Calculate the scaled health bar value based on the proportional scale
			float scaledHealthBar = healthScale * 0.16f;

			// Clamp the scaled health bar value between 0 and 0.16
			float clampedScale = Mathf.Clamp(scaledHealthBar, 0f, 0.16f);

			// Update health bar scale based on clamped value
			_chargeBarTransform.localScale = new Vector3(_chargeBarTransform.localScale.x, clampedScale, _chargeBarTransform.localScale.z);
		}
	}

	void RegenCharge()
	{
		if (_chargeBarTransform != null)
		{
			// Calculate the proportional health scale
			float healthScale = _currentHealth / _maxHealth;

			// Calculate the scaled health bar value based on the proportional scale
			float scaledHealthBar = healthScale * 0.16f;

			// Clamp the scaled health bar value between 0 and 0.16
			float clampedScale = Mathf.Clamp(scaledHealthBar, 0f, 0.16f);

			// Update health bar scale based on clamped value
			_chargeBarTransform.localScale = new Vector3(_chargeBarTransform.localScale.x, clampedScale, _chargeBarTransform.localScale.z);
		}
	}

}
