using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBullet : MonoBehaviour
{
    private int collisionCounter;
    [SerializeField] private GameObject bulletParticles;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider" || other.gameObject.tag == "Ground" || other.gameObject.tag == "Platform"){
            collisionCounter++;
            GameObject currentParticles = Instantiate(bulletParticles, transform.position, Quaternion.identity);
            Destroy(currentParticles, 3f);
            if(collisionCounter >= Random.Range(4,8)){
                Destroy(gameObject);
            }
        }

        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<HealthManager>().LoseHealth(15f, 0.7f);
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Water"){
            Destroy(gameObject);
        }
    }
}
