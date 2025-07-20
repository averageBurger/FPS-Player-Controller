using System.Collections;
using System.Linq;
using UnityEngine;

// I have: dash bunny hop main sympton fixed. adjusted gravity to be stronger, changed jump force slightly and made double jump feel better. changed ground detection. let player jump off walls

// let player move on edges
// add stamina cost to slide
// cooldowns?

public class PlayerMovement2 : MonoBehaviour
{
    private Rigidbody rb;

    [Header("stats")]
    [SerializeField] private float speed;
    private float acceleratingForce = 10f;

    [SerializeField] private float jumpStrength;
    [SerializeField] private float wallFallSlowStrength;
    [SerializeField] private float wallJumpStrength;

    [SerializeField] private float dashStaminaCost;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashPeriod;
    private float dashStopPeriod = 0.06f;
    private float dashAcceleratingForce = 100f;
    private float dashDecceleratingForce = 60f;

    [SerializeField] private float slideStaminaDrainRate;
    [SerializeField] private float slideStartSpeed;
    [SerializeField] private float slideStartPeriod;
    [SerializeField] private float slideFadePeriod;
    private float slideMinimumAngle = 10;
    private float slideAcceleratingForce = 50f;
    private float slideDownMaxForce = 20f;

    [Header("Layers")]
    [SerializeField] private LayerMask surfaceLayerMask;

    [Header("References")]

    [SerializeField] private Transform angleRayFrontPos;
    [SerializeField] private Transform angleRayRearPos;
    [SerializeField] private Transform lowerBodySphereCentre;

    private int airJumpCounter = 0;

    private bool isOnGround = true;
    private GameObject groundGO;
    private bool isOnWall = false;
    private GameObject wallGO;
    private Vector3 wallNormal;

    private bool dashing = false;
    private Vector3 dashDirection;
    private float dashTimer = 0f;
    private float dashForce;

    private Vector3 surfaceNormal;
    private float surfaceAngle = 0f;

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
            if (slideTimer > slideStartPeriod && isOnGround && surfaceAngle >= slideMinimumAngle) {
                HandleSlide();
            }

        } else if (isOnWall && !isOnGround && InputManager.Instance.GetMovementVector3Normalised() == Vector3.forward) {
            SlowFallAtWall();

        } else {
            HandleMovement();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (IsSurface(collision.gameObject.layer)) {
            Vector3 normal = collision.GetContact(0).normal;
            float range = 0.2f;
            if (-range < normal.y && normal.y < range) {
                isOnWall = true;
                wallGO = collision.gameObject;
                wallNormal = normal;
            }

            if (normal.y > (1-range) || normal.y < (range-1)) {
                airJumpCounter = 0;
                isOnGround = true;
                groundGO = collision.gameObject;
            }
        }

        if (dashing) {
            FinishDash();
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject == groundGO) {
            isOnGround = false;
            groundGO = null;
        }

        if (collision.gameObject == wallGO) {
            isOnWall = false;
            wallGO = null;
            wallNormal = Vector3.zero;
        }
    }



    private void InputManager_OnJumpButtonPressed(object sender, System.EventArgs e) {
        if (dashing || InputManager.Instance.IsSlideButtonHeld()) {
            return;
        }

        int maxAirJumps = 1;
        if (isOnGround) {
            Jump();
        } else if (isOnWall) {
            WallJump();
        } else if (airJumpCounter < maxAirJumps) {
            AirJump();
        }
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
    }

    private void WallJump() {
        if (rb.linearVelocity.y < 0) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }

        rb.AddForce(wallNormal * wallJumpStrength, ForceMode.Impulse);
    }


    private void AirJump() {
        airJumpCounter++;
        Vector3 forceVector = Vector3.up * jumpStrength;
        if (rb.linearVelocity.y < 0) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }
        rb.AddForce(forceVector, ForceMode.Impulse);
    }

    private bool IsSurface(int layer) {
        return surfaceLayerMask == (surfaceLayerMask | 1 << layer);
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
                dashDirection = GetVectorParallelToSurface(dashDirection, surfaceNormal);
            } else {
                dashDirection = GetLookingVector3();

                Vector3 potentialDirection = GetVectorParallelToSurface(dashDirection, surfaceNormal);
                if (isOnGround && potentialDirection.y > dashDirection.y) {
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
            slideDirection = GetVectorParallelToSurface(GetLookingVector3(), surfaceNormal);
        }
    }

    private void HandleSlideStart() {
        AddForceUntilTargetVelocity(slideDirection * slideStartSpeed, slideAcceleratingForce, false);
    }

    private void HandleSlide() {
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

        bool angleFound = false;
        if (Physics.Raycast(angleRayRearPos.position, angleRayRearPos.TransformDirection(-Vector3.up), out RaycastHit rearHit, Mathf.Infinity, surfaceLayerMask)) {
            //Debug.DrawRay(angleRayRearPos.position, angleRayRearPos.TransformDirection(-Vector3.up) * rearHit.distance, Color.yellow);
            surfaceAngle = Vector3.Angle(rearHit.normal, Vector3.up);
            surfaceNormal = rearHit.normal;
            angleFound = true;
            //Debug.Log("surface angle: " + surfaceAngle);
        }

        Vector3 frontRayStartPos = new Vector3(angleRayFrontPos.position.x, angleRayRearPos.position.y, angleRayFrontPos.position.z);
        if (Physics.Raycast(frontRayStartPos, angleRayFrontPos.TransformDirection(-Vector3.up), out RaycastHit frontHit, Mathf.Infinity, surfaceLayerMask)) {
            //Debug.DrawRay(frontRayStartPos, angleRayFrontPos.TransformDirection(-Vector3.up) * frontHit.distance, Color.yellow);
            if (!angleFound) {
                surfaceAngle = Vector3.Angle(frontHit.normal, Vector3.up);
                surfaceNormal = frontHit.normal;
                angleFound = true;
            }
        }
    }

    private void SlowFallAtWall() {
        rb.AddForce(Vector3.up * wallFallSlowStrength, ForceMode.Force);
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

    private Vector3 GetVectorParallelToSurface(Vector3 originalVector, Vector3 surfaceNorm) {
        float dotProduct = Vector3.Dot(originalVector, surfaceNorm);
        float requiredChangeInY = -dotProduct / surfaceNorm.y;

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
