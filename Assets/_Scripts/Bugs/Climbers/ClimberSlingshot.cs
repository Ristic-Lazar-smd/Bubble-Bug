using UnityEngine;


public class ClimberSlingshot : MonoBehaviour
{
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] BugStats stats;
    private float wallHitBoxSize;
    Vector2 dragVector;
    Vector2 slingshotOrigin;
    InputManager inputManager;
    LineRenderer lineRenderer;
    Rigidbody2D rb;
    Vector3 localScale;
    bool isDragging;
    int direction = 1;

    bool stopMove = false;
    bool startGroundedCheck = false;
    public bool grounded;

    [SerializeField]private float maxDragDistance = 2f;
    [SerializeField] private float slingForce=2f;

    void Awake()
    {
        wallHitBoxSize = GetComponent<PlayerDrawGizmos>().wallHitBoxSize;
        inputManager = InputManager.Instance;
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.transform.position = new Vector3(0, -3.5f, 0);
    }
    void OnEnable() {
        inputManager.OnStartTouch += OnTouchStart;
        inputManager.OnEndTouch += OnTouchEnd;
    }
    void OnDisable() {
        inputManager.OnStartTouch -= OnTouchStart;
        inputManager.OnEndTouch += OnTouchEnd;
    }

    void OnTouchStart(Vector2 worldPosition, float time) {
        rb.linearVelocity = new Vector2(0, 0);
        slingshotOrigin = worldPosition;
        isDragging = true;
        stopMove = true;
    }
    void OnTouchEnd(Vector2 worldPosition, float time) {
        if (isDragging){
            isDragging = false;
            stopMove = false;
            grounded = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(-dragVector * slingForce, ForceMode2D.Impulse);
            startGroundedCheck = true;
        }
        lineRenderer.enabled = false;
    }

    void Update() {
        if (startGroundedCheck) {
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, groundLayer);
            if (grounded) { 
                stopMove = false; 
                startGroundedCheck = false; 
            }
        }
        if(isDragging){
            if (dragVector.x>=0 && direction>=0 || dragVector.x<0 && direction<0) Flip();
            dragVector = inputManager.PrimaryPosition() - slingshotOrigin;
            //if (dragVector.magnitude < 1f) isDragging = false;
            if (dragVector.magnitude > maxDragDistance) { dragVector = dragVector.normalized * maxDragDistance; }
            UpdateLine();
        }
        
        if (IsWalled()) OnWallHandler();

        


    }
    
    void OnWallHandler(){
        stopMove = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
    }


    private void UpdateLine(){
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, rb.position);
        lineRenderer.SetPosition(1, rb.position - dragVector);
    }

    void Flip() {
        if (direction == 1) {
            direction = -1;
        } else {
            direction = 1;
        }
        localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    bool IsWalled() {
        return Physics2D.OverlapCircle(wallCheck.position, wallHitBoxSize, wallLayer);
    }
}
