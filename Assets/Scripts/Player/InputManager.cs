using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private InputSystem_Actions inputActions;

    public event EventHandler OnRangedAttackButtonPressed;
    public event EventHandler OnMeleeAttackButtonPressed;
    public event EventHandler OnReloadButtonPressed;

    private void Awake() {
        Instance = this;

        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        inputActions.Player.RangedAttack.performed += RangedAttack_performed;
        inputActions.Player.MeleeAttack.performed += MeleeAttack_performed;
        inputActions.Player.Reload.performed += Reload_performed;
    }

    private void Reload_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnReloadButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void RangedAttack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnRangedAttackButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void MeleeAttack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMeleeAttackButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalised() {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
