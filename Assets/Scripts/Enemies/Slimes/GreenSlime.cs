using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GreenSlime : MonoBehaviour
{
   
    [HideInInspector]
    public bool mustPatrol;
    private bool mustFlip;
    public float enemyMoveSpeed;
    public Rigidbody2D enemyRb;
    public Transform groundCheckPos;
    public LayerMask WalkableLayers;

    public Collider2D bodyCollider;
    [SerializeField] private GameObject enemyCanvas;

    void Start()
    {
        mustPatrol = true;
    }

    void Update()
    {
        if(mustPatrol){
            Patrol();
        }

    }

    private void FixedUpdate() {
        if(mustPatrol){
            mustFlip = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, WalkableLayers);
        }
    }

    void Patrol(){

        if(mustFlip || bodyCollider.IsTouchingLayers(WalkableLayers)){
            Flip();
        }

        enemyRb.velocity = new Vector2(enemyMoveSpeed * Time.fixedDeltaTime, enemyRb.velocity.y);
    }

    void Flip(){
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        enemyMoveSpeed *= -1;
        mustPatrol = true;
        enemyCanvas.GetComponent<ScaleChanger>().ChangeUIScale(transform.localScale.x);
    }
}
