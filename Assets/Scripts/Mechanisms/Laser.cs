using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int reflections = 5;
    [SerializeField] private int maxLength;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject particles;

    private Ray2D ray;
    private RaycastHit2D hit;
    private bool particlesInstantiated = false;
    private GameObject endParticles;
    private int detectorId;
    private bool hitDetector;
    GameObject laserDetector = null;

    private void Update() {
        
        if(!particlesInstantiated){
            endParticles = Instantiate(particles, transform.position, transform.rotation);
            particlesInstantiated = true;
        }

        ray = new Ray2D(transform.position, transform.up);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remainingLength = maxLength;

        for(int i = 0; i < reflections; i++){

            hit = Physics2D.Raycast(ray.origin, ray.direction, remainingLength, layerMask);

            if(hit){
                lineRenderer.positionCount += 1;

                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                remainingLength -= Vector2.Distance(ray.origin, hit.point);

                Vector2 updatedDirection = Vector2.Reflect(ray.direction, hit.normal);

                ray = new Ray2D(hit.point + updatedDirection * 0.01f, updatedDirection);

                endParticles.transform.position = lineRenderer.GetPosition(lineRenderer.positionCount-1);

                if(hit.collider.tag == "Mirror"){
                    hit.collider.GetComponent<Mirror>().ChangeMaterial();
                }
                
                if(hit.collider.tag != "Mirror"){
                    break;
                }
  
            }else{
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);

            }

        }

        if(hit.collider.tag == "LaserDetector" && !hitDetector){
            hitDetector = true;
            detectorId = hit.collider.gameObject.GetInstanceID();
            laserDetector = hit.collider.gameObject;
            laserDetector.GetComponent<LaserDetector>().TurnObjects();
        }

        if(hit.collider.gameObject.GetInstanceID() != detectorId && hitDetector){
            hitDetector = false;
            laserDetector.GetComponent<LaserDetector>().TurnObjects();
        }
        


        DealDamage(hit);
    }

    private void DealDamage(RaycastHit2D hit){
        if(hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "ResistanceCollider"){
            if(hit.collider.gameObject.tag == "ResistanceCollider"){
                hit.collider.gameObject.transform.parent.GetComponent<HealthManager>().LoseHealth(100f, 0f);
            }else{
                hit.collider.gameObject.GetComponent<HealthManager>().LoseHealth(100f, 0f);
            }
        }
    }


    
}
