using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    GameObject ignoredGameObject;
    [SerializeField]private TextMeshProUGUI scoreUi;
    public float score;
    public float scoreModifier;
    [SerializeField] float scoreModifierTreshold = 10;
    [SerializeField]private float currentRoomScoreWorth;
    [SerializeField]private float pastRoomScoreWorth;



    void OnCollisionEnter2D(Collision2D col){
        if(!col.gameObject.CompareTag("OneWayPlatform"))return;
        if(col.gameObject != ignoredGameObject){
            ignoredGameObject = col.gameObject;

            pastRoomScoreWorth = currentRoomScoreWorth;
            currentRoomScoreWorth = col.gameObject.GetComponentInParent<LvlInfo>().roomScoreWorth;

            score += scoreModifier * pastRoomScoreWorth;
            scoreUi.text = score.ToString();

            if (score >= scoreModifierTreshold){
                scoreModifier++;
                scoreModifierTreshold *= scoreModifier;
            }
        }
        
    }
}
