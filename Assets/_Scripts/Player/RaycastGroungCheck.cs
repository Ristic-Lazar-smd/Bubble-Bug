using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RayDefinition2D{
    [Range(-180, 180)] public float angle = 0f; // Angle in degrees (0 = right, 90 = up)
    [Min(0.1f)] public float length = 5f;
    public Color debugColor = Color.red;
}
public class RaycastGroungCheck : MonoBehaviour
{
    PlayerMovement playerMovement;

    [Header("Ray Configuration")]
    [Range(0, 2)]public float masterLength = 1f;
    public List<RayDefinition2D> rays = new List<RayDefinition2D>();
    public LayerMask layerMask = ~0;
    public bool drawInEditor = true;

    [Header("Results")]
    [SerializeField] private bool _hasHit;
    public bool HasHit => _hasHit;

    //Helpers
    bool currentState = false;
    bool tracker = false;

    //-------------------------------------//
    void Awake(){
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update(){
        //Proverim da l je grounded, ukoliko je promenio state onda saljem playerMovement skripti
        currentState = GroundCheck();
        if (tracker != currentState){
            playerMovement.IsGrounded = currentState;
            tracker = currentState;
        } 

    }
    public bool GroundCheck(){
        CastRays2D();
        return HasHit;
    }

    public void CastRays2D(){
        _hasHit = false;

        foreach (var rayDef in rays){
            Vector2 direction = CalculateDirection(rayDef.angle);
            Vector2 origin = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDef.length*masterLength, layerMask);
            if (hit.collider != null) _hasHit = true;
        }
    }

    private Vector2 CalculateDirection(float angleDegrees){
        float angleRad = angleDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;
    }

    #if UNITY_EDITOR
    void OnDrawGizmos(){
        if (!drawInEditor) return;

        foreach (var rayDef in rays){
            Vector2 direction = CalculateDirection(rayDef.angle);
            Gizmos.color = rayDef.debugColor;
            Gizmos.DrawRay(transform.position, direction * rayDef.length*masterLength);
        }
    }
    #endif
}