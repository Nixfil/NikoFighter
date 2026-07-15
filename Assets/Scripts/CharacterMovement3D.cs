using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class CharacterMovement3D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [Header("Dash")]
    [SerializeField] private float dashDistance = 4f;
    [SerializeField] private float dashSpeedMultiplier = 3f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing;
    private Vector3 dashDirection;
    private DashVFX dashVFX;

    private float dashRemainingDistance;

    private float dashCooldownTimer;

    private float DashSpeed => moveSpeed * dashSpeedMultiplier;

    private CharacterController controller;
    private IInputProvider inputProvider;
    private Vector3 verticalVelocity;
    private bool isGrounded;

    [field: SerializeField] public CharacterState CurrentState { get;  private set; }

    public bool IsGrounded => isGrounded;
    public bool IsDashing => isDashing;
    public Vector3 DashDirection => dashDirection;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputProvider = GetComponent<IInputProvider>();
        dashVFX = GetComponent<DashVFX>();

        if (inputProvider == null)
        {
            Debug.LogError($"No IInputProvider found on {gameObject.name}!");
        }
    }

    private void Update()
    {
        if (inputProvider == null)
            return;

        // Update dash cooldown
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        //==============================
        // Ground Check
        //==============================

        isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        //==============================
        // Read Movement Input
        //==============================

        Vector3 moveDirection = new Vector3(
            inputProvider.MoveInput.x,
            0f,
            inputProvider.MoveInput.y);

        //==============================
        // Start Dash
        //==============================

        if (!isDashing &&
     dashCooldownTimer <= 0f &&
     inputProvider.DashTriggered &&
     moveDirection.sqrMagnitude > 0.01f)
        {
            StartDash(moveDirection);
        }

        //==============================
        // Horizontal Movement
        //==============================

        Vector3 horizontalMovement = Vector3.zero;

        if (isDashing)
        {
            float step = DashSpeed * Time.deltaTime;

            if (step > dashRemainingDistance)
                step = dashRemainingDistance;

            horizontalMovement = dashDirection * step;

            dashRemainingDistance -= step;

            if (dashRemainingDistance <= 0f)
            {
                EndDash();
            }
        }
        else if (moveDirection.sqrMagnitude > 0.01f)
        {
            moveDirection.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);

            horizontalMovement = moveDirection * moveSpeed * Time.deltaTime;
        }

        //==============================
        // Jump
        //==============================

        if (inputProvider.JumpTriggered && isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

           /* if (inputProvider is AIInputProvider aiInput)
            {
                aiInput.ResetTriggers();
            }*/
        }

        //==============================
        // Gravity
        //==============================

        verticalVelocity.y += gravity * Time.deltaTime;

        //==============================
        // Final Movement
        //==============================

        Vector3 finalMovement = horizontalMovement;
        finalMovement += verticalVelocity * Time.deltaTime;

        UpdateCharacterState();

        controller.Move(finalMovement);
    }
    private void StartDash(Vector3 direction)
    {
        dashDirection = direction.normalized;
        dashRemainingDistance = dashDistance;
        isDashing = true;

        dashCooldownTimer = dashCooldown;

        // Face the dash direction immediately
        transform.rotation = Quaternion.LookRotation(dashDirection);

        CharacterState dashState = isGrounded
    ? CharacterState.DashingGround
    : CharacterState.DashingAir;

        dashVFX?.DashStarted(
    dashState,
    dashDirection);
    }

    private void EndDash()
    {
        isDashing = false;

        dashVFX?.DashEnded();
    }

    private void UpdateCharacterState()
    {
        if (isDashing)
        {
            CurrentState = isGrounded
                ? CharacterState.DashingGround
                : CharacterState.DashingAir;
        }
        else
        {
            CurrentState = isGrounded
                ? CharacterState.Grounded
                : CharacterState.Airborne;
        }
    }
}