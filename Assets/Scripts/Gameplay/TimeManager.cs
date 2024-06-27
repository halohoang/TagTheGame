using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	private float defaultTimeScale = 1.0f; // The default time scale (normal speed).
	private float slowTimeScale = 0.5f;    // The slow time scale (50% speed).

	/* Reference to other shake script */
	[SerializeField] private CameraRecoilShake _cameraShake;
	

	private void Update()
	{
		// Check if the player is holding down the Space key.
		bool isSlowingTime = Input.GetKey(KeyCode.Space);

		// Set the time scale for all game objects and components.
		if (isSlowingTime)
		{
			_cameraShake.enabled = false;
			Time.timeScale = slowTimeScale; // Slow down time.
		}
		else
		{
			_cameraShake.enabled = true;

			Time.timeScale = defaultTimeScale; // Normal time.
		}
	}
}
