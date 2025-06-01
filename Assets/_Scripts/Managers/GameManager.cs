using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject scoreboard;
    CameraManager cameraManager;
    LevelManager levelManager;
    GameObject playerInstance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cameraManager = CameraManager.Instance;
        levelManager = LevelManager.Instance;
    }

    void Update()
    {

    }
    public void SpawnPlayer()
    {
        playerInstance = Instantiate(playerPrefab);
        CameraManager.Instance.GetCameraByLabel("PlayCamera").virtualCamera.Follow = playerInstance.transform;
    }
    public void StartPlay() {
        levelManager.LevelStart();
        cameraManager.SwitchToVirtualCamera("PlayCamera");
        SpawnPlayer();
    }
    public void FailState(){
        Destroy(playerInstance);
        scoreboard.SetActive(true);
    }
    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
