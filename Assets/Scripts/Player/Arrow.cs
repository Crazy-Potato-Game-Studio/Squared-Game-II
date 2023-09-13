using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    bool hasHit;
    public float arrowDamage;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 10f);
    }

    private void Update() {
        if(!hasHit){
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = UnityEngine.Quaternion.AngleAxis(angle, UnityEngine.Vector3.forward);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        source.PlayOneShot(clip);

        if(other.gameObject.tag == "Enemy"){
            arrowDamage = Mathf.Round(arrowDamage);
            other.gameObject.GetComponent<EnemyHealth>().LoseHP(arrowDamage);
            Destroy(this.gameObject);
        }

        if(other.gameObject.tag == "Ground"){
            hasHit = true;
            
            rb.velocity = UnityEngine.Vector2.zero;
            rb.isKinematic = true;
            Destroy(GetComponent<PolygonCollider2D>());
            Destroy(this);
        }
        
    }
}
