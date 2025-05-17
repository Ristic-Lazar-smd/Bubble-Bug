using System;
using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private Coroutine corutine;

    [SerializeField] private float minimumDistance = 0.2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = 0.9f;
    [SerializeField] private GameObject trail;
    [SerializeField] private bool debug;

    public event Action OnSwipeUp;
    public event Action OnSwipeDown;
    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;

    private void Awake() {
        inputManager = InputManager.Instance;
    }

    private void OnEnable() {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable() {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time) {
        startPosition = position;
        startTime = time;
        trail.SetActive(true);
        trail.transform.position = position;
        corutine = StartCoroutine(Trail());
    }

    private IEnumerator Trail() {
        while (true) {
            trail.transform.position = inputManager.PrimaryPosition();
            yield return null;
        }
    }

    private void SwipeEnd(Vector2 position, float time) {
        trail.SetActive(false);
        StopCoroutine(corutine);
        endPosition = position;
        endTime = time;
        DetectSwipe();

    }

    private void DetectSwipe() {
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance && (endTime - startTime) <= maximumTime) {
            if(debug) Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;

            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction) {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold) {
            if (debug) Debug.Log("Swipe Up");
            OnSwipeUp?.Invoke();
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold) {
            if (debug) Debug.Log("Swipe down");
            OnSwipeDown?.Invoke();
        }
        else if(Vector2.Dot(Vector2.left, direction) > directionThreshold) {
            if (debug) Debug.Log("Swipe left");
            OnSwipeLeft?.Invoke();
        }
        else if(Vector2.Dot(Vector2.right, direction) > directionThreshold) {
            if (debug) Debug.Log("Swipe right");
            OnSwipeRight?.Invoke();
        }
    }

}
