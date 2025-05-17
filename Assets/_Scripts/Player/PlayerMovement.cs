using System.Collections;
using NUnit.Framework.Internal;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerFailState))]
[RequireComponent(typeof(PlayerDrawGizmos))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    protected BugStats stats;

    [Header("References")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] bool canWallSlide;
    [SerializeField] bool canWallJump;
    
    private float wallHitBoxSize;
    private InputManager inputManager;
    private Camera cameraMain;
    protected Rigidbody2D rb;

    //Helpers
    protected GameObject OneWayPlatform;
    protected float direction;
    protected bool isWallSliding;
    protected float wallJumpCoyoteTimer;
    protected float jumpCoyoteTimer;
    protected float wallJumpingDirection;
    protected Vector3 localScale;
    bool fallstraight = true;
    protected int jumpCounter;
    protected bool isGrounded;


    public bool IsGrounded {
        get { return isGrounded; }
        set {
            if (isGrounded != value) {
                isGrounded = value;
                //Menjam promenljive kada preko setera promenim IsGrounded
                if (isGrounded) {
                    jumpCounter = 0;
                    jumpCoyoteTimer = stats.jumpCoyoteWindow;
                    fallstraight = false;
                }
            }
        }
    }

    //------------------------------------------------//
    private void OnEnable() {
        inputManager.OnStartTouch += OnTouchStart;
        inputManager.OnEndTouch += OnTouchEnd;
    }
    private void OnDisable() {
        inputManager.OnStartTouch -= OnTouchStart;
        inputManager.OnEndTouch += OnTouchEnd;
    }
    protected void Awake() {
        rb = GetComponent<Rigidbody2D>();
        wallHitBoxSize = GetComponent<PlayerDrawGizmos>().wallHitBoxSize;
        inputManager = InputManager.Instance;
        cameraMain = Camera.main;
    }

    protected virtual void Start(){
        direction = 1;
        rb.transform.position = new Vector3(0, -3.5f, 0);
    }

    protected virtual void OnTouchStart(Vector2 worldPosition, float time)
    {
        //Player Jump, on finger down apply up velocity, if finger up before peak of jump, apply down velocity
        if (CanJump()) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpingPower);
            jumpCounter++;
        }

        //Wall Jump Handler start
        if (wallJumpCoyoteTimer > 0f) {
            fallstraight = false;
            rb.linearVelocity = new Vector2(wallJumpingDirection * stats.wallJumpingPower.x, stats.wallJumpingPower.y);
            wallJumpCoyoteTimer = 0f;

            if (transform.localScale.x != wallJumpingDirection){ Flip(); }
        }
        //Wall Jump Handler end
    }

    protected virtual void OnTouchEnd(Vector2 worldPosition, float time)
    {
        if (rb.linearVelocity.y > 0f) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); 
        }
    }
    protected void Update() {
        //if walled shoot ray straight down, if hit flip,
        if (IsWalled()) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, groundLayer);
            if (hit.collider) {
                Flip();
                Debug.DrawRay(transform.position, Vector2.down * 0.2f, Color.red, 2);
            }
            if (!IsGrounded){
                OnWallHandler();
            }
        } else {
            isWallSliding = false;  
        }

        //Coyote timers
        if (!IsGrounded) { jumpCoyoteTimer -= Time.deltaTime; }
        wallJumpCoyoteTimer -= Time.deltaTime;
    }

    protected void OnWallHandler(){
        if (canWallSlide) {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(0, Mathf.Clamp(rb.linearVelocity.y, -stats.wallSlidingSpeed, float.MaxValue));
            if (rb.linearVelocity.y < 0) {
                fallstraight = true;
            }
        } else { rb.linearVelocity = new Vector2(0, 0); }
            if (canWallJump) { 
                wallJumpingDirection = -transform.localScale.x;
                wallJumpCoyoteTimer = stats.wallJumpCoyoteWindow;
        }
    }

    protected void FixedUpdate() {
        //Moves player left - right
        if (!fallstraight && !isWallSliding) { rb.linearVelocity = new Vector2(direction * stats.speed, rb.linearVelocity.y); }
        
        //OneWayPlatform handler **NAPOMENTA** promeni poziciju iz koje pucas ray ako promenis debljinu platforme
        RaycastHit2D hit = Physics2D.Raycast((/*groundCheck.position*/transform.position + new Vector3(0f, -0.3f)), Vector2.down, 1f, LayerMask.GetMask("OneWayPlatform"));
        if (hit) {
            OneWayPlatform = hit.collider.gameObject;
            StartCoroutine(ChangePlatform());
        }
    }

    IEnumerator ChangePlatform() {
        yield return new WaitForSeconds(0.2f);
        OneWayPlatform.layer = LayerMask.NameToLayer("Platform");
    }

    protected virtual bool IsWalled() {
        return Physics2D.OverlapCircle(wallCheck.position, wallHitBoxSize, wallLayer);
    }

    protected virtual bool CanJump() {
        if (jumpCounter == 0) {
            if (IsGrounded || jumpCoyoteTimer > 0) { return true; }
            else return false;
        }
        if (jumpCounter < stats.maxJumps) { return true; }

        return false;
    }

    protected virtual void Flip() {
        if (direction == 1) {
            direction = -1;
        } else {
            direction = 1;
        }
        localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
