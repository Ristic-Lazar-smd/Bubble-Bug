using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    void Awake() {
        Instance = this;
    }

    public void UpdateScore(){
        
    }

}
