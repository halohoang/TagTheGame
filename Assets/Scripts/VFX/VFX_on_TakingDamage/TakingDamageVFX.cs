using System.Collections;
using UnityEngine;

public class TakingDamageVFX
{
    //--------------------------------------
    // - - - - -  V A R I A B L E S  - - - - 
    //--------------------------------------
    	
	private float _flashingSpeed;  // Speed of the flashing effect
	private float _flashDuration;  // Duration of the flashing effect	
    private bool isFlashing = false;

	private Color defaultColor = Color.white;       // for turning color effect
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    // Properties
    internal bool IsFlashing { get => isFlashing; private set => isFlashing = value; }


    //----------------------------------
    // - - - - -  M E T H O D S  - - - - 
    //----------------------------------

    //---------- Constructors ----------
    public TakingDamageVFX(SpriteRenderer spriteRenderer, float flashSpeed, float flashDuration)
	{
		_spriteRenderer = spriteRenderer;
        _flashingSpeed = flashSpeed;
        _flashDuration = flashDuration;
	}

    //---------- Custom Methods ----------
    internal void FlashingEffect()
    {
        // No need to interpolate, just set to fully red
        _spriteRenderer.color = Color.red;
    }

    //---------- Coroutines ----------
    internal IEnumerator FlashAndRevert()
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
}

