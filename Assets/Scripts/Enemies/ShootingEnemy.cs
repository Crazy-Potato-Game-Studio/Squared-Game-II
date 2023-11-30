using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
   [SerializeField] private float enemyReload;
   [SerializeField] private float bulletForce;
   [SerializeField] private GameObject bullet;
   [SerializeField] private float distanceHit;

     private bool isVisible = false;

   private GameObject player;


   private Rigidbody2D bulletRB;

    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("Shoot", 0f, enemyReload);
    }

    
    void Update()
    {
        
        transform.right = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, distanceHit);
        if(hit != false){
            if(hit.collider.gameObject.tag == "Player"){
                isVisible = true;
            }else{
                isVisible = false;
            }
        }
        
        Debug.DrawRay(transform.position, Vector2.right*distanceHit, Color.red);
    }

    
    
    void Shoot(){
        if(isVisible){
            GameObject bulletCopy = Instantiate(bullet,transform.position,transform.rotation);
            bulletRB = bulletCopy.GetComponent<Rigidbody2D>();
            bulletRB.AddForce(transform.right*bulletForce);
        }
        
    }
}
