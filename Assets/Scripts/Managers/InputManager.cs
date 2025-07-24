using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInput playerInput;

    public bool MenuOpenCloseInput { get; private set; }
    public static bool DoorInteract {  get; private set; }

    private InputAction menuOpenCloseAction;
    private InputAction doorInteractAction;

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
    }

    private void Update()
    {
        MenuOpenCloseInput = menuOpenCloseAction.WasPressedThisFrame();
        DoorInteract = doorInteractAction.WasPressedThisFrame();
    }

    public static void DeactivatePlayerControls()
    {
        Instance.playerInput.currentActionMap.Disable();
    }

    public static void ActivatePlayerControls()
    {
        Instance.playerInput.currentActionMap.Enable();
    }
}
