using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPartol : MonoBehaviour
{
    [HideInInspector]
    public bool mustPartol;
    private bool mustTurn;
    public LayerMask groundLayer;
    public Transform groundCheckPos;
    public Rigidbody2D enemyRb;
    public Collider2D bodyCollider;

    public float patrolSpeed = 200f;

    void Start()
    {
        mustPartol = true;
    }

    void Update()
    {
        if(mustPartol){
            Patrol();
        }
    }

    private void FixedUpdate() {
        if(mustPartol){
            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
        }
    }

    void Patrol(){
        
        if(mustTurn || bodyCollider.IsTouchingLayers(groundLayer)){
            Flip();
        }

        enemyRb.velocity = new Vector2(patrolSpeed * Time.deltaTime, enemyRb.velocity.y);
    }

    void Flip(){
        mustPartol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        patrolSpeed *= -1; 
        mustPartol = true;
    }
}
