using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    [SerializeField] private float SpikesDamage = 10f;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            other.GetComponent<HealthManager>().LoseHealth(SpikesDamage, 1f);
        }
    }
}