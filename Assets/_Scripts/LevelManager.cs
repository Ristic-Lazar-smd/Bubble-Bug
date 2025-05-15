using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]private LevelInfo[] allLevels;
    private int currentBiomeRoomsLeft = 0;
    private LevelInfo.BiomType currentBiome;
    private int currentDifficulty = 1;
    private int levelCounter = 0;
    Vector3 spawnPoint;
    Vector3 firstSpawnPoint;
    Vector3 nextSpawnPoint;

    [Header("Settings")]

    [Tooltip("Number of rooms the player needs to pass for the difficulty to change")]
    public int changeDifficultyEvery;
    [Tooltip("Minimum number of rooms to spawn per biom.")]
    [SerializeField] int lowEnd;
    [Tooltip("Maximum number of rooms to spawn per biom, Max spawns higEnd - 1 rooms")]
    [SerializeField] int highEnd;


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
        // Change difficulty every X levels
        levelCounter++;
        if (levelCounter % changeDifficultyEvery == 0){
            currentDifficulty++;
            Debug.Log($"Difficulty increased to {currentDifficulty}");
        }
        // Filter rooms by current biome and difficulty
        var biomeRooms = allLevels.Where(r => 
            r.Biom == currentBiome && 
            r.Difficulty == currentDifficulty).ToList();

        // Failsafe if no rooms with current filters
        if (biomeRooms.Count == 0){
            Debug.LogWarning("No rooms available with current filters! Random room spawned.");
            biomeRooms = allLevels.Where(r => r.Biom == currentBiome).ToList();
        }
        // Select random room from filtered list
        var selectedRoom = biomeRooms[Random.Range(0, biomeRooms.Count)];
        currentBiomeRoomsLeft--;
        
        Debug.Log($"Spawning {selectedRoom.LevelPrefab.name} " +
                 $"(Biome: {currentBiome}, " +
                 $"Difficulty: {currentDifficulty}");
        
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
        currentBiomeRoomsLeft = Random.Range(lowEnd, highEnd); // lowEnd to highEnd-1 rooms
        
        Debug.Log($"Switching to {currentBiome} biome for {currentBiomeRoomsLeft} rooms");
    }


}
