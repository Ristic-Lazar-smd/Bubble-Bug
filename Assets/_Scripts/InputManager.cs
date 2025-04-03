using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference swipeAction;
    private Vector2 startPosition;
    private Vector2 endPosition;
    [SerializeField] private float minSwipeDistance = 50f;

    private void OnEnable()
    {
        swipeAction.action.Enable();
        swipeAction.action.performed += OnSwipePerformed;
    }

    private void OnDisable()
    {
        swipeAction.action.performed -= OnSwipePerformed;
        swipeAction.action.Disable();
    }

    private void OnSwipePerformed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 currentPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            
            if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                startPosition = currentPosition;
            }
            else if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                endPosition = currentPosition;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        Vector2 swipeDelta = endPosition - startPosition;
        
        if (swipeDelta.magnitude >= minSwipeDistance)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                // Horizontal swipe
                if (swipeDelta.x > 0)
                {
                    Debug.Log("Right Swipe");
                }
                else
                {
                    Debug.Log("Left Swipe");
                }
            }
            else
            {
                // Vertical swipe
                if (swipeDelta.y > 0)
                {
                    Debug.Log("Up Swipe");
                }
                else
                {
                    Debug.Log("Down Swipe");
                }
            }
        }
    }
}