using System.Collections;
using UnityEngine;

// I have: made dash stop less harshly. Dash will go parallel to the surface if the surface is tilted. Fixed super jump (still small bunny hop occasionally). Sliding done!

// let player move on edges
// let player jump off walls

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
    private float dashStopPeriod = 0.06f;
    private float dashAcceleratingForce = 100f;
    private float dashDecceleratingForce = 60f;
    private float dashForce;

    [SerializeField] private float slideStaminaDrainRate;
    [SerializeField] private float slideStartSpeed;
    [SerializeField] private float slideStartPeriod;
    [SerializeField] private float slideFadePeriod;
    private float slideMinimumAngle = 10;
    private float slideAcceleratingForce = 50f;
    private float slideDownMaxForce = 20f;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask surfaceLayerMask;

    [Header("References")]

    [SerializeField] private Transform angleRayFrontPos;
    [SerializeField] private Transform angleRayRearPos;


    private bool isOnGround = true;
    private int airJumpCounter = 0;

    private bool dashing = false;
    private Vector3 dashDirection;
    private float dashTimer = 0f;

    private Vector3 surfaceNormal;
    private float surfaceAngle = 0f;
    private bool isFlatSurface = true;

    private float slideTimer;
    private Vector3 slideDirection;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        slideTimer = slideStartPeriod * 2f;
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputManager.Instance.OnJumpButtonPressed += InputManager_OnJumpButtonPressed;
        InputManager.Instance.OnDashPressed += InputManager_OnDashPressed;
        InputManager.Instance.OnSlidePressed += InputManager_OnSlidePressed;
        InputManager.Instance.OnSlideReleased += InputManager_OnSlideReleased;
    }

    private void Update() {
        CheckSurfaceAngle();
    }

    private void FixedUpdate() {
        if (dashing) {
            HandleDash();
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= dashPeriod + dashStopPeriod) {
                FinishDash();
            } else if (dashTimer >= dashPeriod) {
                dashDirection = dashDirection * 1.5f * (speed / dashSpeed);
                dashForce = dashDecceleratingForce;
            }

        } else if (InputManager.Instance.IsSlideButtonHeld()) {
            if (slideTimer < slideStartPeriod + slideFadePeriod) {
                slideTimer += Time.fixedDeltaTime;
                HandleSlideStart();
            }
            if (slideTimer > slideStartPeriod && isOnGround && !isFlatSurface && surfaceAngle >= slideMinimumAngle) {
                HandleSlide();
            }

        } else {
            HandleMovement();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (IsGroundLayer(collision.gameObject.layer)) {
            airJumpCounter = 0;
            isOnGround = true;
        }

        if (dashing) {
            FinishDash();
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (IsGroundLayer(collision.gameObject.layer)) {
            isOnGround = false;
        }
    }


    private void InputManager_OnJumpButtonPressed(object sender, System.EventArgs e) {
        if (dashing || InputManager.Instance.IsSlideButtonHeld()) {
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

    private bool IsGroundLayer(int layer) {
        return 1 == groundLayer.value >> layer;
    }


    private void HandleMovement() {
        Vector3 moveVector = InputManager.Instance.GetMovementVector3Normalised();
        moveVector = GetRealSpaceVector(moveVector);

        AddForceUntilTargetVelocity(moveVector * speed, acceleratingForce);
    }


    private void InputManager_OnDashPressed(object sender, System.EventArgs e) {
        if (!dashing && PlayerLogicManager.Instance.GetCurrentStamina() >= dashStaminaCost) {
            PlayerLogicManager.Instance.DrainStamina(dashStaminaCost);
            PlayerLogicManager.Instance.SetInvulnerability(true);
            dashing = true;

            dashForce = dashAcceleratingForce;
            dashTimer = 0f;
            dashDirection = InputManager.Instance.GetMovementVector3Normalised();

            if (dashDirection != Vector3.zero && dashDirection != Vector3.forward) {
                dashDirection = GetRealSpaceVector(dashDirection);

                if (isOnGround && !isFlatSurface) {
                    dashDirection = SetVectorParallelToSurface(dashDirection);
                }
            } else {
                dashDirection = GetLookingVector3();

                Vector3 potentialDirection = SetVectorParallelToSurface(dashDirection);
                if (potentialDirection.y > dashDirection.y) {
                    dashDirection = potentialDirection;
                }
            }
        }
    }

    private void HandleDash() {
        AddForceUntilTargetVelocity(dashDirection * dashSpeed, dashForce, false);
    }

    private void FinishDash() {
        PlayerLogicManager.Instance.SetInvulnerability(false);
        dashing = false;
    }


    private void InputManager_OnSlidePressed(object sender, System.EventArgs e) {
        if (isOnGround) {
            slideTimer = 0f;
            slideDirection = SetVectorParallelToSurface(GetLookingVector3());
            Debug.Log("slide pressed: " + slideDirection.x + ", " + slideDirection.y + ", " + slideDirection.z);
        }
    }

    private void HandleSlideStart() {
        Debug.Log("start sliding");
        AddForceUntilTargetVelocity(slideDirection * slideStartSpeed, slideAcceleratingForce, false);
    }

    private void HandleSlide() {
        Debug.Log("slide gravity");
        Vector3 temp = Vector3.Cross(surfaceNormal, Vector3.up);
        Vector3 downSlopeVector = Vector3.Cross(surfaceNormal, temp);
        downSlopeVector = downSlopeVector.normalized;

        float force = slideDownMaxForce * (surfaceAngle / 90);

        rb.AddForce(downSlopeVector * force, ForceMode.Force);

    }

    private void InputManager_OnSlideReleased(object sender, System.EventArgs e) {
        
    }

    private void CheckSurfaceAngle() {
        angleRayRearPos.rotation = Quaternion.Euler(-transform.rotation.x, 0, 0);
        angleRayFrontPos.rotation = Quaternion.Euler(-transform.rotation.x, 0, 0);

        if (Physics.Raycast(angleRayRearPos.position, angleRayRearPos.TransformDirection(-Vector3.up), out RaycastHit rearHit, Mathf.Infinity, surfaceLayerMask)) {
            //Debug.DrawRay(angleRayRearPos.position, angleRayRearPos.TransformDirection(-Vector3.up) * rearHit.distance, Color.yellow);
            surfaceAngle = Vector3.Angle(rearHit.normal, Vector3.up);
            surfaceNormal = rearHit.normal;
            //Debug.Log("surface angle: " + surfaceAngle);
        }

        Vector3 frontRayStartPos = new Vector3(angleRayFrontPos.position.x, angleRayRearPos.position.y, angleRayFrontPos.position.z);
        if (Physics.Raycast(frontRayStartPos, angleRayFrontPos.TransformDirection(-Vector3.up), out RaycastHit frontHit, Mathf.Infinity, surfaceLayerMask)) {
            //Debug.DrawRay(frontRayStartPos, angleRayFrontPos.TransformDirection(-Vector3.up) * frontHit.distance, Color.yellow);
        }

        isFlatSurface = false;
        if (frontHit.distance == rearHit.distance) {
            isFlatSurface = true;
        }
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

    private Vector3 GetRealSpaceVector(Vector3 vector) {
        float dotQuadrantTest = Mathf.Clamp(Vector3.Dot(Vector3.right, GetForwardVector3()), -1, 1);
        float dot = Mathf.Clamp(Vector3.Dot(Vector3.forward, GetForwardVector3()), -1, 1);

        float angleQuadrantTest = Mathf.Acos(dotQuadrantTest) * Mathf.Rad2Deg;
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angleQuadrantTest > 90) {
            angle = 360 - angle;
        }

        Vector3 realSpaceVector = Quaternion.AngleAxis(angle, Vector3.up) * vector;
        return realSpaceVector.normalized;
    }

    private Vector3 SetVectorParallelToSurface(Vector3 originalVector) {
        float dotProduct = Vector3.Dot(originalVector, surfaceNormal);
        float requiredChangeInY = -dotProduct / surfaceNormal.y;

        Vector3 newDirection = new Vector3(originalVector.x, originalVector.y + requiredChangeInY, originalVector.z);
        return newDirection.normalized;
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
