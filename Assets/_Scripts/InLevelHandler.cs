using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class InLevelHandler : MonoBehaviour
{
    [SerializeField]private GameObject[] hazardGroups;
    int hazardGroupEnabled;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PickAndSpawnHazardGroups();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PickAndSpawnHazardGroups(){
        hazardGroupEnabled = Random.Range(0, hazardGroups.Length);
        hazardGroups[hazardGroupEnabled].SetActive(true);
        Debug.Log(hazardGroupEnabled);
    }
}
