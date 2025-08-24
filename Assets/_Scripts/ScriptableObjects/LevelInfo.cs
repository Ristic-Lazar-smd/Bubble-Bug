using UnityEngine;

[CreateAssetMenu(fileName = "New LevelInfo", menuName = "Game Data/Level Info")]
public class LevelInfo : ScriptableObject
{
    public int Difficulty;
    [Tooltip("The base amount of points the player gets after passing this level ")]
    public int Score;

    public enum BiomType
    {
        Forest,
        Desert,
        Ice,
        Cave,
        Volcano
    }
    public BiomType Biom;
    public enum GameplayType
    {
        Normal,
        Boss,
        Puzzle,
        TimeTrial,
        Survival
    }
    public GameplayType Gameplay;
    
    public GameObject LevelPrefab;

    [Tooltip("Weight for random selection, weight < 1 less likely to spawn")]
    public float weight = 1f; 

}
