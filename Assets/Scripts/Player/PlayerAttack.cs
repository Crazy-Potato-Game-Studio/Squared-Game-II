using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    int playerAttackDamage = 20;
    private Animator animator;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            other.gameObject.GetComponent<EnemyHealth>().LoseHP(playerAttackDamage);
        }
    }

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            animator.SetBool("isAttacking", true);
        }else{
            animator.SetBool("isAttacking", false);
        }
    }
}
