using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInput playerInput;
    [field: NonSerialized] public SwipeDetection swipeDetection { get; private set; }

    // Eventovi za Touch
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;
    // Eventovi za Hold
    public event Action OnStartHold;
    public event Action OnEndHold;
    public event Action OnCancelHold;
    public event Action OnPerformHold;

    // Misc
    [Tooltip("Debug loguje pozociju i vreme touch start i touch end")]
    [SerializeField] bool debug;
    private Camera mainCamera;
    private bool holding;

    private void Awake()
    {
        Instance = this;
        playerInput = new PlayerInput();
        mainCamera = Camera.main;
        swipeDetection = GetComponent<SwipeDetection>();
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
        //Debug.Log("Hold Performed");
        holding = true;

        OnPerformHold?.Invoke();
    }
    private void EndHold(InputAction.CallbackContext context)
    {  
        if(holding == true)
        {
            //Debug.Log("Hold Ended");
            OnEndHold?.Invoke();
            holding = false;
        }
        else
        {
            //Debug.Log("Hold Canceled");
            OnCancelHold?.Invoke();
            holding = false;
        }
    }
    private void StartHold(InputAction.CallbackContext context)
    {
        Debug.Log("Hold Started");
        holding = false;

        OnStartHold?.Invoke();
    }
}
