using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{
    [SerializeField] private float reloadTime;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform raycastPoint;
    [SerializeField] private Transform raycastPoint2;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float visionRange;
    [SerializeField] private GameObject enemyBullet;
    private Rigidbody2D bulletRB;
    [SerializeField] private float bulletForce;
    private bool isReloading = false;

    private void Awake() {
        InvokeRepeating("Shoot", 0f, reloadTime);
    }

    private void Update() {
        RaycastHit2D hit = Physics2D.Raycast(raycastPoint.position, transform.right * GetComponent<Transform>().localScale.x, visionRange, layerMask);
        RaycastHit2D hit2 = Physics2D.Raycast(raycastPoint2.position, transform.right * GetComponent<Transform>().localScale.x, visionRange, layerMask);
        
        if(hit){
            if(hit.collider.gameObject.tag == "Player"){
                if(!isReloading){
                    StartCoroutine(Shoot());
                }
            }
        }

        if(hit2){
            if(hit2.collider.gameObject.tag == "Player"){
                if(!isReloading){
                    StartCoroutine(Shoot());  
                }
            }
        }
    }

    IEnumerator Shoot(){
        isReloading = true;
        GameObject bulletCopy = Instantiate(enemyBullet,shootingPoint.position,transform.rotation);
        bulletRB = bulletCopy.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(transform.right * GetComponent<Transform>().localScale.x * bulletForce);

        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }
}
