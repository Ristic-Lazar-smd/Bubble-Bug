using UnityEngine;

public class HalfWayTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("Player")){
            //old
            //LevelManager.Instance.SpawnRandom(transform.position);
            //new
            LevelManager.Instance.SpawnRoom();
            gameObject.SetActive(false);
        }
       
    }
}
