using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    Vector2 inputVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.phase);
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    //Checks the input put in
    public void Move(InputAction.CallbackContext context)
    {
        inputVector =  context.ReadValue<Vector2>();
        Debug.Log(inputVector);
    }

    // Applies the force into the direction decided by the input
    private void FixedUpdate()
    {
        float speed = 30f;
        rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }

    /*private void HandleMovement() {
        Vector2 moveInput = InputManager.Instance.GetMovementVectorNormalised();
        Vector3 moveVector = new Vector3(moveInput.x, 0, moveInput.y);
        float speed = 5f;

        rb.linearVelocity = moveVector * speed;
    }*/
}
