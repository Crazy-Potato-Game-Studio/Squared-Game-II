using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTouch : MonoBehaviour
{

    [SerializeField] private float SpikesDamage = 3f;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            other.GetComponent<HealthManager>().LoseHealth(SpikesDamage);
        }
    }
}