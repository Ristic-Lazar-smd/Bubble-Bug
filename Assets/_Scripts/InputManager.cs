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

    private void Awake()
    {
        Instance = this;
        playerInput = new PlayerInput();
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
        Debug.Log("Touch Started" + playerInput.Touch.TouchPosition.ReadValue<Vector2>());

        if (OnStartTouch != null)
        {
            OnStartTouch(playerInput.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch Ended" + playerInput.Touch.TouchPosition.ReadValue<Vector2>());

        if (OnEndTouch != null)
        {
            OnEndTouch(playerInput.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
        }
    }
}