using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuManagement
{
    public class MenuButtonHandler : MonoBehaviour
    {
        //------------------------------ Fields ------------------------------
        [SerializeField] private InputReaderSO _inputReaderSO;

        //------------------------------ Methods ------------------------------

        //---------- Custom Methods ----------
        private void Awake()
        {
            // NullCheck and Autoreferencing
            if (_inputReaderSO == null)
            {
                _inputReaderSO = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;
                Debug.Log($"<color=yellow>Caution! Reference for Scriptable Object 'InputReaderSO' was not set in Inspector of '{this}'.Trying to set automatically.</color>");
            }
        }

        public void LoadScene(int sceneToLoad)
        {
            // general setup for changing scene
            Time.timeScale = 1;                         // in case TimeScale was set to 0 in PauseMenu or so            
            _inputReaderSO.GameInput.Player.Enable();   // in case Player-Input was disabled bfore changing Scene (e.g. thats the case when enabling PausMenu)

            SceneManager.LoadScene(sceneToLoad);
        }
        
        public void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
        }
    }
}