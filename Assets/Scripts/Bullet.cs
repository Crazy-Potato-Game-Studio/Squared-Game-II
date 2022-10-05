using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Ground")){
            GameObject.Destroy(gameObject, 5f);
            GetComponent<CircleCollider2D>().enabled = false;
        }
        if(other.CompareTag("Enemy")){
            GameObject.Destroy(gameObject);
        }
            
    }
}
