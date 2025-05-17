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

}
