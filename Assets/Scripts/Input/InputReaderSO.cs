using EnumLibrary;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scriptable Objects/InputReader")]
public class InputReaderSO : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions   // implementing Interfaces for Input Callbacks
{
    //--------------- Events ---------------
    public event UnityAction<Vector2> OnMovementInput;
    public event UnityAction<Vector2> OnMouseMovement;
    public event UnityAction<Enum_Lib.ESpaceKey> OnFastMovementInput;
    public event UnityAction OnAttackInput;
    public event UnityAction OnInteractionInput;
    public event UnityAction OnReloadingInput;
    public event UnityAction OnWeaponSwitch;
    public event UnityAction OnPrimarytWeaponEquip;
    public event UnityAction OnSecondaryWeaponEquip;
    public event UnityAction OnHolsteringWeapons;    
    public static event UnityAction OnEscPress;


    //--------------- Fields ---------------
    private GameInput _gameInput;

    public GameInput GameInput { get => _gameInput; private set => _gameInput = value; }


    //--------------- Methods ---------------
    //---------- Unity-Executed Methods ----------
    internal void OnEnable()
    {
        if (GameInput == null)
            GameInput = new GameInput();

        // Setting Callbacks and enabeling the spicific InputMaps
        GameInput.Player.SetCallbacks(this);
        GameInput.Player.Enable();

        GameInput.UI.SetCallbacks(this);
        GameInput.UI.Enable();
        Debug.Log($"<color=magenta> OnEnable() was called in {this} </color>");
    }

    private void OnDisable()
    {
        // Disableing the specific InputMaps
        GameInput.Player.Disable();

        GameInput.UI.Disable();
        Debug.Log($"<color=magenta> OnDisable() was called in {this} </color>");
    }


    //---------- Input-Callback Methods, Player-Related ----------
    public void OnMovement(InputAction.CallbackContext context)
    {
        OnMovementInput?.Invoke(context.ReadValue<Vector2>());
        Debug.Log($"<color=magenta> 'OnMovement()' was called in '{this}', ergo 'OnMovementInput'-event should have been fired. </color>");
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

    public void OnSprinting(InputAction.CallbackContext context)
    {
        bool isSpaceHeld = context.ReadValue<float>() > 0.1f;

        if (isSpaceHeld)
            OnFastMovementInput?.Invoke(Enum_Lib.ESpaceKey.Pressed);
        else
            OnFastMovementInput?.Invoke(Enum_Lib.ESpaceKey.NotPressed);
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

    public void OnWeaponSwap(InputAction.CallbackContext context)
    {
        if (context.started)
            OnWeaponSwitch?.Invoke();
    }

    //---------- Input-Callback Methods, UI-related ----------
    public void OnTogglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnEscPress?.Invoke();
            Debug.Log($"<color=orange> 'Esc'-Key was pressed </color>");
        }
    }

    public void OnFirstWeaponEquip(InputAction.CallbackContext context)
    {
        if (context.started)
            OnPrimarytWeaponEquip?.Invoke();
    }

    public void OnSecondWeaponEquip(InputAction.CallbackContext context)
    {
        if (context.started)
            OnSecondaryWeaponEquip?.Invoke();
    }

    public void OnHolsterWeapons(InputAction.CallbackContext context)
    {
        if (context.started)
            OnHolsteringWeapons?.Invoke();
    }
}
