using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] private LayerMask groundLayer;
    void Awake(){
        player = GetComponentInParent<PlayerMovement>();
    }
    void OnTriggerEnter2D(Collider2D collision){
        player.IsGrounded = true;
    }

    void OnTriggerExit2D(Collider2D collision){
        player.IsGrounded = false;
    }

    public bool CheckGrounded(){
        return Physics2D.OverlapCircle(transform.position, 0.12f, groundLayer);
    }
}
