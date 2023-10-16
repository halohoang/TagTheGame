using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakingDamage : MonoBehaviour
{
	// Variables
	private SpriteRenderer _spriteRenderer;

	/*Flashing Effect */
	[SerializeField] private float _flashingSpeed = 0;
	[SerializeField] private float _flashDuration = 0.1f; // Duration of the flashing effect

	private bool isFlashing = false;

	/* Turns color effect */
	private Color defaultColor = Color.white;

	void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		defaultColor = _spriteRenderer.color;
	}

	void Update()
	{
		if (isFlashing)
		{
			FlashingEffect();
		}
	}

	internal void FlashOnce()
	{
		// Start flashing immediately
		StartCoroutine(FlashAndRevert());
	}

	private IEnumerator FlashAndRevert()
	{
		isFlashing = true;

		// Turn the enemy fully red
		_spriteRenderer.color = Color.red;

		// Wait for the flashing duration
		yield return new WaitForSeconds(_flashDuration);

		// Revert to the default color
		isFlashing = false;
		_spriteRenderer.color = defaultColor;
	}

	private void FlashingEffect()
	{
		// No need to interpolate, just set to fully red
		_spriteRenderer.color = Color.red;
	}
}

