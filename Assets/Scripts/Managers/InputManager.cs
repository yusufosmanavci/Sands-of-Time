using Assets.Scripts.OtherScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInput playerInput;

    public bool MenuOpenCloseInput { get; private set; }
    public static bool DoorInteract { get; private set; }
    public static bool DashInput { get; private set; }
    public static bool AttackInput { get; set; }
    public static float HorizontalMoveInput;
    public static bool healInput { get; set; }
    public static bool pauseInput { get; set; }
    
    public InputAction menuOpenCloseAction { get; set; }
    public InputAction doorInteractAction { get; set; }
    public InputAction dashAction { get; set; }
    public InputAction attackAction { get; set; }
    public InputAction horizontalMove { get; set; }
    public InputAction jumpAction { get; set; }
    public InputAction healAction { get; set; }
    public InputAction pauseAction { get; set; }


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
        playerInput = GetComponent<PlayerInput>();
        menuOpenCloseAction = playerInput.actions["MenuOpenClose"];
        doorInteractAction = playerInput.actions["DoorInteraction"];
        dashAction = playerInput.actions["DashInput"];
        attackAction = playerInput.actions["AttackInput"];
        horizontalMove = playerInput.actions["MoveInputs"];
        jumpAction = playerInput.actions["JumpInput"];
        healAction = playerInput.actions["HealInput"];
        pauseAction = playerInput.actions["PauseInput"];
    }

    private void Start()
    {
        jumpAction.performed += PlayerManager.Instance.playerController.Jump; // Set jump flag when jump action is performed
    }

    private void Update()
    {
        MenuOpenCloseInput = menuOpenCloseAction.WasPressedThisFrame();
        DoorInteract = doorInteractAction.WasPressedThisFrame();
        DashInput = dashAction.WasPressedThisFrame();
        AttackInput = attackAction.WasPressedThisFrame();
        HorizontalMoveInput = horizontalMove.ReadValue<Vector2>().x; // Read horizontal movement input
        healInput = healAction.WasPressedThisFrame(); // Read heal input
        pauseInput = pauseAction.WasPressedThisFrame();
    }

    public static void DeactivatePlayerControls()
    {
        InputManager.Instance.doorInteractAction.Disable(); // kapat�r
        InputManager.Instance.dashAction.Disable(); // kapat�r
        InputManager.Instance.attackAction.Disable(); // kapat�r
        InputManager.Instance.horizontalMove.Disable(); // Disable horizontal movement input
        InputManager.Instance.jumpAction.Disable(); // Disable jump input
        InputManager.Instance.healAction.Disable(); // Disable heal input
    }

    public static void ActivatePlayerControls()
    {
        InputManager.Instance.doorInteractAction.Enable(); // kap�y� a�ar
        InputManager.Instance.dashAction.Enable(); // dash'i a�ar
        InputManager.Instance.attackAction.Enable(); // sald�r�y� a�ar
        InputManager.Instance.horizontalMove.Enable(); // Enable horizontal movement input
        InputManager.Instance.jumpAction.Enable(); // Enable jump input
        InputManager.Instance.healAction.Enable(); // Enable heal input
    }


    private void OnDestroy()
    {
        jumpAction.performed -= PlayerManager.Instance.playerController.Jump; // Set jump flag when jump action is performed
    }
}
