using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPickup : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            player.GetComponentInChildren<ArrowsManager>().arrowCount++;
            player.gameObject.GetComponentInChildren<ArrowsManager>().UpdateArrowText();
            player.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 0.1f);
        }
    }
}
