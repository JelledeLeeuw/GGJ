using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement variables")]
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float sprintMultiplier;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float maxVelocity;

    private Vector3 currentMovement;

    private InputHandler inputHandler;

    private CharacterController characterController;

    private Rigidbody rb;

    private void Start()
    {
        // characterController = gameObject.GetComponent<CharacterController>();
        inputHandler = FindFirstObjectByType<InputHandler>();
        Camera.main.GetComponent<SetCameraTarget>().AssignTarget(transform);
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        print($"is owner: {IsOwner}");
        if (!IsOwner)
            return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Movement input from player
        print(inputHandler.moveInput.x);
        Vector3 movementInput = new Vector3(inputHandler.moveInput.x, 0, inputHandler.moveInput.y);
        movementInput.Normalize();

        // Get the forward and right position of the camera while setting the y to 0 since we dont want to rotate on the y-axis
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Calculate the direction based on where the player is going and looking
        Vector3 worldDirection = cameraRight * movementInput.x + cameraForward * movementInput.z;

        // Apply it to a class variable so we can use it in rotation
        currentMovement.x = worldDirection.x * walkSpeed;
        currentMovement.z = worldDirection.z * walkSpeed;

        rb.AddForce(currentMovement * Time.deltaTime, ForceMode.Impulse);
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxVelocity)
        {
            Vector3 clampedVelocity = horizontalVelocity.normalized * maxVelocity;
            rb.linearVelocity = new Vector3(
                clampedVelocity.x,
                rb.linearVelocity.y,
                clampedVelocity.z
            );
        }

        //currentMovement += Physics.gravity;

        HandleRotation(worldDirection, cameraForward);
    }

    private void HandleRotation(Vector3 worldDirection, Vector3 cameraForward)
    {
        if (worldDirection.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation( /*inputHandler.aimTriggered ? cameraForward :*/
                worldDirection
            );
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
