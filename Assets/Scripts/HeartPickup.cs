using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject healingParticles;
    [SerializeField] private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            SpawnParticles(other);
            player.GetComponent<HealthManager>().GainHealth(25);
            player.GetComponent<AudioSource>().PlayOneShot(clip);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 0.1f);
        }
    }

    private void SpawnParticles(Collision2D other){
        GameObject healParticles = Instantiate(healingParticles, other.transform.position, other.transform.rotation);
        healingParticles.transform.parent = null;
        Destroy(healParticles, 5);
    }
}
