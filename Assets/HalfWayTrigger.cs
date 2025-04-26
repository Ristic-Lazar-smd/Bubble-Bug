using UnityEngine;

public class HalfWayTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("Player")){
            LevelManager.Instance.SpawnRandom(transform.position);
            //GetComponent<BoxCollider2D>().enabled=false;
            gameObject.SetActive(false);
            Debug.Log("spawn random");
        }
       
    }
}
