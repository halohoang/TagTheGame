using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
	// Variables

	// Functions
	public void Play()
	{
		SceneManager.LoadScene(1); // Load First Level
	}

	public void Quit()
	{
		Application.Quit();
	}
}
