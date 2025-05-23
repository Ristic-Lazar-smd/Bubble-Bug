using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInput playerInput;

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;
    [Tooltip("Debug loguje pozociju i vreme touch start i touch end")]
    [SerializeField] bool debug;
    private Camera mainCamera;
    private bool holding;

    public TextMeshProUGUI started;
    public TextMeshProUGUI performed;
    public TextMeshProUGUI end;

    private int counter1;
    private int counter2;
    private int counter3;

    private void Awake()
    {
        Instance = this;
        playerInput = new PlayerInput();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        playerInput.Touch.Touch.started += ctx => StartTouch(ctx);
        playerInput.Touch.Touch.canceled += ctx => EndTouch(ctx);
        playerInput.Touch.Hold.performed += ctx => HoldPerformed(ctx);
        playerInput.Touch.Hold.started += ctx => StartHold(ctx);
        playerInput.Touch.Hold.canceled += ctx => EndHold(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        if (debug) { Debug.Log("Touch Started" + (Utils.ScreenToWorld(mainCamera, playerInput.Touch.TouchPosition.ReadValue<Vector2>()))); }

        if (OnStartTouch != null)
        {
            OnStartTouch(Utils.ScreenToWorld(mainCamera, playerInput.Touch.TouchPosition.ReadValue<Vector2>()), (float)context.startTime);
        }
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (debug) { Debug.Log("Touch Ended" + (Utils.ScreenToWorld(mainCamera, playerInput.Touch.TouchPosition.ReadValue<Vector2>()))); }

        if (OnEndTouch != null)
        {
            OnEndTouch(Utils.ScreenToWorld(mainCamera, playerInput.Touch.TouchPosition.ReadValue<Vector2>()), (float)context.time);
        }
    }

    public Vector2 PrimaryPosition() {
        return Utils.ScreenToWorld(mainCamera, playerInput.Touch.TouchPosition.ReadValue<Vector2>());
    }

    private void HoldPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Hold Performed");
        performed.text += counter2;
    }
    private void EndHold(InputAction.CallbackContext context) {
        Debug.Log("Hold Ended");
        end.text += counter3;
    }
    private void StartHold(InputAction.CallbackContext context)
    {
        Debug.Log("Hold Started");
        started.text += counter1;
    }
}
