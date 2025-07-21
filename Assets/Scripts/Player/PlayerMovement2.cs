using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    // wall jump goes up
    // remove slide cooldown
    // slide automatic get up leaves until you slow down more
    // stamina come back
    // stop dash while sliding, or slide while dash. also can stop slide if jump in air

    // you're dead
    // test weapons shooting enemies

    private Rigidbody rb;
    private InputManager inputM;

    [Header("Basic stats")]
    [SerializeField] private float speed;
    private float acceleratingForce = 10f;
    [SerializeField] private float wallFallSlowStrength;
    [SerializeField] private float fastFallStrength;

    [Header("Jumping")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private float wallJumpSideStrength;
    [SerializeField] private float wallJumpUpStrength;
    [SerializeField] private float specialJumpStaminaCost;

    [Header("Dash")]
    [SerializeField] private float dashStaminaCost;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashPeriod;
    private float dashStopPeriod = 0.06f;
    private float dashAcceleratingForce = 100f;
    private float dashDecceleratingForce = 60f;

    [Header("Sliding")]
    [SerializeField] private float slideStaminaDrainRate;
    [SerializeField] private float slideStartSpeed;
    [SerializeField] private float slideStartPeriod;
    [SerializeField] private float slideFadePeriod;
    private float slideMinimumAngle = 5;
    private float slideAcceleratingForce = 50f;
    private float slideDownMaxForce = 30f;

    [Header("Layers")]
    [SerializeField] private LayerMask surfaceLayerMask;

    [Header("References")]

    [SerializeField] private Transform angleRayFrontPos;
    [SerializeField] private Transform angleRayRearPos;
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Collider slideCollider;

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
    private Vector3 slideStartDirection;
    private Vector3 slideSlopeDownDirection;
    private bool sliding = false;

    public event EventHandler OnSlideStart;
    public event EventHandler OnSlideEnd;



    private void Awake() {
        rb = GetComponent<Rigidbody>();
        slideTimer = (slideStartPeriod + slideFadePeriod) * 2f;
    }

    private void Start() {
        inputM = InputManager.Instance;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputM.OnJumpButtonPressed += InputManager_OnJumpButtonPressed;
        inputM.OnDashPressed += InputManager_OnDashPressed;
        inputM.OnSlidePressed += InputManager_OnSlidePressed;
        inputM.OnSlideReleased += InputManager_OnSlideReleased;
    }

    private void Update() {
        CheckSurfaceAngle();
    }

    private void FixedUpdate() {
        if (PlayerLogicManager.Instance.IsDead()) {
            return;
        }

        if (dashing) {
            HandleDash();
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= dashPeriod + dashStopPeriod) {
                FinishDash();
            } else if (dashTimer >= dashPeriod) {
                dashDirection = dashDirection * 1.5f * (speed / dashSpeed);
                dashForce = dashDecceleratingForce;
            }

        } else if (sliding) {
            if (PlayerLogicManager.Instance.GetCurrentStamina() < slideStaminaDrainRate * Time.fixedDeltaTime) {
                FinishSliding();
                return;
            }

            bool slid = false;
            if (slideTimer < (slideStartPeriod + slideFadePeriod) && inputM.IsWButtonHeld()) {
                slideTimer += Time.fixedDeltaTime;
                HandleSlideStart();
                slid = true;
            } else if (surfaceAngle < slideMinimumAngle && (!inputM.IsWButtonHeld() || rb.linearVelocity.magnitude < speed) ) {
                FinishSliding();
            }


            if (slideTimer > slideStartPeriod && isOnGround && surfaceAngle >= slideMinimumAngle) {
                HandleSlide();
                slid = true;
            }
            if (slid) {
                PlayerLogicManager.Instance.DrainStamina(slideStaminaDrainRate * Time.fixedDeltaTime);
            }

        } else if (isOnWall && !isOnGround && inputM.IsWButtonHeld()) {
            SlowFallAtWall();

        } else {
            if (!isOnGround && !sliding && inputM.IsSlideButtonHeld()) {
                FastFallInAir();
            }

            HandleMovement();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (IsSurface(collision.gameObject.layer)) {
            Vector3 normal = collision.GetContact(0).normal;
            float range = 0.1f;
            bool isWall = -range < normal.y && normal.y < range;
            if (isWall) {
                isOnWall = true;
                wallGO = collision.gameObject;
                wallNormal = normal;

                if (sliding) {
                    FinishSliding();
                }
            }

            range = 0.3f;
            bool isGround = normal.y > range || normal.y < -range;
            if (isGround) {
                groundGO = collision.gameObject;
                if (!isOnGround) {
                    airJumpCounter = 0;
                    isOnGround = true;
                    if (inputM.IsSlideButtonHeld()) {
                        ContinueSlideFromAir();
                    }
                }
            }

            if (dashing) {
                FinishDash();
                rb.linearVelocity = Vector3.zero;
            }
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



    private void InputManager_OnJumpButtonPressed(object sender, EventArgs e) {
        if (dashing) {
            return;
        }

        int maxAirJumps = 1;
        if (isOnGround) {
            if (ShouldCancelSlide()) {
                FinishSliding();
            }
            Jump();
        } else if (isOnWall) {
            WallJump();
        } else if (airJumpCounter < maxAirJumps) {
            if (ShouldCancelSlide()) {
                FinishSliding();
            }
            AirJump();
        }
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
    }

    private void WallJump() {
        if (PlayerLogicManager.Instance.GetCurrentStamina() < specialJumpStaminaCost) {
            return;
        }

        PlayerLogicManager.Instance.DrainStamina(specialJumpStaminaCost);
        if (rb.linearVelocity.y < 0) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }

        Vector3 wallJumpForce = wallNormal * wallJumpSideStrength + Vector3.up * wallJumpUpStrength;
        rb.AddForce(wallJumpForce, ForceMode.Impulse);
    }


    private void AirJump() {
        if (PlayerLogicManager.Instance.GetCurrentStamina() < specialJumpStaminaCost) {
            return;
        }

        PlayerLogicManager.Instance.DrainStamina(specialJumpStaminaCost);
        airJumpCounter++;
        if (rb.linearVelocity.y < 0) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }

        Vector3 forceVector = Vector3.up * jumpStrength;
        rb.AddForce(forceVector, ForceMode.Impulse);
    }

    private bool IsSurface(int layer) {
        return surfaceLayerMask == (surfaceLayerMask | 1 << layer);
    }



    private void HandleMovement() {
        Vector3 inputVector = inputM.GetMovementVector3Normalised();
        inputVector = GetRealSpaceVectorNormalised(inputVector);
        Vector3 moveVector = GetParallelToSurfaceNormalised(inputVector, surfaceNormal);

        bool moveIntoWall = Vector3.Angle(wallNormal, moveVector) > 90;
        if (isOnWall && moveIntoWall && !VectorsAreSimiliar(inputVector, -wallNormal, 10)) {
            Vector3 parallel = Vector3.Cross(surfaceNormal, wallNormal).normalized;
            if (Vector3.Dot(parallel, moveVector) < 0) {
                parallel = -parallel;
            }
            moveVector = parallel.normalized;
        }

        AddForceUntilTargetVelocity(moveVector * speed, acceleratingForce);
    }



    private void InputManager_OnDashPressed(object sender, System.EventArgs e) {
        BeginDash();
    }

    private void BeginDash() {
        if (!dashing && !sliding && PlayerLogicManager.Instance.GetCurrentStamina() >= dashStaminaCost) {
            PlayerLogicManager.Instance.DrainStamina(dashStaminaCost);
            PlayerLogicManager.Instance.SetInvulnerability(true);
            dashing = true;

            dashForce = dashAcceleratingForce;
            dashTimer = 0f;
            dashDirection = inputM.GetMovementVector3Normalised();

            if (dashDirection != Vector3.zero && dashDirection != Vector3.forward) {
                dashDirection = GetRealSpaceVectorNormalised(dashDirection);
                dashDirection = GetParallelToSurfaceNormalised(dashDirection, surfaceNormal);
            } else {
                dashDirection = GetLookingVector3();

                Vector3 potentialDirection = GetParallelToSurfaceNormalised(dashDirection, surfaceNormal);
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
        BeginSliding(0f);
    }

    private void InputManager_OnSlideReleased(object sender, System.EventArgs e) {
        FinishSliding();
    }

    private void ContinueSlideFromAir() {
        float time = slideTimer;
        if (!sliding) {
            time = slideStartPeriod;
        }

        BeginSliding(time);
    }

    private bool ShouldCancelSlide() {
        if (sliding) {
            bool lookingInSlideDirection = VectorsAreSimiliar(rb.linearVelocity, GetLookingVector3(), 30);
            bool moveInputIntoSlide = inputM.IsWButtonHeld() || inputM.GetMovementVector3Normalised() == Vector3.zero;

            return !(lookingInSlideDirection && moveInputIntoSlide);
        }
        return false;
    }

    private void BeginSliding(float timerStart) {
        if (isOnGround && !dashing && inputM.GetMovementVector3Normalised() == Vector3.forward && PlayerLogicManager.Instance.GetCurrentStamina() >= slideStaminaDrainRate) {
            slideTimer = timerStart;
            slideStartDirection = GetParallelToSurfaceNormalised(GetLookingVector3(), surfaceNormal);
            sliding = true;
            mainCollider.enabled = false;
            slideCollider.enabled = true;
            OnSlideStart?.Invoke(this, new EventArgs());
        }
    }

    private void HandleSlideStart() {
        AddForceUntilTargetVelocity(slideStartDirection * slideStartSpeed, slideAcceleratingForce, false);
    }

    private void HandleSlide() {
        float force = 0;
        if (isOnGround) {
            Vector3 temp = Vector3.Cross(surfaceNormal, Vector3.up);
            Vector3 downSlopeVector = Vector3.Cross(surfaceNormal, temp);
            slideSlopeDownDirection = downSlopeVector.normalized;

            force = slideDownMaxForce * Mathf.Sqrt(surfaceAngle / 45);
        } else {
            force = slideDownMaxForce * Mathf.Sqrt(slideMinimumAngle / 45);
        }

        rb.AddForce(slideSlopeDownDirection * force, ForceMode.Force);

    }

    private void FinishSliding() {
        sliding = false;
        slideCollider.enabled = false;
        mainCollider.enabled = true;
        OnSlideEnd?.Invoke(this, new EventArgs());
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
        if (rb.linearVelocity.y < 0) {
            rb.AddForce(Vector3.up * wallFallSlowStrength, ForceMode.Force);
        }
    }

    private void FastFallInAir() {
        rb.AddForce(Vector3.down * fastFallStrength, ForceMode.Force);
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

    private Vector3 GetRealSpaceVectorNormalised(Vector3 vector) {
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

    private Vector3 GetParallelToSurfaceNormalised(Vector3 originalVector, Vector3 surfaceNorm, bool viaY = true) {
        float dotProduct = Vector3.Dot(originalVector, surfaceNorm);
        float requiredChangeInY = -dotProduct / surfaceNorm.y;

        Vector3 newDirection = new Vector3(originalVector.x, originalVector.y + requiredChangeInY, originalVector.z);
        return newDirection.normalized;
    }

    private bool VectorsAreSimiliar(Vector3 vector1, Vector3 vector2, float angleRange) {
        float angle = Vector3.Angle(vector1, vector2);
        return angle < angleRange || angle > (360 - angleRange);
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

    public Vector3 GetSlideStartDirection() {
        return slideStartDirection;
    }

    public bool IsSliding() {
        return sliding;
    }

    public bool IsDashing() {
        return dashing;
    }
}
