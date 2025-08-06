using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ClimbersSlingshotMovement : PlayerMovement {
    [SerializeField] private float slingForce = 5f;
    [SerializeField] private float maxDragDistance = 2f;
    [SerializeField] private float cancelTreshold = 0.2f;
    bool isHolding;
    bool stopOld = false;
    ClimberTrajectory trajectory;
    Vector2 dragVector;
    Vector2 slingshotOrigin;
    Vector2 currentTouchPos;
    bool isSlinging;
    bool readyToStick;

    protected override void Awake() {
        trajectory = GetComponent<ClimberTrajectory>();
        base.Awake();
        InputManager.Instance.swipeDetection.OnAnySwipe += PrepareToStick;
        slingshotOrigin = Vector2.zero;
    }

    protected override void Update() {
        if (IsWalled() && readyToStick) {
            isSlinging = true;
            readyToStick = false;
        }

        if (isSlinging) {
            stopOld = true;
            currentTouchPos = inputManager.PrimaryPosition();
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;

            //Drag Vector constraints
            dragVector = currentTouchPos - slingshotOrigin;
            if (dragVector.magnitude > maxDragDistance) { dragVector = dragVector.normalized * maxDragDistance; }
            if (direction > 0 && dragVector.x < 0.15) dragVector.x = 0.15f;
            if (direction < 0 && dragVector.x > -0.15) dragVector.x = -0.15f;

            ////Cancel sling
            //if ((currentTouchPos-slingshotOrigin).magnitude<cancelTreshold) isSlinging = false;

            if (slingshotOrigin != Vector2.zero) {
                trajectory.Show(slingshotOrigin);
                trajectory.UpdateDots(rb.transform.position, (-dragVector * slingForce));
            }

        }
        else {
            stopOld = false;
            trajectory.Hide();
        }

        if (base.IsGrounded) {
            stopOld = false;
            isSlinging = false;
            readyToStick = false;
        }

        if (!stopOld) base.Update();
    }

    protected override void OnTouchStart(Vector2 worldPosition) {
        if (isSlinging) {
            slingshotOrigin = worldPosition;
        }

        isHolding = true;
        if (!stopOld) base.OnTouchStart(worldPosition);
    }
    protected override void OnTouchEnd(Vector2 worldPosition) {
        isHolding = false;

        if (slingshotOrigin != Vector2.zero && isSlinging) {

            //if (dragVector.magnitude < cancelTreshold) { isSlinging = false; }

            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            base.jumpCounter += 2;
            rb.AddForce(-dragVector * slingForce, ForceMode2D.Impulse);
            Flip();
            isSlinging = false;
            slingshotOrigin = Vector2.zero;
        }

        if (!stopOld) base.OnTouchEnd(worldPosition);
    }
    protected override void FixedUpdate() {
        if (!stopOld) { base.FixedUpdate(); }
    }

    private void PrepareToStick() {
        if(!isGrounded && !IsWalled())
        readyToStick = true;
    }
}
