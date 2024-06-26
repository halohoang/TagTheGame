using JansLittleHelper;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static event UnityAction OnRestartScene;
    public static event UnityAction<bool> OnTogglePauseScene;

    // Variables
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private InputReaderSO _inputReaderSO;
    private bool _isGamePaused = false;
    [Space(5)]

    [SerializeField, ReadOnly] private GameObject[] _menuObjects;

    public bool IsGamePaused { get => _isGamePaused; private set => _isGamePaused = value; }

    // Functions

    private void Start()
    {
        _menuObjects = GameObject.FindGameObjectsWithTag("MenuPanel");

        if (_pauseMenu == null)
            _pauseMenu = NullChecksAndAutoReferencing.CheckAndGetGameObject(_pauseMenu, "PauseMenu", _menuObjects);

        if (_optionsMenu == null)
            _optionsMenu = NullChecksAndAutoReferencing.CheckAndGetGameObject(_optionsMenu, "OptionsMenu", _menuObjects);
    }

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
        if (!IsGamePaused) // enable PauseMenu
        {
            Time.timeScale = 0;
            _inputReaderSO.GameInput.Player.Disable();

            _pauseMenu.SetActive(true);
            IsGamePaused = true;
            //Cursor.SetCursor()
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;

            OnTogglePauseScene?.Invoke(IsGamePaused);

            //Debug.Log("PauseMenu was enabled, Game is paused");
        }
        else if (IsGamePaused && _optionsMenu.activeSelf) // Disable PauseMenu and optionsMenu if that is open
        {
            _pauseMenu.SetActive(false);
            _optionsMenu.SetActive(false);
            IsGamePaused = false;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;			

            Time.timeScale = 1;

            _inputReaderSO.GameInput.Player.Enable();

            OnTogglePauseScene?.Invoke(IsGamePaused);

            Debug.Log("PauseMenu was disabled, resume to Game");
        }
        else if (IsGamePaused) // Disable PauseMenu
        {
            _pauseMenu.SetActive(false);
            IsGamePaused = false;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;			

            Time.timeScale = 1;

            _inputReaderSO.GameInput.Player.Enable();

            OnTogglePauseScene?.Invoke(IsGamePaused);

            Debug.Log("PauseMenu was disabled, resume to Game");
        }
    }

    public void ResumeGame()    // Disable PauseMenu
    {
        if (IsGamePaused /*&& !_howToPlayMenu.activeSelf*/)
        {
            _pauseMenu.SetActive(false);
            IsGamePaused = false;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1;

            _inputReaderSO.GameInput.Player.Enable();

            OnTogglePauseScene?.Invoke(IsGamePaused);

            Debug.Log("PauseMenu was disabled, resume to Game.");
        }
    }
    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        OnRestartScene?.Invoke();
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        if (soundManager != null)
        {
            soundManager.ResetBackgroundSongPlaying();
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        _inputReaderSO.GameInput.Player.Enable();
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        if (soundManager != null)
        {
            soundManager.ResetBackgroundSongPlaying();
            soundManager.StopPersistence(); // Stop the SoundManager from persisting
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
    }

}

