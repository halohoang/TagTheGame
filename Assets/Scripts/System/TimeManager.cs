using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	private float defaultTimeScale = 1.0f; // The default time scale (normal speed).
	private float slowTimeScale = 0.5f;    // The slow time scale (50% speed).

	private void Update()
	{
		// Check if the player is holding down the Space key.
		bool isSlowingTime = Input.GetKey(KeyCode.Space);

		// Set the time scale for all game objects and components.
		if (isSlowingTime)
		{
			Time.timeScale = slowTimeScale; // Slow down time.
		}
		else
		{
			Time.timeScale = defaultTimeScale; // Normal time.
		}
	}
}
