using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    TextMeshPro scoreTMP;

    int currentScore=0;
    public float dificultyMod=1;
    public float roomspassedMod=1;
    public float specialMod=1;


    void Awake() {
        Instance = this;
    }

    public void UpdateScore(int updateIncrement){
        currentScore += (int)(updateIncrement * dificultyMod * roomspassedMod * specialMod);
        scoreTMP.text = ("test");
    }


    

}
