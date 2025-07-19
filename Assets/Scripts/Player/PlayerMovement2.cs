using Unity.Mathematics;
using UnityEngine;

// I have: let player look around. Dash goes to look direction if no input. Dash slows down faster now. air jumps counter + isOnGround rather than jumps counter. // make air jumps feel nicer

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
    private float dashStopPeriod = 0.05f;
    private float dashAcceleratingForce = 100f;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    private bool isOnGround = true;
    private int airJumpCounter = 0;

    private bool dashing = false;
    private Vector3 dashDirection;
    private float dashTimer = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputManager.Instance.OnJumpButtonPressed += InputManager_OnJumpButtonPressed;
        InputManager.Instance.OnDashPressed += InputManager_OnDashPressed;
    }

    private void InputManager_OnDashPressed(object sender, System.EventArgs e) {
        if (!dashing && PlayerLogicManager.Instance.GetCurrentStamina() >= dashStaminaCost) {
            PlayerLogicManager.Instance.DrainStamina(dashStaminaCost);
            PlayerLogicManager.Instance.SetInvulnerability(true);
            dashing = true;

            dashTimer = 0f;
            dashDirection = InputManager.Instance.GetMovementVector3Normalised();
            if (dashDirection != Vector3.zero && dashDirection != Vector3.forward) {
                dashDirection = GetRealSpaceVelocity(dashDirection);
            } else {
                dashDirection = GetLookingVector3();
            }

            if (isOnGround && dashDirection.y < 0) {
                dashDirection.y = 0f;
            }
        }
    }

    private void FinishDash() {
        PlayerLogicManager.Instance.SetInvulnerability(false);
        dashing = false;
    }

    private void InputManager_OnJumpButtonPressed(object sender, System.EventArgs e) {
        if (dashing) {
            return;
        }

        int maxAirJumps = 1;
        if (isOnGround) {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        } else if (airJumpCounter < maxAirJumps) {
            airJumpCounter++;
            float airJumpStrengthMultiplier = 1.5f;
            rb.AddForce(Vector3.up * jumpStrength * airJumpStrengthMultiplier, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (IsGroundLayer(collision.gameObject.layer)) {
            airJumpCounter = 0;
            isOnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (IsGroundLayer(collision.gameObject.layer)) {
            isOnGround = false;
        }
    }

    private bool IsGroundLayer(int layer) {
        return 1 == groundLayer.value >> layer;
    }

    private void FixedUpdate() {
        if (dashing) {
            HandleDash();
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= dashPeriod + dashStopPeriod) {
                FinishDash();
            } else if (dashTimer >= dashPeriod) {
                dashDirection = dashDirection * (speed / dashSpeed);
            }
        } else {
            HandleMovement();
        }
    }

    private void HandleMovement() {
        Vector3 moveVector = InputManager.Instance.GetMovementVector3Normalised();
        moveVector = GetRealSpaceVelocity(moveVector);

        AddForceUntilTargetVelocity(moveVector * speed, acceleratingForce);
    }

    private void HandleDash() {
        AddForceUntilTargetVelocity(dashDirection * dashSpeed, dashAcceleratingForce, false);
    }

    private void AddForceUntilTargetVelocity(Vector3 targetVelocity, float force, bool ignoreYAxis = true) {
        if (force == 0 || (targetVelocity.magnitude == 0 && rb.linearVelocity.magnitude == 0)) {
            return;
        }

        Vector3 velocity = targetVelocity + targetVelocity.normalized * 0.2f * rb.linearDamping;
        force = Mathf.Clamp(force, -rb.mass / Time.fixedDeltaTime, rb.mass / Time.fixedDeltaTime);
        Vector3 currentVelocity = rb.linearVelocity;
        if (ignoreYAxis) {
            currentVelocity.y = 0f;
        }

        if (rb.linearVelocity.magnitude == 0) {
            rb.AddForce(velocity * force, ForceMode.Force);
        } else if (targetVelocity.magnitude == 0) {
            rb.AddForce(-currentVelocity * force, ForceMode.Force);
        } else {
            float velocityProjectedToTargetMagnitude = Vector3.Dot(velocity, currentVelocity) / velocity.magnitude;
            rb.AddForce(velocity.normalized * (velocity.magnitude - velocityProjectedToTargetMagnitude) * force, ForceMode.Force);
        }
    }

    private Vector3 GetRealSpaceVelocity(Vector3 velocity) {
        float dotQuadrantTest = Mathf.Clamp(Vector3.Dot(Vector3.right, GetForwardVector3()), -1, 1);
        float dot = Mathf.Clamp(Vector3.Dot(Vector3.forward, GetForwardVector3()), -1, 1);

        float angleQuadrantTest = Mathf.Acos(dotQuadrantTest) * Mathf.Rad2Deg;
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angleQuadrantTest > 90) {
            angle = 360 - angle;
        }

        return Quaternion.AngleAxis(angle, Vector3.up) * velocity;
    }

    private Vector3 GetForwardVector3() {
        Vector3 cameraVector = WeaponManager.Instance.GetLookingVector3();
        Vector3 playerForwardVector = cameraVector;
        playerForwardVector.y = 0f;
        playerForwardVector = playerForwardVector.normalized;

        return playerForwardVector;
    }

    private Vector3 GetLookingVector3() {
        return WeaponManager.Instance.GetLookingVector3();
    }
}
