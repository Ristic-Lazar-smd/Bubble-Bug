using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]private LevelInfo[] allLevels;
    [SerializeField]private float spawnOffset;
    private int currentBiomeRoomsLeft = 0;
    private LevelInfo.BiomType currentBiome;
    private int currentStage = 1;
    private int levelCounter = 0;
    Vector3 spawnPoint;
    Vector3 firstSpawnPoint;
    Vector3 nextSpawnPoint;


    void Awake()
    {
        Instance = this;
        spawnPoint = new Vector3 (0,0);
    }

    void Start(){
        LevelStart();
    }

    public void LevelStart(){
        SpawnRoom(new Vector3(0,-4));
        //SpawnRoom(spawnPoint);
    }
    public void SpawnRoom(){
        Debug.Log("SPAWN POINT JE: "+spawnPoint);
        GameObject newRoom = Instantiate(GenerateNextRoom().LevelPrefab, spawnPoint, Quaternion.identity);
        spawnPoint = new Vector3(0,newRoom.GetComponent<HighestPointFinder>().GetHighestPoint());
    }
    // Override version with manual position
    public void SpawnRoom(Vector3 manualSpawnPoint){ 
        GameObject newRoom = Instantiate(GenerateNextRoom().LevelPrefab, manualSpawnPoint, Quaternion.identity);
        spawnPoint = new Vector3(0,newRoom.GetComponent<HighestPointFinder>().GetHighestPoint());
    }

    public LevelInfo GenerateNextRoom(){
        // Check if we need to switch biome
        if (currentBiomeRoomsLeft <= 0){
            SelectNewBiome();
        }
        // Update stage every 10 levels
        levelCounter++;
        if (levelCounter % 10 == 0){
            currentStage++;
            Debug.Log($"Stage increased to {currentStage}");
        }
        // Filter rooms by current biome and stage
        var biomeRooms = allLevels.Where(r => 
            r.Biom == currentBiome && 
            r.Stage == currentStage).ToList();

        // Failsafe if no rooms with current filters
        if (biomeRooms.Count == 0){
            Debug.LogWarning("No rooms available with current filters!");
            biomeRooms = allLevels.Where(r => r.Biom == currentBiome).ToList();
        }
        // Select random room from filtered list
        var selectedRoom = biomeRooms[Random.Range(0, biomeRooms.Count)];
        currentBiomeRoomsLeft--;
        
        Debug.Log($"Spawning {selectedRoom.LevelPrefab.name} " +
                 $"(Biome: {currentBiome}, " +
                 $"Difficulty: {currentStage}");
        
        return selectedRoom;
    }

    private void SelectNewBiome(){
        // Get all unique biomes
        var allBiomes = allLevels.Select(level => level.Biom).Distinct().ToList();
        // Don't select the same biome twice in a row if possible   
        var candidateBiomes = allBiomes.Where(b => b != currentBiome).ToList();
        if (candidateBiomes.Count == 0){ // Fallback if only one biome exists
            candidateBiomes = allBiomes;
        }
        currentBiome = candidateBiomes[Random.Range(0, candidateBiomes.Count)];
        currentBiomeRoomsLeft = Random.Range(3, 6); // 3-5 rooms
        
        Debug.Log($"Switching to {currentBiome} biome for {currentBiomeRoomsLeft} rooms");
    }


}
