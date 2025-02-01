using System.Collections;
using Developers.Scripts;
using Unity.Netcode;
using Unity.Netcode.Components;
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

    private NetworkRigidbody rb;

    private Camera mainCamera; // caches the main camera

    private bool _isJumping;

    private bool _isGrounded;

    private void Start()
    {
        // characterController = gameObject.GetComponent<CharacterController>();
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        //Cursor.lockState = CursorLockMode.Locked;
        inputHandler = FindFirstObjectByType<InputHandler>();
        mainCamera = Camera.main;
        mainCamera.GetComponent<SetCameraTarget>().AssignTarget(transform);

        rb = gameObject.GetComponent<NetworkRigidbody>();
        GameManager.Instance.AddPlayer(this);
    }

    [Rpc(SendTo.Authority)]
    public void GameStartRpc()
    {
        GameManager.Instance.PlayerReady();
    }

    private void FixedUpdate()
    {
        if (!IsOwner || GameManager.Instance.gameState != GameState.Playing)
            return;
        HandleMovement();
        Jump();
    }

    private void HandleMovement()
    {
        // Movement input from player
        Vector3 movementInput = new Vector3(inputHandler.moveInput.x, 0, inputHandler.moveInput.y);
        movementInput.Normalize();

        // Get the forward and right position of the camera while setting the y to 0 since we dont want to rotate on the y-axis
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Calculate the direction based on where the player is going and looking
        Vector3 worldDirection = cameraRight * movementInput.x + cameraForward * movementInput.z;

        // Apply it to a class variable so we can use it in rotation
        currentMovement.x = worldDirection.x * walkSpeed;
        currentMovement.z = worldDirection.z * walkSpeed;

        rb.Rigidbody.AddForce(currentMovement * Time.deltaTime, ForceMode.Impulse);
        Vector3 horizontalVelocity = new Vector3(rb.Rigidbody.linearVelocity.x, 0, rb.Rigidbody.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxVelocity && _isJumping == false)
        {
            Vector3 clampedVelocity = horizontalVelocity.normalized * maxVelocity;
            rb.Rigidbody.linearVelocity = new Vector3(
                clampedVelocity.x,
                rb.Rigidbody.linearVelocity.y,
                clampedVelocity.z
            );
        }

        HandleRotation(worldDirection, cameraForward);
    }

    private void Jump()
    {
        if(inputHandler.jumpTriggered == true && _isJumping == false && _isGrounded == true)
        {
            Debug.Log("yess");
            StartCoroutine(Jumping());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

    private void HandleRotation(Vector3 worldDirection, Vector3 cameraForward)
    {
        if (worldDirection.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(worldDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private IEnumerator Jumping()
    {
        _isJumping = true;
        rb.Rigidbody.AddForce(0,20,0,ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        _isJumping = false;
    }
}
