using UnityEngine;
using UnityEngine.Accessibility;

public class PlayerMovement2 : MonoBehaviour
{
    private Rigidbody rb;

    [Header("stats")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleratingForce;
    [SerializeField] private float jumpStrength;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    private int jumpCounter = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        InputManager.Instance.OnJumpButtonPressed += InputManager_OnJumpButtonPressed;
    }

    private void InputManager_OnJumpButtonPressed(object sender, System.EventArgs e) {
        if (jumpCounter < 2) {
            jumpCounter++;
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (IsGroundLayer(collision.gameObject.layer)) {
            jumpCounter = 0;
        }
    }

    private bool IsGroundLayer(int layer) {
        return 1 == groundLayer.value >> layer;
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 moveInput = InputManager.Instance.GetMovementVectorNormalised();
        Vector3 moveVector = new Vector3(moveInput.x, 0, moveInput.y);

        if (acceleratingForce == 0 || moveVector.magnitude == 0) {
            return;
        }
        Debug.Log("move");

        Vector3 velocity = moveVector * speed + moveVector * 0.2f * rb.linearDamping;
        float force = Mathf.Clamp(acceleratingForce, -rb.mass / Time.fixedDeltaTime, rb.mass / Time.fixedDeltaTime);

        if (rb.linearVelocity.magnitude == 0) {
            rb.AddForce(velocity * force, ForceMode.Force);
        } else {
            float velocityProjectedToTarget = Vector3.Dot(velocity, rb.linearVelocity) / velocity.magnitude;
            rb.AddForce(velocity.normalized * (velocity.magnitude - velocityProjectedToTarget) * force, ForceMode.Force);
        }
    }
}
