using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GoblinChasePlayer : MonoBehaviour
{
    [SerializeField] private Transform rayCast;
    [SerializeField] private LayerMask rayCastMask;
    [SerializeField] private float rayCastLenght;
    [SerializeField] private float attackDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float timer;

    private GameObject target;
    private Rigidbody2D enemyRb;
    private Animator animator;
    private float distance;
    private bool attackMode;
    private bool inRange;
    private bool isCooling;
    private float intTimer;
    private bool isTooClose = false;

    private void Awake() {
        intTimer = timer;
        enemyRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player"); 
    }
    
    private void Update() {

        if(inRange){
            GnomeLogic();
        }else{
            inRange = false;
        }

        if(inRange == false){
            animator.SetBool("canWalk", false);
            StopAttack();
        }

        CheckIfInRange();
    }
    
    private void CheckIfInRange(){

        float rangeDistance = Vector2.Distance(transform.position, target.transform.position);

        if(rangeDistance < 6){
            inRange = true;
        }
    }

    void GnomeLogic(){
        distance = Vector2.Distance(transform.position, target.transform.position);

        if(distance > attackDistance && inRange){
            Move();
            StopAttack();
        }else if(attackDistance >= distance && isCooling == false){
            Attack();
        }

        if(isCooling){
            animator.SetBool("Attack", false);
        }
    }

    void Move(){
        animator.SetBool("canWalk", true);
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("GnomeAttack")){
            Vector2 anotherDistance = transform.position - target.transform.position;

            if(transform.position.x - target.transform.position.x < 1f){
                isTooClose = true;
            }else{
                isTooClose = false;
            }

            enemyRb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, enemyRb.velocity.y);

            if(anotherDistance.x > 0 && !isTooClose){
                transform.localScale = new Vector2(-1, transform.localScale.y);
                moveSpeed = -220;
            }else{
                transform.localScale = new Vector2(1, transform.localScale.y);
                moveSpeed = 220;
            }  
        }
    }

    void Attack(){

        if(transform.position.x - target.transform.position.x < 1f){
            StopAttack();
            isTooClose = true;
        }else{
            attackMode = true;

            animator.SetBool("canWalk", false);
            animator.SetBool("Attack", true);

            timer = intTimer;
            isTooClose = false;
        }
    }

    void StopAttack(){
        isCooling = false;
        attackMode = false;
        animator.SetBool("Attack", false);
    }
}
