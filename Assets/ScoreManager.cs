using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    int currentScore;

    void Awake() {
        Instance = this;
    }

    public void UpdateScore(int updateIncrement){

    //currentScore+=updateIncrement*dificulty mod*roomspassed mod*special
    }

    

}
