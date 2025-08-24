using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField]private LevelInfo[] allLevels;

    [Header("Single Room Testing")]
    [SerializeField] bool singleRoomTesting;
    [SerializeField] private LevelInfo testRoom;

    [Header("Settings")]
    [Tooltip("Must match the higest difficulty set in the level scriptable object")]
    [SerializeField] private int maxDifficulty;
    [Tooltip("Number of rooms the player needs to pass for the difficulty to change")]
    public int changeDifficultyEvery;
    [Tooltip("Minimum number of rooms to spawn per biom.")]
    [SerializeField] public int lowEnd;
    [Tooltip("Maximum number of rooms to spawn per biom, Max spawns higEnd - 1 rooms")]
    [SerializeField] public int highEnd;
    [SerializeField] int  startDeletingAfter = 5;


    public static LevelManager Instance { get; private set; }
    Queue<int> scoreQueue;
    private int currentBiomeRoomsLeft = 0;
    private LevelInfo.BiomType currentBiome;
    private int currentDifficulty = 1;
    private int levelCounter = 0;
    private Vector3 spawnPoint;
    private Queue<GameObject> deleteionQueue = new();
    private LevelInfo selectedRoom;
    private LevelInfo previousRoom;
    private List<LevelInfo.BiomType> allBiomes = new();





    void Awake()
    {
        Instance = this;
    }

    void Start(){
        spawnPoint = new Vector3 (0,-4);
        scoreQueue = ScoreManager.Instance.scoreQueue;
        GetAllBiomes();
        //LevelStart();
    }

    public void LevelStart(){
        if (singleRoomTesting) { SpawnTestRooms(); return; }
        SpawnRoom();
        SpawnRoom();
    }
    public void SpawnRoom(){
        GameObject newRoom = Instantiate(GenerateNextRoom().LevelPrefab, spawnPoint, Quaternion.identity);
        spawnPoint = new Vector3(0,newRoom.GetComponent<HighestPointFinder>().GetHighestPoint());

        deleteionQueue.Enqueue(newRoom);
        DeleteQueuedRooms();
    }
    // Override with manual position
    public void SpawnRoom(Vector3 manualSpawnPoint){ 
        GameObject newRoom = Instantiate(GenerateNextRoom().LevelPrefab, manualSpawnPoint, Quaternion.identity);
        spawnPoint = new Vector3(0,newRoom.GetComponent<HighestPointFinder>().GetHighestPoint());
        deleteionQueue.Enqueue(newRoom);
    }
    private void SpawnTestRooms(){
        for (int i = 0; i < 10; i++) {
            GameObject newRoom = Instantiate(testRoom.LevelPrefab, spawnPoint, Quaternion.identity);
            spawnPoint = new Vector3(0, newRoom.GetComponent<HighestPointFinder>().GetHighestPoint());
        }
    }

    public LevelInfo GenerateNextRoom(){
        // Check if we need to switch biome
        if (currentBiomeRoomsLeft <= 0){
            SelectNewBiome();
        }
        // Change difficulty every X levels
        levelCounter++;
        if (levelCounter % changeDifficultyEvery == 0){
            if (currentDifficulty < maxDifficulty) {
                currentDifficulty++;
            }
            Debug.Log($"Difficulty increased to {currentDifficulty}");
        }
        // Filter rooms by current biome and difficulty
        var biomeRooms = allLevels.Where(r => 
            r.Biom == currentBiome && 
            r.Difficulty == currentDifficulty).ToList();

        // Failsafe if no rooms with current filters
        if (biomeRooms.Count == 0){
            biomeRooms = allLevels.Where(r => r.Biom == currentBiome && r.Difficulty == (currentDifficulty-1)).ToList();
        }
        // Remove previousRoom if possible, bad code but not the prio right now!!!!!!!!!!!!!
        if (previousRoom != null && biomeRooms.Count > 1){
            biomeRooms = biomeRooms.Where(r => r != previousRoom).ToList();
        }

        //---
        float totalWeight = biomeRooms.Sum(r => r.weight);
        float roll = Random.value * totalWeight;
        LevelInfo picked = biomeRooms[0]; // fallback

        foreach (var room in biomeRooms){
            if (roll < room.weight){
                picked = room;
                break;
            }
            roll -= room.weight;
        }
        // Decay its weight so it becomes less likely next time
        picked.weight *= 0.5f;
        if (picked.weight < 0.1f) picked.weight = 0.1f; // don't let it vanish completely
        selectedRoom = picked;
        previousRoom = selectedRoom;


        /*
        // Select random room from filtered list
        selectedRoom = biomeRooms[Random.Range(0, biomeRooms.Count)];
        previousRoom = selectedRoom;*/

        //---


        //Debug.Log($"Spawning {selectedRoom.LevelPrefab.name} " + $"(Biome: {currentBiome}, " + $"Difficulty: {currentDifficulty}");
        scoreQueue.Enqueue(selectedRoom.Score);
        Debug.Log(selectedRoom.Score);
        currentBiomeRoomsLeft--;
        return selectedRoom;

    }



    private void SelectNewBiome(){
        // Don't select the same biome twice in a row if possible   
        var candidateBiomes = allBiomes.Where(b => b != currentBiome).ToList();
        if (candidateBiomes.Count == 0){ // Fallback if only one biome exists
            candidateBiomes = allBiomes;
        }
        currentBiome = candidateBiomes[Random.Range(0, candidateBiomes.Count)];
        currentBiomeRoomsLeft = Random.Range(lowEnd, highEnd); // lowEnd to highEnd-1 rooms
        
        //Debug.Log($"Switching to {currentBiome} biome for {currentBiomeRoomsLeft} rooms");
    }
   
    private void GetAllBiomes(){
        allBiomes = allLevels.Select(level => level.Biom).Distinct().ToList();
    }

    private void DeleteQueuedRooms(){
        if (deleteionQueue.Count>=startDeletingAfter){
            Destroy(deleteionQueue.Dequeue());
        }
    }
}
