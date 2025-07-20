using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject playerBasePrefab;
    [SerializeField] private GameObject playerClimberPrefab;
    [SerializeField] private GameObject playerDasherPrefab;
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

    //Quick fiz za spawn razlicitih buba na dugme
    public void SpawnBasePlayer()
    {
        playerInstance = Instantiate(playerBasePrefab);
        CameraManager.Instance.GetCameraByLabel("PlayCamera").virtualCamera.Follow = playerInstance.transform;
    }
    public void SpawnClimberPlayer()
    {
        playerInstance = Instantiate(playerClimberPrefab);
        CameraManager.Instance.GetCameraByLabel("PlayCamera").virtualCamera.Follow = playerInstance.transform;
    }
    public void SpawnDasherPlayer()
    {
        playerInstance = Instantiate(playerDasherPrefab);
        CameraManager.Instance.GetCameraByLabel("PlayCamera").virtualCamera.Follow = playerInstance.transform;
    }
    //Quick fiz za spawn razlicitih buba na dugme

    //Main menu dugme odradi prvo startplay po onda nekog playera spawnuje
    public void StartPlay() {
        levelManager.LevelStart();
        cameraManager.SwitchToVirtualCamera("PlayCamera");
        //SpawnPlayer();
    }
    public void FailState(){
        Destroy(playerInstance);
        scoreboard.SetActive(true);
    }
    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
