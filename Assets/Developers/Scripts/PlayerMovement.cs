using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement variables")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float rotationSpeed;

    private Vector3 currentMovement;

    private InputHandler inputHandler;

    private CharacterController characterController;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = gameObject.GetComponent<CharacterController>();
        inputHandler = FindFirstObjectByType<InputHandler>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Movement input from player
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

        characterController.Move(currentMovement * Time.deltaTime);

        currentMovement += Physics.gravity;

        HandleRotation(worldDirection, cameraForward);
    }

    private void HandleRotation(Vector3 worldDirection, Vector3 cameraForward)
    {
        if (worldDirection.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(/*inputHandler.aimTriggered ? cameraForward :*/ worldDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

