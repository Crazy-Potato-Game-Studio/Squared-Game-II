using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanBullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<HealthManager>().LoseHealth(15f);
        }

        Destroy(gameObject);
    }
}
