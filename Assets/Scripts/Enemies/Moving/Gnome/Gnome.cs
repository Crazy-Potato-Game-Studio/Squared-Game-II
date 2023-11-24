using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnome : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float agroDistance = 7f;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float gnomeMoveSpeed;
    [SerializeField] private Rigidbody2D gnomeRb;
    [SerializeField] private Collider2D swordCollider;
    [SerializeField] private Animator animator;
    private float totalDistance;
    private Vector2 direction;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        swordCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        totalDistance = Vector2.Distance(transform.position, player.transform.position);

        if(totalDistance < agroDistance && totalDistance > attackDistance){
            ChasePlayer();
            Debug.Log("a");
        }else if(totalDistance < attackDistance){
            StopMoving();
            Attack(); 
            Debug.Log("b");
        }

        if(Mathf.Abs(gnomeRb.velocity.x) > 0.1f){
            animator.SetBool("isRunning", true);
        }else{
            animator.SetBool("isRunning", false);
        }
    }

    private void ChasePlayer(){

        animator.SetBool("isAttacking", false);
        direction = player.transform.position - transform.position;

        if(direction.x < 0){
            Move(-1);
            Debug.Log("d");
        }else if(direction.x > 0){
            Move(1);
            Debug.Log("e");
        }
    }

    private void StopMoving(){
        gnomeRb.velocity = Vector2.zero;
    }

    private void Move(int dir){
        gnomeRb.velocity = new Vector2(gnomeMoveSpeed * dir * Time.deltaTime, gnomeRb.velocity.y);
    }

    private void Attack(){
        animator.SetBool("isAttacking", true);
        swordCollider.enabled = true;
    }

    private void Flip(int dir){
        transform.localScale = new Vector2(dir, transform.localScale.y);
    }
}
