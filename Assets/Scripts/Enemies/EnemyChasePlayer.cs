using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasePlayer : MonoBehaviour
{

    private Transform player;
    [SerializeField] private float agroDistance = 7f;
    private bool isChasingPlayer;
    [SerializeField] LayerMask layerMask;
    [SerializeField] private float enemyMoveSpeed;
    private Rigidbody2D enemyRb;
    private Vector2 distance;
    [SerializeField] private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyRb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(Physics2D.CircleCast(transform.position,3f ,Vector2.right,agroDistance, layerMask) || Physics2D.Raycast(transform.position, Vector2.left, agroDistance, layerMask) ){
            Debug.DrawRay(transform.position, Vector2.left * agroDistance, Color.red);
            Debug.DrawRay(transform.position, Vector2.right * agroDistance, Color.blue);
            ChasePlayer();
        }else{
            isChasingPlayer = false;
            animator.SetBool("isChasingPlayer", isChasingPlayer);
        }
    }

    void ChasePlayer(){

        isChasingPlayer = true;
        animator.SetBool("isChasingPlayer", isChasingPlayer);

        distance = transform.position - player.position;

        enemyRb.velocity = new Vector2(enemyMoveSpeed * Time.fixedDeltaTime, enemyRb.velocity.y);

        if(distance.x > 0){
            transform.localScale = new Vector2(-1, transform.localScale.y);
            enemyMoveSpeed = -150;
        }else{
            transform.localScale = new Vector2(1, transform.localScale.y);
            enemyMoveSpeed = 150;
        }           
            
    }
}
