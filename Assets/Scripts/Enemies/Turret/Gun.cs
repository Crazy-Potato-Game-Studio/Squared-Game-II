using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Gun : MonoBehaviour
{
   [SerializeField] private float reloadTime;
   [SerializeField] private float bulletForce;
   [SerializeField] private GameObject bullet;
   [SerializeField] private float agroDistance;
   [SerializeField] private GameObject shootPoint;
   [SerializeField] private GameObject gunParticles;
   [SerializeField] private AudioClip clip;
   [SerializeField] private AudioSource source;
   [SerializeField] private LineRenderer lineRenderer;
   [SerializeField] private Material green;
   [SerializeField] private Material red;

    private bool isReloading = false;

    private GameObject player;

    private Rigidbody2D bulletRB;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("Shoot", 0f, reloadTime);
    }

    void Update()
    {
        float currentDistance = Vector2.Distance(transform.position, player.transform.position);

        lineRenderer.SetPosition(0, shootPoint.transform.position);
        RaycastHit2D laser = Physics2D.Raycast(shootPoint.transform.position, transform.right);
        if(laser){
            lineRenderer.SetPosition(1, laser.point);
        }else{
            lineRenderer.SetPosition(1, transform.right * 100f);
        }
        

        if(currentDistance <= agroDistance){
            Vector2 raycastDir = player.transform.position - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDir, agroDistance);
            Debug.DrawRay(transform.position, raycastDir, Color.red);

            if(hit){
                
                if(hit.collider.gameObject.tag == "Player"){
                    transform.right = player.transform.position - transform.position;
                    if(!isReloading){
                        StartCoroutine(Shoot());
                    }

                    lineRenderer.material = green;
                }else{
                    lineRenderer.material = red;
                }
            }
        }
    }

    IEnumerator Shoot(){
        isReloading = true;

        source.PlayOneShot(clip);

        GameObject bulletCopy = Instantiate(bullet, shootPoint.transform.position, transform.rotation);
        bulletRB = bulletCopy.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(new Vector2(transform.right.x + RandomAngle(), transform.right.y + RandomAngle()) * bulletForce);

        GameObject particles = Instantiate(gunParticles, shootPoint.transform.position, shootPoint.transform.rotation);
        Destroy(particles, 0.5f);

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
    }

    float RandomAngle(){
        return Random.Range(-0.2f, 0.2f);
    }
}
