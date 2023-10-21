using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scriptable Objects/InputReader")]
public class InputReaderSO : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions   // implementing Interfaces for Input Callbacks
{
    //--------------- Events ---------------
    public event UnityAction<Vector2> OnMovementInput;
    public event UnityAction<Vector2> OnMouseMovement;
    public event UnityAction OnDashInput;
    public event UnityAction OnAttackInput;
    public event UnityAction OnInteractionInput;
    public event UnityAction OnReloadingInput;


    //--------------- Fields ---------------
    private GameInput _gameInput;

    public GameInput GameInput { get => _gameInput; private set => _gameInput = value; }


    //--------------- Methods ---------------
    //---------- Unity-Executed Methods ----------
    private void OnEnable()
    {
        if (GameInput == null)
            GameInput = new GameInput();

        // Setting Callbacks and enabeling the spicific InputMaps
        GameInput.Player.SetCallbacks(this);
        GameInput.Player.Enable();

        GameInput.UI.SetCallbacks(this);
        GameInput.UI.Enable();
    }

    private void OnDisable()
    {
        // Disableing the specific InputMaps
        GameInput.Player.Disable();

        GameInput.UI.Disable();
    }


    //---------- Input-Callback Methods, Player-Related ----------
    public void OnMovement(InputAction.CallbackContext context)
    {
        OnMovementInput?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnRotation(InputAction.CallbackContext context)
    {
        OnMouseMovement?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAttacking(InputAction.CallbackContext context)
    {
        if (context.started)
            OnAttackInput?.Invoke();
    }

    public void OnDashing(InputAction.CallbackContext context)
    {
        OnDashInput?.Invoke();
        //Debug.Log($"space was pressed");
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.started)
            OnInteractionInput?.Invoke();
    }

    public void OnReloading(InputAction.CallbackContext context)
    {
        if (context.started)
            OnReloadingInput?.Invoke();
    }


    //---------- Input-Callback Methods, UI-related ----------
    public void OnTogglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.started)
            Debug.Log($"<color=orange> 'Esc'-Key was pressed </color>");
    }
}
