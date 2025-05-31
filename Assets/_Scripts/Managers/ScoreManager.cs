using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField]TextMeshProUGUI scoreTMP;

    public int currentScore=0;
    public float dificultyMod=1;
    public float roomspassedMod=1;
    public float specialMod=1;
    public Queue<int> scoreQueue = new();


    void Awake() {
        Instance = this;
    }

    public void UpdateScore(){
        currentScore += (int)(scoreQueue.Dequeue()* dificultyMod * roomspassedMod * specialMod);
        scoreTMP.text = currentScore.ToString();
    }
}
