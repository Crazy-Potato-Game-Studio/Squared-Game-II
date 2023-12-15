using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlueSlime : MonoBehaviour
{
    [SerializeField] private float agroDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform raycastPoint;
    [SerializeField] private float raycastRadius;
 
    private Rigidbody2D enemyRb;
    
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        RaycastHit2D hitRight = Physics2D.CircleCast(new Vector2(raycastPoint.position.x, raycastPoint.position.y), raycastRadius, transform.right, agroDistance, layerMask);
        RaycastHit2D hitLeft = Physics2D.CircleCast(new Vector2(raycastPoint.position.x, raycastPoint.position.y), raycastRadius, transform.right * -1, agroDistance, layerMask);
        RaycastHit2D hitTop = Physics2D.CircleCast(new Vector2(raycastPoint.position.x, raycastPoint.position.y), raycastRadius * 2, transform.up, 1, layerMask);

        if(hitRight || hitLeft || hitTop){  

            if(hitLeft){
                if(hitLeft.collider.gameObject.tag == "Player"){
                    ChasePlayer(-1);
                }
            }else{
                SetAnimatorBool(false);
            }

            if(hitRight){
                if(hitRight.collider.gameObject.tag == "Player"){
                    ChasePlayer(1);
                }
            }else{
                SetAnimatorBool(false);
            }

            if(hitTop){
                if(hitTop.collider.gameObject.tag == "Player"){
                    SetAnimatorBool(true);
                }
            }else{
                SetAnimatorBool(false);
            }
            
        }else{
            SetAnimatorBool(false);
        }
    }

    void ChasePlayer(int right){
        SetAnimatorBool(true);

        transform.localScale = new Vector2(right, transform.localScale.y);
        enemyRb.velocity = new Vector2(right * moveSpeed, enemyRb.velocity.y);
    }

    private void SetAnimatorBool(bool isChasingPlayer){
        animator.SetBool("isChasingPlayer", isChasingPlayer);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(raycastPoint.position, raycastRadius);
    }
}
