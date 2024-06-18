using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] public Transform shootPoint;
    [SerializeField] private GameObject particles;
    private GameObject endParticles;
    Collider2D lastTouchedMirror;
    public bool endParticlesInstantiated;

    private void Update(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        lineRenderer.SetPosition(0, transform.position + shootPoint.position);

        if(hit){
            if(!endParticlesInstantiated)InstantiateEndParticles();
            lineRenderer.SetPosition(1, hit.point);
            if(endParticles)endParticles.transform.position = hit.point;

            if(hit.collider.gameObject.tag == "Player"){
                hit.collider.gameObject.GetComponent<HealthManager>().LoseHealth(50f, 3f);
            }

            if(hit.collider.gameObject.tag == "Mirror"){
                lastTouchedMirror = hit.collider;
                lastTouchedMirror.gameObject.GetComponent<Mirror>().SwitchLaser(true);
                //lastTouchedMirror.gameObject.GetComponent<Mirror>().trans = hit.point;
            }else{
                if(lastTouchedMirror){
                    lastTouchedMirror.gameObject.GetComponent<Mirror>().SwitchLaser(false);
                }
            }

        }else{
            lineRenderer.SetPosition(1, transform.up * 100);
            if(endParticles)endParticles.transform.position = transform.up * 100;
        }
    }

    private void InstantiateEndParticles(){
        endParticles = Instantiate(particles, transform.position, transform.rotation);
        endParticlesInstantiated = true;
    }
    
}
