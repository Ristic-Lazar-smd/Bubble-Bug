using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class PlayerMovement : MonoBehaviour
{
    private float direction;
    public float speed = 1f;
    public float jumpingPower = 8f;
    public float wallSlidingSpeed = 0.5f;
    private bool isWallSliding;
    //private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.3f;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private PlayerInput playerInput;
    InputAction touchAction;
    bool fallstraight = true;
    private GameObject OneWayPlatform;

    
    [Header ("HitBox")]
    [Tooltip("Enable gizmos to see hitbox")]
    [SerializeField]bool showHitBox;
    [SerializeField]float wallHitBoxSize;
    [SerializeField]float groundHitBoxSize;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        touchAction = playerInput.actions["Touch"];
    }
    private void Start(){
        direction = 1;
        rb.transform.position = new Vector3(0,-3.5f,0);
    }
    private void Update()
    {
        
        if (/*touchAction.WasPressedThisFrame()*/Input.GetMouseButtonDown(0) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (/*touchAction.WasReleasedThisFrame()*/Input.GetMouseButtonUp(0) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        WallSlide();
        WallJump();

        if (IsWalled())
        {
            Flip();
        }
        if (IsGrounded()){
            fallstraight = false;
        }
    }
    private void FixedUpdate()
    {
        if (!fallstraight && !isWallSliding)
        {
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
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundHitBoxSize, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallHitBoxSize, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded())
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(0, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
            if (rb.linearVelocity.y <0){
            fallstraight = true;}
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (/*touchAction.WasPressedThisFrame()*/Input.GetMouseButtonDown(0) && wallJumpingCounter > 0f)
        {
            fallstraight = false;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                if(direction == 1)direction = -1;
                else direction = 1;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        //isWallJumping = false;
    }

    private void Flip()
    {
        if (IsGrounded() && IsWalled()){
            if(direction == 1)direction = -1;
            else direction = 1;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void OnDrawGizmos()
    {
        if (showHitBox){
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(wallCheck.position, wallHitBoxSize);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundCheck.position, groundHitBoxSize);
        }
    }
}