using UnityEngine;

public class ClimberGroundCheck : MonoBehaviour
{
    ClimberSlingshot player;
    [SerializeField] private LayerMask groundLayer;
    void Awake(){
        player = GetComponentInParent<ClimberSlingshot>();
    }
    void OnTriggerEnter2D(Collider2D collision){
        player.grounded = true;
    }

    void OnTriggerExit2D(Collider2D collision){
        player.grounded = false;
    }

    /*public bool CheckGrounded(){
        return Physics2D.OverlapCircle(transform.position, 0.12f, groundLayer);
    }*/
}
