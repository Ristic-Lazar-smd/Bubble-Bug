using Unity.VisualScripting;
using UnityEngine;

public class PlayerDrawGizmos : MonoBehaviour
{
    [Header("Debug")]
    [Tooltip("Enable gizmos to see hitbox")]
    [SerializeField] private bool showHitBox;
    [SerializeField] public float wallHitBoxSize;
    [SerializeField] public float groundHitBoxSize;

    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    protected virtual void OnDrawGizmos()
    {
        if (showHitBox)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(wallCheck.position, wallHitBoxSize);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundCheck.position, groundHitBoxSize);
        }
    }
}
