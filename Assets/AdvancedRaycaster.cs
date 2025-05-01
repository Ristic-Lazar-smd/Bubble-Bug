using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RayDefinition2D
{
    [Range(-180, 180)] public float angle = 0f; // Angle in degrees (0 = right, 90 = up)
    [Min(0.1f)] public float length = 5f;
    public Color debugColor = Color.red;
}

public class AdvancedRaycaster : MonoBehaviour
{
    [Header("Ray Configuration")]
    public List<RayDefinition2D> rays = new List<RayDefinition2D>();
    public LayerMask layerMask = ~0; // Default: everything

    [Header("Input Settings")]
    public KeyCode castKey = KeyCode.R;
    public bool drawInEditor = true;

    [Header("Results")]
    [SerializeField] private bool _hasHit;
    [SerializeField] private List<RaycastHit2D> _hits = new List<RaycastHit2D>();

    public bool HasHit => _hasHit;
    public IReadOnlyList<RaycastHit2D> Hits => _hits.AsReadOnly();

    void Update()
    {
        if (Input.GetKeyDown(castKey))
        {
            CastRays2D();
        }
    }
    public bool GroundCheck(){
        //stopAfterHit = true;
        CastRays2D();
        return HasHit;
    }

    public void CastRays2D()
    {
        _hits.Clear();
        _hasHit = false;

        foreach (var rayDef in rays)
        {
            Vector2 direction = CalculateDirection(rayDef.angle);
            Vector2 origin = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDef.length, layerMask);

            if (hit.collider != null)
            {
                _hits.Add(hit);
                _hasHit = true;
                Debug.DrawLine(origin, hit.point, rayDef.debugColor, 0.5f);
                Debug.Log($"Hit 2D collider: {hit.collider.name} at {hit.point}");
            }
            else
            {
                Debug.DrawRay(origin, direction * rayDef.length, 
                    new Color(rayDef.debugColor.r, rayDef.debugColor.g, rayDef.debugColor.b, 0.3f), 0.5f);
            }
        }
    }

    private Vector2 CalculateDirection(float angleDegrees)
    {
        float angleRad = angleDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!drawInEditor) return;

        foreach (var rayDef in rays)
        {
            Vector2 direction = CalculateDirection(rayDef.angle);
            Gizmos.color = rayDef.debugColor;
            Gizmos.DrawRay(transform.position, direction * rayDef.length);
        }
    }
    #endif
}