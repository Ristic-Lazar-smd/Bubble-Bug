using UnityEngine;
using TMPro;

public class deathLine : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI distanceTMP;
    private Transform playerTransform;
    public float minSpeed = 2f;
    public float maxSpeed = 10f;
    public float catchUpDistance = 20f; // distance at which it reaches max speed
    private float currentSpeed;

    //Text color change
    public float minColorValue = 3f;
    public float maxColorValue = 10f;

    void Start()
    {
        playerTransform = GameManager.Instance.playerInstance.transform;
    }

    
    void Update()
    {
        float distance = playerTransform.position.y - transform.position.y;
        distanceTMP.text = Mathf.Round(distance).ToString();
        // Normalize into 0–1
        float t = Mathf.InverseLerp(0f, catchUpDistance, distance);
        // Lerp between minSpeed and maxSpeed
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, t);
        // Move the death line upward
        transform.position += currentSpeed * Time.deltaTime * Vector3.up;

        //UI text distance display color change
        // Normalize into 0–1
        float a = Mathf.InverseLerp(minColorValue, maxColorValue, distance);
        // Lerp between red (low) and green (high)
        Color c = Color.Lerp(Color.red, Color.green, a);
        distanceTMP.color = c;
    }
}
