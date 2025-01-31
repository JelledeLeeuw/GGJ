using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("InputSystem")]
    [SerializeField] private Controls playerControls;

    private InputAction moveAction;
    //private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction attackAction;
    public Vector2 moveInput { get; private set; }

    public float sprintValue { get; private set; }

    public bool jumpTriggered { get; private set; }

    public bool attackTriggered { get; private set; }

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
        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }
}
