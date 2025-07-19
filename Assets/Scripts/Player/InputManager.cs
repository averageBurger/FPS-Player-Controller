using System;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private InputSystem_Actions inputActions;

    public event EventHandler OnRangedAttackButtonPressed;
    public event EventHandler OnMeleeAttackButtonPressed;
    public event EventHandler OnReloadButtonPressed;

    public event EventHandler OnJumpButtonPressed;
    public event EventHandler OnDashPressed;

    private void Awake() {
        Instance = this;

        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        inputActions.Player.RangedAttack.performed += RangedAttack_performed;
        inputActions.Player.MeleeAttack.performed += MeleeAttack_performed;
        inputActions.Player.Reload.performed += Reload_performed;

        inputActions.Player.Jump.performed += Jump_performed;
        inputActions.Player.Dash.performed += Dash_performed;
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnDashPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJumpButtonPressed?.Invoke(this, EventArgs.Empty);
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

    public Vector3 GetMovementVector3Normalised() {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        Vector3 moveVector = new Vector3(inputVector.x, 0, inputVector.y);

        return moveVector;
    }

    public Vector2 GetLookRotationVector2Normalised() {
        Vector2 inputVector = inputActions.Player.Look.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public bool IsRangedAttackButtonHeld() {
        return inputActions.Player.RangedAttack.IsPressed();
    }
}
