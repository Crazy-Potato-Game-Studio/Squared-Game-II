using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.Mathematics;

public class AcidBat : MonoBehaviour
{
    [SerializeField] private AIPath aiPath;
    [SerializeField] private Transform returnPoint;
    [SerializeField] private GameObject enemyCanvas;
    private GameObject player;
    private float distance;
    private bool canShoot;
    [SerializeField] private float canShootDistance;
    [SerializeField] private GameObject acidBullet;
    private bool hasLineOfSight = false;
    [SerializeField] private LayerMask seeLayers;

    private void Awake() {
        GetComponent<AIDestinationSetter>().target = returnPoint;
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<AIPath>().endReachedDistance = canShootDistance;
        InvokeRepeating("Shoot", 0, 0.05f);
    }

    void Update(){
        if(player){
            distance = Vector2.Distance(transform.position, player.transform.position);

            if(distance < 15f){
                GetComponent<AIDestinationSetter>().target = player.transform;
            }else if(distance > 15f ){
                GetComponent<AIDestinationSetter>().target = returnPoint;
            }

            if(Vector2.Distance(transform.position, player.transform.position) <= canShootDistance + 1){
                
                Vector2 dir = player.transform.position - transform.position;

                RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, seeLayers);

                if(ray.collider.CompareTag("Player") || ray.collider.CompareTag("ResistanceCollider")){
                    canShoot = true;
                }else{
                    canShoot = false;
                    GetComponent<AIPath>().endReachedDistance = 0.5f;
                }
            }
        }

        if(aiPath.desiredVelocity.x >= 0.01f){
            transform.localScale = new Vector2(-1f, 1f);
            enemyCanvas.GetComponent<ScaleChanger>().ChangeUIScale(transform.localScale.x);
        }else if(aiPath.desiredVelocity.x <= 0.01f){
            transform.localScale = new Vector2(1f, 1f);
            enemyCanvas.GetComponent<ScaleChanger>().ChangeUIScale(transform.localScale.x);
        }

        if(player && aiPath.desiredVelocity.x == 0){
            if(player.transform.position.x - transform.position.x > 0){
                transform.localScale = new Vector2(-1f, 1f);
                enemyCanvas.GetComponent<ScaleChanger>().ChangeUIScale(transform.localScale.x);
            }else{
                transform.localScale = new Vector2(1f, 1f);
                enemyCanvas.GetComponent<ScaleChanger>().ChangeUIScale(transform.localScale.x);
            }
        }

    }

    private void Shoot(){
        if(canShoot){
            GameObject currentBullet = Instantiate(acidBullet, transform.position, Quaternion.identity);
            if(currentBullet){
                currentBullet.transform.right = player.transform.position - transform.position;
            }
            Rigidbody2D rb = currentBullet.GetComponent<Rigidbody2D>();
            rb.AddForce(currentBullet.transform.right * 1.5f, ForceMode2D.Impulse);
        }  
    }
}
