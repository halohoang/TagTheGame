using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public static event UnityAction<bool> OnRestartScene;

	// Variables
	[SerializeField] private GameObject _pauseMenu;
	[SerializeField] private InputReaderSO _inputReaderSO;
	private bool _isGamePaused = false;
	// Functions

	private void OnEnable()
	{
		InputReaderSO.OnEscPress += TogglePauseMenu;
	}

	private void OnDisable()
	{
		InputReaderSO.OnEscPress -= TogglePauseMenu;
	}

	private void TogglePauseMenu()
	{
		if (!_isGamePaused) // enable PauseMenu
		{
			Time.timeScale = 0;
			_inputReaderSO.GameInput.Player.Disable();

			_pauseMenu.SetActive(true);
			_isGamePaused = true;
			//Cursor.SetCursor()
			//Cursor.visible = true;
			//Cursor.lockState = CursorLockMode.None;

			Debug.Log("PauseMenu was enabled, Game is paused");
		}
		else if (_isGamePaused /*&& !_howToPlayMenu.activeSelf*/) // Disable PauseMenu
		{
			_pauseMenu.SetActive(false);
			_isGamePaused = false;
			//Cursor.visible = false;
			//Cursor.lockState = CursorLockMode.Locked;			

			Time.timeScale = 1;

			_inputReaderSO.GameInput.Player.Enable();

			Debug.Log("PauseMenu was disabled, resume to Game");
		}
	}

	public void ResumeGame()    // Disable PauseMenu
	{
		if (_isGamePaused /*&& !_howToPlayMenu.activeSelf*/)
		{
			_pauseMenu.SetActive(false);
			_isGamePaused = false;
			//Cursor.visible = false;
			//Cursor.lockState = CursorLockMode.Locked;

			Time.timeScale = 1;

			_inputReaderSO.GameInput.Player.Enable();

			Debug.Log("PauseMenu was disabled, resume to Game.");
		}
	}
	public void RestartScene()
	{
		bool isSceneRestarted = true;
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		OnRestartScene?.Invoke(isSceneRestarted);
	}
	public void Quit()
	{
		Application.Quit();
	}
}
