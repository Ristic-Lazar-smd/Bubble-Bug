using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField]private LevelInfo[] levels;
    [SerializeField]private float spawnOffset;
    GameObject[] toDelete;
    private int n = 2;
    GameObject toSpawn;
    Vector3 spawnPoint;

    int pastRoll;
    int thisRoll;

    void Awake()
    {
        Instance = this;
    }

    void Start(){
        toDelete = new GameObject[] { null, null, null,null,null };
        //lvls = Resources.LoadAll("Levels");
        InitialSpawn();
        Debug.Log(levels.Length);
    }

    
    public void SpawnRandom(Vector3 triggerPoint){
        toSpawn = levels[NewRoll()].LevelPrefab;
        spawnPoint = triggerPoint + new Vector3(0,spawnOffset);
        toDelete[n] = Instantiate(toSpawn, spawnPoint, Quaternion.identity) as GameObject;
        //DeleteRooms();
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
        thisRoll = Random.Range(0, levels.Length);
        toSpawn = levels[thisRoll].LevelPrefab;
        GameObject x = Instantiate(toSpawn, new Vector2(0,1), Quaternion.identity) as GameObject;
        toDelete[0] = x;

        toSpawn = levels[NewRoll()].LevelPrefab;
        toDelete[1] = Instantiate(toSpawn, new Vector2(0,11), Quaternion.identity) as GameObject;
    }

    
    private int NewRoll(){
        pastRoll = thisRoll;
        while (thisRoll == pastRoll){
            thisRoll = Random.Range(0, levels.Length);
        }
        return thisRoll;
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("Halfway")){
            SpawnRandom(col.gameObject.transform.parent.gameObject.transform.position);
            col.enabled=false;
        }
        Debug.Log("spawn random");
    }
}
