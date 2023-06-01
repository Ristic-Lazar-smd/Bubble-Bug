using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFailState : MonoBehaviour
{
 
    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("DeathPlane")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
    }
}
