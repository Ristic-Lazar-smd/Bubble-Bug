using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField]private float spawnOffset;
    [SerializeField]private Object[] lvls;
    GameObject[] toDelete;
    private int n = 2;
    GameObject toSpawn;
    Vector3 spawnPoint;

    int pastRoll;
    int thisRoll;
   

    void Start(){
        toDelete = new GameObject[] { null, null, null,null,null };
        lvls = Resources.LoadAll("Levels");
        InitialSpawn();
    }

    
    private void SpawnRandom(Vector3 triggerPoint){
        toSpawn = (GameObject)lvls[NewRoll()];
        spawnPoint = triggerPoint + new Vector3(0,spawnOffset);
        toDelete[n] = Instantiate(toSpawn, spawnPoint, Quaternion.identity) as GameObject;
        DeleteRooms();
    }

    private void DeleteRooms(){
        switch (n){
            case 0: {
                Object.Destroy(toDelete[1]);
                n++;
            }break;
            case 1: {
                Object.Destroy(toDelete[2]);
                n++;
            }break;
            case 2: {
                Object.Destroy(toDelete[3]);
                n++;
            }break;
            case 3: {
                Object.Destroy(toDelete[4]);
                n++;
            }break;
            case 4: {
                Object.Destroy(toDelete[0]);
                n=0;
            }break;
        }
    }

    private void InitialSpawn(){
        thisRoll = Random.Range(0, lvls.Length);
        toSpawn = (GameObject)lvls[thisRoll];
        GameObject x = Instantiate(toSpawn, new Vector2(0,1), Quaternion.identity) as GameObject;
        toDelete[0] = x;

        toSpawn = (GameObject)lvls[NewRoll()];
        toDelete[1] = Instantiate(toSpawn, new Vector2(0,11), Quaternion.identity) as GameObject;
    }

    
    private int NewRoll(){
        pastRoll = thisRoll;
        while (thisRoll == pastRoll){
            thisRoll = Random.Range(0, lvls.Length);
        }
        return thisRoll;
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("Halfway")){
            SpawnRandom(col.gameObject.transform.parent.gameObject.transform.position);
            col.enabled=false;
        }
    }
}
