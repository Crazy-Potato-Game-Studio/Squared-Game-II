using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TheSeekerBoss : MonoBehaviour
{
    [SerializeField] private AIPath aiPath;
    [SerializeField] private Transform returnPoint;
    private GameObject player;
    private bool canShoot;
    [SerializeField] private float canShootDistance;
    [SerializeField] private GameObject acidBullet;
    [SerializeField] private LayerMask seeLayers;
    private Color color; 

    [SerializeField] int currentPosition;

    [SerializeField] private GameObject doors;

    private bool fadeIn;
    private bool fadeOut;

    Vector2[] array = {new Vector2(-18, -12), new Vector2(0, -12), new Vector2(-9.5f, -3.5f), new Vector2(-9.5f, -11.5f)};

    private void Awake() {
        GetComponent<AIDestinationSetter>().target = returnPoint;
        InvokeRepeating("Shoot", 0, 0.05f);
        InvokeRepeating("StartTeleport", 5, Random.Range(7, 13));
        color = GetComponent<SpriteRenderer>().color;
    }

    private void StartTeleport(){
        fadeOut = true;
    }

    private void Teleport(){
        int nextPosition;
        nextPosition = Random.Range(0, 4);
        while (nextPosition == currentPosition)
        {
            nextPosition = Random.Range(0, 4);
        }
        transform.position = array[nextPosition];
        currentPosition = nextPosition;
        fadeIn = true;
    }

    void Update(){
        if(player){

            GetComponent<AIDestinationSetter>().target = player.transform;

            if(Vector2.Distance(transform.position, player.transform.position) <= canShootDistance + 1){
                
                Vector2 dir = player.transform.position - transform.position;

                RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, seeLayers);

                if(ray.collider.CompareTag("Player") || ray.collider.CompareTag("ResistanceCollider")){
                    canShoot = true;
                }else{
                    canShoot = false;
                }
            }else{
                canShoot = false;
            }
        }else{
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if(player && aiPath.desiredVelocity.x >= 0.01f){
            transform.localScale = new Vector2(-1f, 1f);
        }else if(player && aiPath.desiredVelocity.x <= 0.01f){
            transform.localScale = new Vector2(1f, 1f);
        }

        if(player && aiPath.desiredVelocity.x == 0){
            if(player.transform.position.x - transform.position.x > 0){
                transform.localScale = new Vector2(-1f, 1f);
            }else{
                transform.localScale = new Vector2(1f, 1f);
            }
        }

        if(fadeIn){
            color.a += Time.deltaTime;
            
            GetComponent<SpriteRenderer>().color = color;

            if(color.a >= 1){
                fadeIn = false;
            }
        }

        if(fadeOut){
            color.a -= Time.deltaTime;
            
            GetComponent<SpriteRenderer>().color = color;

            if(color.a <= 0){
                fadeOut = false;
                Teleport();
            }
        }
    }

    private void Shoot(){
        if(canShoot && player){
            GameObject currentBullet = Instantiate(acidBullet, transform.position, Quaternion.identity);
            if(player && currentBullet){
                currentBullet.transform.right = player.transform.position - transform.position;
            }
            Rigidbody2D rb = currentBullet.GetComponent<Rigidbody2D>();
            rb.AddForce(currentBullet.transform.right * 1.5f, ForceMode2D.Impulse);
        }  
    }

    public void OpenTheDoors(){
        doors.GetComponent<Doors>().OpenDoors();
    }
}

