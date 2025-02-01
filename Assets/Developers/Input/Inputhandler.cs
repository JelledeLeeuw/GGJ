using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("InputSystem")]
    [SerializeField] private Controls playerControls;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction shootAction;
    public Vector2 moveInput { get; private set; }

    public bool jumpTriggered { get; private set; }

    public bool shootTriggered { get; private set; }


    public static InputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        playerControls = new Controls();
        playerControls.Enable();
        moveAction = playerControls.Player.Move;
        shootAction = playerControls.Player.Shoot;
        jumpAction = playerControls.Player.Jump;
        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;
        shootAction.performed += context => shootTriggered = true;
        shootAction.canceled += context => shootTriggered = false;
        jumpAction.performed += context => jumpTriggered = true;
        jumpAction.canceled += context => jumpTriggered = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        shootAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        shootAction.Disable();
    }
}
