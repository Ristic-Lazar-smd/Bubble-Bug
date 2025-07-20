using UnityEngine;

[ExecuteInEditMode]
public class HighestPointFinder : MonoBehaviour
{
    [SerializeField, Tooltip("World-space Y position of highest point")]
    private float _worldSpaceHighestY;
    
    public float WorldHighestPoint => _worldSpaceHighestY;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            CalculateWorldSpaceHighestPoint();
        }
    }
    #endif

    void Awake(){
        CalculateWorldSpaceHighestPoint();
    }

    private void CalculateWorldSpaceHighestPoint()
    {
        SpriteRenderer[] allSprites = GetComponentsInChildren<SpriteRenderer>();
        
        if (allSprites.Length == 0)
        {
            _worldSpaceHighestY = transform.position.y;
            Debug.LogWarning("No sprites found - using parent position", this);
            return;
        }

        float maxWorldY = float.MinValue;
        foreach (SpriteRenderer sprite in allSprites)
        {
            // World-space calculation (accounts for all parent transforms)
            float spriteTopWorldY = sprite.bounds.max.y;
            if (spriteTopWorldY > maxWorldY)
            {
                maxWorldY = spriteTopWorldY;
            }
        }
        
        _worldSpaceHighestY = maxWorldY;
    }

    public float GetHighestPoint() => _worldSpaceHighestY;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 worldPos = new Vector3(
            transform.position.x, 
            _worldSpaceHighestY, 
            transform.position.z
        );
        
        Gizmos.DrawSphere(worldPos, 0.15f);
        Gizmos.DrawLine(transform.position, worldPos);
        
        // Label the point in scene view
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(worldPos + Vector3.up * 0.2f, 
            $"Highest: {_worldSpaceHighestY:F2}");
        #endif
    }
    [SerializeField] GameObject halfWay;

    public void SpawnHalfWayTrigger(){
        CalculateWorldSpaceHighestPoint();
        GameObject spawnedHalfWay = Instantiate(halfWay,transform.position,Quaternion.identity,transform);
        spawnedHalfWay.transform.localPosition = new Vector3(0,GetHighestPoint()/2);

    }

    
}