using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject playerPrefab;
    void Awake()
    {
        Instance = this;
    }
    public void SpawnPlayer()
    {
        GameObject playerInstance = Instantiate(playerPrefab);
        CameraManager.Instance.GetCameraByLabel("PlayCamera").virtualCamera.Follow = playerInstance.transform;
    }
    void Start()
    {

    }

    void Update()
    {

    }
}
