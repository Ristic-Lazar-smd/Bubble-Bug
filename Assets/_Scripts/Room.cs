using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject halfWay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject spawnedHalfWay = Instantiate(halfWay,transform.position,Quaternion.identity,transform);
        spawnedHalfWay.transform.localPosition = Vector3.zero;
    }

}
