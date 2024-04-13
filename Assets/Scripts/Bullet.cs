using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<EnemyHealth>().LoseHP(20f);
        }
        Destroy(gameObject);
    }

    private void Start() {
        Destroy(gameObject, 5f);
    }
}
