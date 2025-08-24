using Unity.VisualScripting;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) {
            ScoreManager.Instance.UpdateScore();
            gameObject.SetActive(false);
        }
    }
}
