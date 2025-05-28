using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ClimbersSlingshotMovement : PlayerMovement
{
    [SerializeField] private float slingForce=2f;
    bool isHolding;
    bool stopOld = false;
    InputManager climberInputManager;
    LineRenderer lineRenderer;  
    Vector2 dragVector;
    Vector2 slingshotOrigin;
    bool sling;

    protected override void Awake()
    {
        climberInputManager = InputManager.Instance;
        lineRenderer = GetComponent<LineRenderer>();
        base.Awake();
    }
    protected override void OnEnable() { 
        //climberInputManager.OnStartHold += OnHoldStart;
        //climberInputManager.OnEndHold += OnHoldEnd;
        base.OnEnable();
    }

    protected override void Update()
    {
        Debug.Log(rb.linearVelocityX);
        if (isHolding && IsWalled()) {
            
            sling = true;
            stopOld = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
            
            dragVector = inputManager.PrimaryPosition() - slingshotOrigin;
            //if (dragVector.x>=0 && base.direction>=0 || dragVector.x<0 && base.direction<0) base.Flip();
            UpdateLine();

        }else {  /*stopOld = false;*/}

        if(!stopOld) base.Update();

        if (base.IsGrounded){
            stopOld = false;
            sling = false;
        }

        

    }

    protected override void OnTouchStart(Vector2 worldPosition, float time) { 
        slingshotOrigin = worldPosition;
        isHolding = true;
        if(!stopOld) base.OnTouchStart(worldPosition, time);
    }
    protected override void OnTouchEnd(Vector2 worldPosition, float time) { 
        if(!stopOld) base.OnTouchEnd(worldPosition, time);
        //stopOld = false;
        isHolding = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (sling) {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            lineRenderer.enabled = false;
            rb.AddForce(-dragVector * slingForce, ForceMode2D.Impulse);
            lineRenderer.enabled = false;
            Flip();

            //sling = false;
        }
    }
    protected override void FixedUpdate() {
        if (!sling) { base.FixedUpdate(); }
    }


    private void UpdateLine(){
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, rb.position);
        lineRenderer.SetPosition(1, rb.position - dragVector);
    }
}
