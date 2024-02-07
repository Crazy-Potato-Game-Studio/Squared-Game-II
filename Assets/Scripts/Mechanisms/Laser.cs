using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject particles;
    private GameObject endParticles;

    private void Awake() {
        endParticles = Instantiate(particles, transform.position, transform.rotation);
    }

    private void Update(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        lineRenderer.SetPosition(0, shootPoint.position);

        if(hit){
            lineRenderer.SetPosition(1, hit.point);
            endParticles.transform.position = hit.point;

            if(hit.collider.gameObject.tag == "Player"){
                hit.collider.gameObject.GetComponent<HealthManager>().LoseHealth(50f);
            }

        }else{
            lineRenderer.SetPosition(1, transform.up * 100);
            endParticles.transform.position = transform.up * 100;
        }
    }

    
}
