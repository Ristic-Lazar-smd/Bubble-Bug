using UnityEngine;

[CreateAssetMenu(fileName = "New LevelInfo", menuName = "Game Data/Level Info")]
public class LevelInfo : ScriptableObject
{
        public int Stage;
    
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

}
