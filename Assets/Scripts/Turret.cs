using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public float turretRange;
    public Transform target;
    [SerializeField] bool enemyDetected;
    Vector2 direction; 
    [SerializeField] float fireRate;
    [SerializeField] float fireForce;
    [SerializeField] GameObject bullet;

    public AudioClip shootArrow;
    public AudioSource source;

    float nextTime;
    public LayerMask ignoreLayers;

    public GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in enemies)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    } // nie mam pojęcia co to ale było w unity docs


    void Update()
    {
        target = FindClosestEnemy().transform;

        Vector2 targetPosition = target.position;

        direction = targetPosition - (Vector2)transform.position;

        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, direction, turretRange, ~ignoreLayers);

        if(rayInfo){
            if(rayInfo.collider.gameObject.tag == "Enemy"){
                if(enemyDetected == false) {
                    enemyDetected = true;
                }
            }else{
                if(enemyDetected == true){
                    enemyDetected = false;
                }
            }
        }
        if(enemyDetected){
            transform.up = direction;
            if(Time.time > nextTime){
                nextTime = Time.time+1/fireRate;
                Shoot();
            }
        }

    }

    void Shoot(){
        source.PlayOneShot(shootArrow);
        GameObject bulletIns = Instantiate(bullet, transform.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * fireForce;
        float rotation = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        bulletIns.transform.rotation = Quaternion.Euler(0,0,rotation);
    }
    

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, turretRange);
    }


}
