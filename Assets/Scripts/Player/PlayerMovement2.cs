using Unity.Mathematics;
using UnityEngine;

// let player look around
// dash at look direction if no input
// let player move on edges

public class PlayerMovement2 : MonoBehaviour
{
    private Rigidbody rb;

    [Header("stats")]
    [SerializeField] private float speed;
    private float acceleratingForce = 10f;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float dashStaminaCost;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashPeriod;
    private float dashAcceleratingForce = 100f;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    private int jumpCounter = 0;

    private bool dashing = false;
    private Vector3 dashDirection;
    private float dashTimer = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        InputManager.Instance.OnJumpButtonPressed += InputManager_OnJumpButtonPressed;
        InputManager.Instance.OnDashPressed += InputManager_OnDashPressed;
    }

    private void InputManager_OnDashPressed(object sender, System.EventArgs e) {
        if (!dashing && PlayerLogicManager.Instance.GetCurrentStamina() >= dashStaminaCost) {
            PlayerLogicManager.Instance.DrainStamina(dashStaminaCost);
            PlayerLogicManager.Instance.SetInvulnerability(true);
            dashing = true;

            dashTimer = 0f;
            dashDirection = InputManager.Instance.GetMovementVectorNormalised();
        }
    }

    private void FinishDash() {
        PlayerLogicManager.Instance.SetInvulnerability(false);
        dashing = false;
        dashDirection = Vector3.zero;
    }

    private void InputManager_OnJumpButtonPressed(object sender, System.EventArgs e) {
        if (jumpCounter < 2 && !dashing) {
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
        if (dashing) {
            HandleDash();
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= dashPeriod) {
                FinishDash();
            }
        } else {
            HandleMovement();
        }
    }

    private void HandleMovement() {
        Vector3 moveVector = InputManager.Instance.GetMovementVectorNormalised();

        AddForceUntilTargetVelocity(moveVector * speed, acceleratingForce);
    }

    private void HandleDash() {
        AddForceUntilTargetVelocity(dashDirection * dashSpeed, dashAcceleratingForce);
    }

    private void AddForceUntilTargetVelocity(Vector3 targetVelocity, float force) {
        if (force == 0 || targetVelocity.magnitude == 0) {
            return;
        }

        Vector3 velocity = targetVelocity + targetVelocity.normalized * 0.2f * rb.linearDamping;
        force = Mathf.Clamp(force, -rb.mass / Time.fixedDeltaTime, rb.mass / Time.fixedDeltaTime);

        if (rb.linearVelocity.magnitude == 0) {
            rb.AddRelativeForce(velocity * force, ForceMode.Force);
        } else {
            float dotQuadrantTest = Mathf.Clamp(Vector3.Dot(Vector3.right, transform.forward), -1, 1);
            float dot = Mathf.Clamp(Vector3.Dot(Vector3.forward, transform.forward), -1, 1);

            float angleQuadrantTest = Mathf.Acos(dotQuadrantTest) * Mathf.Rad2Deg;
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angleQuadrantTest > 90) {
                angle = 360 - angle;
            }

            Vector3 realSpaceVelocity = Quaternion.AngleAxis(angle, Vector3.up) * velocity;

            float velocityProjectedToTargetMagnitude = Vector3.Dot(realSpaceVelocity, rb.linearVelocity) / velocity.magnitude;
            rb.AddRelativeForce(velocity.normalized * (velocity.magnitude - velocityProjectedToTargetMagnitude) * force, ForceMode.Force);
        }
    }
}
