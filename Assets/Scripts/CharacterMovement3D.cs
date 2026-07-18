using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement3D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    private bool rotationLocked;
    private ForcedMovement currentForcedMovement;

    private CharacterController controller;
    private IInputProvider inputProvider;

    private Vector3 verticalVelocity;
    private bool isGrounded;

    [field: SerializeField]
    public CharacterState CurrentState { get; private set; }

    public bool IsGrounded => isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputProvider = GetComponent<IInputProvider>();

        if (inputProvider == null)
        {
            Debug.LogError($"No IInputProvider found on {gameObject.name}!");
        }
    }

    private void Update()
    {
        if (inputProvider == null)
            return;

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
        // Horizontal Movement
        //==============================

        Vector3 horizontalMovement = Vector3.zero;

        if (currentForcedMovement != null)
        {
            horizontalMovement = currentForcedMovement.Tick(
                this,
                Time.deltaTime);

            if (currentForcedMovement.Finished)
            {
                currentForcedMovement.End(this);
                currentForcedMovement = null;
            }
        }
        else if (moveDirection.sqrMagnitude > 0.01f)
        {
            moveDirection.Normalize();

            if (!rotationLocked)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime);
            }

            horizontalMovement = moveDirection * moveSpeed * Time.deltaTime;
        }

        //==============================
        // Jump
        //==============================

        if (inputProvider.JumpTriggered && isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
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

    private void UpdateCharacterState()
    {
        if (currentForcedMovement != null)
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

    public void SetRotationLocked(bool locked)
    {
        rotationLocked = locked;
    }

    public void BeginForcedMovement(ForcedMovement movement)
    {
        currentForcedMovement = movement;
        currentForcedMovement.Begin(this);
    }
}