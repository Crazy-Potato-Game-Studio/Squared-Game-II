using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Debug.Log("Gracz dotknął kolców");
        }
    }
}
