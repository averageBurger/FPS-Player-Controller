using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

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

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 inputVector =  context.ReadValue<Vector2>();
        Debug.Log(inputVector);
        float speed = 30f;
        rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }

}
