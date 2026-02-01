using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputsManager : MonoBehaviour
{
    [TabGroup("References")]
    [Title("Player Input")]
    [SerializeField] private PlayerInput playerInput;

    [TabGroup("Inputs")]
    [SerializeField] RSE_OnPlayerGettingCatch _onPlayerGettingCatch;

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

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnSprintCancelInput rseOnSprintCancelInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnBookInput rseOnBookInput;

    private IA_PlayerInput iaPlayerInput = null;
    private bool inputInitialized = false;

    private void Awake()
    {
        inputInitialized = false;
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
        iaPlayerInput.Player.Sprint.canceled += OnSprintCancelChanged;

        iaPlayerInput.Player.Book.performed += OnBookChanged;

        _onPlayerGettingCatch.action += DesactivatePlayerInput;
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
        iaPlayerInput.Player.Sprint.canceled -= OnSprintCancelChanged;

        iaPlayerInput.Player.Book.performed -= OnBookChanged;

        _onPlayerGettingCatch.action -= DesactivatePlayerInput;
    }

    private void Start()
    {
        StartCoroutine(S_Utils.DelayRealTime(0.6f, () => inputInitialized = true));
    }

    void DesactivatePlayerInput(Transform transform)
    {
        if (inputInitialized)
        {
            iaPlayerInput.Player.Disable();
        }   
    }

    private void OnMoveChanged(InputAction.CallbackContext ctx)
    {
        if (inputInitialized)
        {
            rseOnMoveInput.Call(ctx.ReadValue<Vector2>());
        }
    }

    private void OnLookChanged(InputAction.CallbackContext ctx)
    {
        if (inputInitialized)
        {
            rseOnLookInput.Call(ctx.ReadValue<Vector2>());
        }
    }

    private void OnInteractChanged(InputAction.CallbackContext ctx)
    {
        if (inputInitialized)
        {
            rseOnInteractInput.Call();
        }
    }

    private void OnMask1Changed(InputAction.CallbackContext ctx)
    {
        if (inputInitialized)
        {
            rseOnMask1Input.Call();
        }
    }

    private void OnMask2Changed(InputAction.CallbackContext ctx)
    {
        if (inputInitialized)
        {
            rseOnMask2Input.Call();
        }
    }

    private void OnMask3Changed(InputAction.CallbackContext ctx)
    {
        if (inputInitialized)
        {
            rseOnMask3Input.Call();
        }
    }

    private void OnEscapeChanged(InputAction.CallbackContext ctx)
    {
        if (inputInitialized)
        {
            rseOnEscapeInput.Call();
        }
    }

    private void OnSprintChanged(InputAction.CallbackContext ctx)
    {
        if (inputInitialized)
        {
            rseOnSprintInput.Call();
        }
    }

    private void OnSprintCancelChanged(InputAction.CallbackContext ctx)
    {
        if (inputInitialized)
        {
            rseOnSprintCancelInput.Call();
        }
    }

    private void OnBookChanged(InputAction.CallbackContext ctx)
    {
        if (inputInitialized)
        {
            rseOnBookInput.Call();
        }
    }
}