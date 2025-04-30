using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;
using UnityEditor.Experimental.GraphView;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]private float speed = 1f;
    [SerializeField]private float jumpingPower = 8f;
    [SerializeField]private int maxJumps;

    [Header("Wall Interaction")]
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);
    public float wallSlidingSpeed = 0.5f;

    [Header("Coyote Time")]
    public float wallJumpCoyoteWindow = 0.2f;
    public float jumpCoyoteWindow = 0.2f;

    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header ("Debug")]
    [Tooltip("Enable gizmos to see hitbox")]
    [SerializeField]bool showHitBox;
    [SerializeField]float wallHitBoxSize;
    [SerializeField]float groundHitBoxSize;

    //Helpers
    private GameObject OneWayPlatform;
    private float direction;
    private bool isWallSliding;
    private float wallJumpCoyoteTimer;
    private float jumpCoyoteTimer;
    private float wallJumpingDirection;

    private Rigidbody2D rb;
    bool fallstraight = true;

    


    private int jumpCounter;
    private bool isGrounded;
    public bool IsGrounded{
        get {return isGrounded;}
        set{
            if (isGrounded != value){
                isGrounded = value;
                //Menjam promenljive kada preko setera promenim IsGrounded
                if (isGrounded){
                    jumpCounter = 0;
                    jumpCoyoteTimer = jumpCoyoteWindow;
                }
            }
        }
    }

    //------------------------------------------------//

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start(){
        direction = 1;
        rb.transform.position = new Vector3(0,-3.5f,0);
    }

    void OnTouch(InputValue value){

        //Player Jump, on finger down apply up velocity, if finger up before peak of jump, apply down velocity
        //Finger down
        if (value.isPressed && CanJump()){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
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
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
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

    private void Update(){
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
    private void FixedUpdate(){
        //Moves player left - right
        if (!fallstraight && !isWallSliding){
           rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
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

    private bool IsWalled(){
        return Physics2D.OverlapCircle(wallCheck.position, wallHitBoxSize, wallLayer);
    }
    private bool CanJump(){
        if (jumpCounter == 0){
            if (IsGrounded || jumpCoyoteTimer>0) return true;
            else return false;
        }
        if (jumpCounter<maxJumps){
                return true;
        }
        return false;
    }

    private void WallSlide(){
        if (IsWalled() && !IsGrounded){
            isWallSliding = true;
            rb.linearVelocity = new Vector2(0, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
            if (rb.linearVelocity.y <0){
            fallstraight = true;}
        }
        else{
            isWallSliding = false;
        }
    }

    private void WallJump(){
        if (isWallSliding){
            wallJumpingDirection = -transform.localScale.x;
            wallJumpCoyoteTimer = wallJumpCoyoteWindow;
        }
        else{
            wallJumpCoyoteTimer -= Time.deltaTime;
        }
    }

    private void Flip(){
        if (/*IsGrounded*/groundCheck.GetComponent<PlayerGroundCheck>().CheckGrounded() && IsWalled()){
            if(direction == 1)direction = -1;
            else direction = 1;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //For debug
    void OnDrawGizmos(){
        if (showHitBox){
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(wallCheck.position, wallHitBoxSize);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundCheck.position, groundHitBoxSize);
        }
    }
}