using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ClimbersSlingshotMovement : PlayerMovement
{
    [SerializeField] private float slingForce=5f;
    [SerializeField]private float maxDragDistance = 2f;
    [SerializeField] private float cancelTreshold = 0.2f;
    bool isHolding;
    bool stopOld = false;
    ClimberTrajectory trajectory;
    Vector2 dragVector;
    Vector2 slingshotOrigin;
    Vector2 currentTouchPos;
    bool sling;

    protected override void Awake()
    {
        trajectory = GetComponent<ClimberTrajectory>();
        base.Awake();
    }

    protected override void Update()
    {
        if (isHolding && IsWalled() && !base.IsGrounded) {
            
            sling = true;
            stopOld = true;
            currentTouchPos = inputManager.PrimaryPosition();
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;

            //Drag Vector constraints
            dragVector = currentTouchPos - slingshotOrigin;
            if (dragVector.magnitude > maxDragDistance) { dragVector = dragVector.normalized * maxDragDistance; }
            if (direction>0 && dragVector.x<0.15) dragVector.x = 0.15f;
            if (direction<0 && dragVector.x>-0.15) dragVector.x = -0.15f;

            //Cancel sling
            if ((currentTouchPos-slingshotOrigin).magnitude<cancelTreshold) sling = false;
        }

        if (base.IsGrounded){
            stopOld = false;
            sling = false;
        }

        if(!stopOld) base.Update();

        if (IsWalled() && !isHolding) { stopOld = false; }

        if (sling){
            trajectory.Show(slingshotOrigin);
            trajectory.UpdateDots(rb.transform.position, (-dragVector * slingForce));
        } else trajectory.Hide();
    }

    protected override void OnTouchStart(Vector2 worldPosition, float time) { 
        slingshotOrigin = worldPosition;
        isHolding = true;
        if(!stopOld) base.OnTouchStart(worldPosition, time);
    }
    protected override void OnTouchEnd(Vector2 worldPosition, float time) { 
        isHolding = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (dragVector.magnitude < cancelTreshold) { sling = false; }
        if (sling) {
            base.jumpCounter++;
            rb.AddForce(-dragVector * slingForce, ForceMode2D.Impulse);
            Flip();
            sling = false;
        }
        if(!stopOld) base.OnTouchEnd(worldPosition, time);
    }
    protected override void FixedUpdate() {
        if (!stopOld) { base.FixedUpdate(); }
    }




}
