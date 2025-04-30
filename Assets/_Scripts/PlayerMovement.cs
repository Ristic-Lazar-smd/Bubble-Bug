using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerFailState))]
[RequireComponent(typeof(PlayerDrawGizmos))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] protected BugStats stats;

    [Header("References")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask wallLayer;
    private float wallHitBoxSize;


    //Helpers
    protected GameObject OneWayPlatform;
    protected float direction;
    protected bool isWallSliding;
    protected float wallJumpCoyoteTimer;
    protected float jumpCoyoteTimer;
    protected float wallJumpingDirection;

    protected Rigidbody2D rb;
    bool fallstraight = true;

    protected int jumpCounter;
    protected bool isGrounded;
    public bool IsGrounded{
        get {return isGrounded;}
        set{
            if (isGrounded != value){
                isGrounded = value;
                //Menjam promenljive kada preko setera promenim IsGrounded
                if (isGrounded){
                    jumpCounter = 0;
                    jumpCoyoteTimer = stats.jumpCoyoteWindow;
                }
            }
        }
    }

    //------------------------------------------------//

    protected void Awake(){
        rb = GetComponent<Rigidbody2D>();
        wallHitBoxSize = GetComponent<PlayerDrawGizmos>().wallHitBoxSize;
    }
    protected void Start(){
        direction = 1;
        rb.transform.position = new Vector3(0,-3.5f,0);
    }

    protected virtual void OnTouch(InputValue value){

        //Player Jump, on finger down apply up velocity, if finger up before peak of jump, apply down velocity
        //Finger down
        if (value.isPressed && CanJump()){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpingPower);
            jumpCounter++;
            Debug.Log(jumpCoyoteTimer);
            //IsGrounded = false; //setujem i ovde da bi kod dovoljno brzo prebacio IsGrounded na false
        }
        //Finger up
        else if (rb.linearVelocity.y > 0f){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        //Wall Jump Handler start 
        if (value.isPressed && wallJumpCoyoteTimer > 0f){
            fallstraight = false;
            rb.linearVelocity = new Vector2(wallJumpingDirection * stats.wallJumpingPower.x, stats.wallJumpingPower.y);
            wallJumpCoyoteTimer = 0f;

            if (transform.localScale.x != wallJumpingDirection){
                if(direction == 1)direction = -1;
                else direction = 1;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        //Wall Jump Handler end 
    }

    protected void Update(){
        WallSlide();
        WallJump();

        if (IsWalled()){
            Flip();
        }
        if (IsGrounded){
            fallstraight = false;
        }else {
            jumpCoyoteTimer -= Time.deltaTime;
        }
        
    }
    protected void FixedUpdate(){
        //Moves player left - right
        if (!fallstraight && !isWallSliding){
           rb.linearVelocity = new Vector2(direction * stats.speed, rb.linearVelocity.y);
        }
        //OneWayPlatform handler **NAPOMENTA** promeni poziciju iz koje pucas ray ako promenis debljinu platforme
        RaycastHit2D hit = Physics2D.Raycast((groundCheck.position + new Vector3(0f,-0.3f)), Vector2.down,1f,LayerMask.GetMask("OneWayPlatform"));
        if (hit){
            OneWayPlatform = hit.collider.gameObject;
            StartCoroutine(ChangePlatform());
        }
    }
    IEnumerator ChangePlatform(){
        yield return new WaitForSeconds(0.2f);
        OneWayPlatform.layer = LayerMask.NameToLayer("Platform");
    }

    /*private bool IsGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position, groundHitBoxSize, groundLayer);
    }*/

    protected virtual bool IsWalled(){
        return Physics2D.OverlapCircle(wallCheck.position, wallHitBoxSize, wallLayer);
    }
    protected virtual bool CanJump(){
        if (jumpCounter == 0){
            if (IsGrounded || jumpCoyoteTimer>0) return true;
            else return false;
        }
        if (jumpCounter< stats.maxJumps){
                return true;
        }
        return false;
    }

    protected virtual void WallSlide(){
        if (IsWalled() && !IsGrounded){
            isWallSliding = true;
            rb.linearVelocity = new Vector2(0, Mathf.Clamp(rb.linearVelocity.y, -stats.wallSlidingSpeed, float.MaxValue));
            if (rb.linearVelocity.y <0){
            fallstraight = true;}
        }
        else{
            isWallSliding = false;
        }
    }

    protected virtual void WallJump(){
        if (isWallSliding){
            wallJumpingDirection = -transform.localScale.x;
            wallJumpCoyoteTimer = stats.wallJumpCoyoteWindow;
        }
        else{
            wallJumpCoyoteTimer -= Time.deltaTime;
        }
    }

    protected virtual void Flip(){
        if (/*IsGrounded*/groundCheck.GetComponent<PlayerGroundCheck>().CheckGrounded() && IsWalled()){
            if(direction == 1)direction = -1;
            else direction = 1;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}