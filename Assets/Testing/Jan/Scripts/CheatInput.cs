using Player;
using UnityEngine;
using UnityEngine.Events;

public class CheatInput : MonoBehaviour
{
    public static event UnityAction OnResetDoors;               // for sending message to NavMeshBuilder.cs to update NavMeshSurface
    public static event UnityAction<bool> OnSetGodMode;         // informing UI-Manager on GodMode Set
    public static event UnityAction<float> OnTimeScaleChange;   // inforng UI-Manager if TimeSclae was manipulated

    [Header("References")]
    [Space(2)]
    [SerializeField] private GameObject[] _interactableDoorObjects;
    [SerializeField] private PlayerStats _playerStatsScript;

    private void Awake()
    {
        #region AutoReferencing

        _interactableDoorObjects = GameObject.FindGameObjectsWithTag("Door");

        if (_playerStatsScript == null)
            _playerStatsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        #endregion
    }

    private void Update()
    {
        // decrease TimeScale on pressing 'Plus-Key' on NumPad -> TimeLapse
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Time.timeScale += 0.1f;
            OnTimeScaleChange?.Invoke(Time.timeScale);
        }

        // decrease TimeScale on pressing 'Minus-Key' on NumPad -> Slowmotion
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (Time.timeScale > 0.09999999f)   // check for avoiding the timeScale get out of range
                Time.timeScale -= 0.1f;
            OnTimeScaleChange?.Invoke(Time.timeScale);
        }

        // Reset Timescale to normal on pressing 'Multiply-Key' on Numpad
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            Time.timeScale = 1.0f;
            OnTimeScaleChange?.Invoke(Time.timeScale);
        }

        // If '0' ond NumPad is Pressed all disabled Doors will be enabled again
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            for (int i = 0; i < _interactableDoorObjects.Length; i++)
            {
                if (!_interactableDoorObjects[i].activeSelf)
                    _interactableDoorObjects[i].SetActive(true);
            }

            OnResetDoors?.Invoke();

            /* todo: (!)if runtime rebaking of Navmesh on Door-KickIn is implemented already execute a new runtime rebaking of the NavMesh here as well to avoid
             * Navmesh-Bugs, since the Doors will be reactivated again; JM (09.Oct.2023) */
        }

        //Enabling GodMode
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (_playerStatsScript.IsPlayerInvincible)
                _playerStatsScript.SetPlayerInvincibleStatus(false);
            else
                _playerStatsScript.SetPlayerInvincibleStatus(true);

            OnSetGodMode?.Invoke(_playerStatsScript.IsPlayerInvincible);
        }
    }
}
