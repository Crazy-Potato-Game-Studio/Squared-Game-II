using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPickup : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponentInChildren<ArrowsManager>().arrowCount++;
            other.gameObject.GetComponentInChildren<ArrowsManager>().UpdateArrowText();
            other.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 0.1f);
        }
    }
}
