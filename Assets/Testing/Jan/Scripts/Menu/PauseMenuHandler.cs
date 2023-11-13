using JansLittleHelper;
using NaughtyAttributes;
using UnityEngine;

namespace MenuManagement
{
    public class PauseMenuHandler : MonoBehaviour
    {
        //------------------------------ Fields ------------------------------
        [Header("References to Resources")]
        [Space(2)]
        [Tooltip("The Scriptable Object called 'InputReader' needs to be referenced here. (To be found in Assets/Resources/ScriptableObjects in Hierarchy)")]
        [SerializeField] private InputReaderSO _inputReaderSO;
        //[Tooltip("Ingame Mouse Cursor as Crosshair for better Aiming")]
        //[SerializeField] private Texture2D _ingameCrosshair;
        //[Tooltip("Menu Mouse Cursor as actual cursor for better menu maneuvering")]
        //[SerializeField] private Texture2D _menuMouseCursor;
        [Space(3)]
        [Header("References to GameObjects")]
        [Space(2)]
        [Tooltip("GameObject of the 'PauseMenu-Panel' (To be found in 'Ingame-Menu_Canvas' in Hierarchy)")]
        [SerializeField] private GameObject _pauseMenu;
        [Tooltip("GameObject of the 'HowToPlay-Panel' (To be found in 'Ingame-Menu_Canvas' in Hierarchy)")]
        [SerializeField] private GameObject _howToPlayMenu;
        [Space(5)]
        [Tooltip("Array of Objects in Scene with Tag 'MenuPanel' for Nullchecking and Autoreferencing MenuPanels.")]
        [SerializeField, ReadOnly] private GameObject[] _menuPanels;

        private bool _isGamePaused = false;


        //------------------------------ Methods ------------------------------

        //---------- Unity-Executed Methods ----------
        private void Awake()
        {
            #region Null Checks And Autoreferencing
            // Null Checks And Autoreferencing
            _menuPanels = GameObject.FindGameObjectsWithTag("MenuPanel");

            _pauseMenu = NullChecksAndAutoReferencing.CheckAndGetGameObject(_pauseMenu, "PauseMenu_Panel", _menuPanels);
            _howToPlayMenu = NullChecksAndAutoReferencing.CheckAndGetGameObject(_howToPlayMenu, "HowToMenu_Panel", _menuPanels);

            if (_inputReaderSO == null)
            {
                _inputReaderSO = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;
                Debug.Log($"<color=yellow>Caution! Reference for Scriptable Object 'InputReaderSO' was not set in Inspector of '{this}'.Trying to set automatically.</color>");
            }

            if (_pauseMenu == null)
            {
                _pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
                Debug.LogWarning($"<color=yellow>Caution! Reference for 'PauseMenu-Panel' was not set in Inspector in '{this}'. Trying to set automatically.</color>");
            }

            //if (_ingameCrosshair == null)
            //{
            //    _ingameCrosshair = Resources.Load("Sprites/WeaponCrosshair/OutlineRetina/crosshair161") as Texture2D;
            //    Debug.LogWarning($"<color=yellow>Caution! Reference for 'Ingame Crosshair' was not set in Inspector in '{this}'.Setting to default Crosshair Texture.</color>");
            //}

            //if (_menuMouseCursor == null)
            //{
            //    _menuMouseCursor = Resources.Load("Sprites/WeaponCrosshair/OutlineRetina/crosshair161") as Texture2D;
            //    Debug.LogWarning($"<color=yellow>Caution! Reference for 'Ingame Crosshair' was not set in Inspector in '{this}'.Setting to default Crosshair Texture.</color>");
            //}
            #endregion
        }

        private void OnEnable()
        {
            InputReaderSO.OnEscPress += TogglePauseMenu;
        }

        private void OnDisable()
        {
            InputReaderSO.OnEscPress -= TogglePauseMenu;
        }

        /// <summary>
        /// Resuming to Game by disableing PauseMenu and setting TimeScale back to 1 
        /// </summary>
        public void ResumeGame()    // Disable PauseMenu
        {
            if (_isGamePaused && !_howToPlayMenu.activeSelf)
            {
                _pauseMenu.SetActive(false);
                _isGamePaused = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                Time.timeScale = 1;

                _inputReaderSO.GameInput.Player.Enable();

                Debug.Log("PauseMenu was disabled, resume to Game.");
            }
        }

        /// <summary>
        /// Toggle PausMenu, When PausMenu is activated it will be closed and the TimeScale will be set to 1 again, if PauseMenu is disabled and game is running the opposite happens
        /// the other way round
        /// </summary>
        private void TogglePauseMenu()
        {
            if (!_isGamePaused) // enable PauseMenu
            {
                Time.timeScale = 0;
                _inputReaderSO.GameInput.Player.Disable();

                _pauseMenu.SetActive(true);
                _isGamePaused = true;
                //Cursor.SetCursor()
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Debug.Log("PauseMenu was enabled, Game is paused");
            }
            else if (_isGamePaused && !_howToPlayMenu.activeSelf) // Disable PauseMenu
            {
                _pauseMenu.SetActive(false);
                _isGamePaused = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                Time.timeScale = 1;

                _inputReaderSO.GameInput.Player.Enable();

                Debug.Log("PauseMenu was disabled, resume to Game");
            }
        }

        /// <summary>
        /// Toggle Submenus if 'Ecs'-key is pressend and wether 'OptionsMenu-Panel' or 'SelectSaveGame-Panel' is enabled the according menu shall be disabled
        /// and the PauseMenu-Panel shall be enabled again (since it will be disabled by pressing the Options/LoadGame-Button)
        /// </summary>
        private void DisableSubMenus()
        {
            if (_howToPlayMenu.activeSelf)
            {
                _howToPlayMenu.SetActive(false);
                _pauseMenu.SetActive(true);
            }
        }
    }
}