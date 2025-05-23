using UnityEngine;
using UnityEngine.SocialPlatforms;

public class HalfWayTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("Player")){
            gameObject.SetActive(false);
            LevelManager.Instance.SpawnRoom();
        }
    }
}
