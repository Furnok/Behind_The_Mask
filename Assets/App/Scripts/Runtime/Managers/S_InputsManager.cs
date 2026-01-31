using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputsManager : MonoBehaviour
{
    [TabGroup("References")]
    [Title("Player Input")]
    [SerializeField] private PlayerInput playerInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnMoveInput rseOnMoveInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnLookInput rseOnLookInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnInteractInput rseOnInteractInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnMask1Input rseOnMask1Input;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnMask2Input rseOnMask2Input;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnMask3Input rseOnMask3Input;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnEscapeInput rseOnEscapeInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnSprintInput rseOnSprintInput;


    private IA_PlayerInput iaPlayerInput = null;

    private void Awake()
    {
        iaPlayerInput = new IA_PlayerInput();
        playerInput.actions = iaPlayerInput.asset;
    }

    private void OnEnable()
    {
        playerInput.actions.Enable();

        iaPlayerInput.Player.Move.performed += OnMoveChanged;
        iaPlayerInput.Player.Move.canceled += OnMoveChanged;

        iaPlayerInput.Player.Look.performed += OnLookChanged;
        iaPlayerInput.Player.Look.canceled += OnLookChanged;

        iaPlayerInput.Player.Interact.performed += OnInteractChanged;

        iaPlayerInput.Player.Mask1.performed += OnMask1Changed;

        iaPlayerInput.Player.Mask2.performed += OnMask2Changed;

        iaPlayerInput.Player.Mask3.performed += OnMask3Changed;

        iaPlayerInput.Player.Escape.performed += OnEscapeChanged;

        iaPlayerInput.Player.Sprint.performed += OnSprintChanged;
        iaPlayerInput.Player.Sprint.canceled += OnSprintChanged;
    }

    private void OnDisable()
    {
        playerInput.actions.Disable();

        iaPlayerInput.Player.Move.performed -= OnMoveChanged;
        iaPlayerInput.Player.Move.canceled -= OnMoveChanged;

        iaPlayerInput.Player.Look.performed -= OnLookChanged;
        iaPlayerInput.Player.Look.canceled -= OnLookChanged;

        iaPlayerInput.Player.Interact.performed -= OnInteractChanged;

        iaPlayerInput.Player.Mask1.performed -= OnMask1Changed;

        iaPlayerInput.Player.Mask2.performed -= OnMask2Changed;

        iaPlayerInput.Player.Mask3.performed -= OnMask3Changed;

        iaPlayerInput.Player.Escape.performed -= OnEscapeChanged;

        iaPlayerInput.Player.Sprint.performed -= OnSprintChanged;
        iaPlayerInput.Player.Sprint.canceled -= OnSprintChanged;
    }

    private void OnMoveChanged(InputAction.CallbackContext ctx)
    {
        rseOnMoveInput.Call(ctx.ReadValue<Vector2>());
    }

    private void OnLookChanged(InputAction.CallbackContext ctx)
    {
        rseOnLookInput.Call(ctx.ReadValue<Vector2>());
    }

    private void OnInteractChanged(InputAction.CallbackContext ctx)
    {
        rseOnInteractInput.Call();
    }

    private void OnMask1Changed(InputAction.CallbackContext ctx)
    {
        rseOnMask1Input.Call();
    }

    private void OnMask2Changed(InputAction.CallbackContext ctx)
    {
        rseOnMask2Input.Call();
    }

    private void OnMask3Changed(InputAction.CallbackContext ctx)
    {
        rseOnMask3Input.Call();
    }

    private void OnEscapeChanged(InputAction.CallbackContext ctx)
    {
        rseOnEscapeInput.Call();
    }

    private void OnSprintChanged(InputAction.CallbackContext ctx)
    {
        rseOnSprintInput.Call();
    }
}