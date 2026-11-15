using System.Runtime.CompilerServices;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }
    private InputSystem_Actions inputActions;

    
    [Header("Player Movement")]
    public Vector2 movementInput;
    public float horizontalInput;

    public bool jumpInput;
    public bool dashHeld;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();
            //Moving
            inputActions.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += i => movementInput = Vector2.zero;
            //Jumping
            inputActions.Player.Jump.performed += i => jumpInput = true;
            inputActions.Player.Jump.canceled += i => jumpInput = false;
            //Dashing
            inputActions.Player.Dash.performed += i => dashHeld = true;
            inputActions.Player.Dash.canceled += i => dashHeld = false;

            inputActions.Enable();
        }
    }

    void HandleMovementInput()
    {
        horizontalInput = movementInput.x;
    }

    void Update()
    {
        HandleMovementInput();
    }



}
