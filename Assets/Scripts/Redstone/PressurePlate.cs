using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private float initPosition;
    private bool moveBack;
    [SerializeField] private Light2D redLight;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    private void Start() {
        initPosition = transform.position.y;
        redLight.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube"){
            other.transform.parent = transform;
            redLight.enabled = true;
            if(transform.childCount < 2){
                source.PlayOneShot(clip);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube"){

            other.transform.parent = transform;
            redLight.enabled = true;

            if(initPosition - transform.position.y >= 0.1f){
                
            }
            else{
                transform.position = transform.position + new Vector3(0, -0.006f, 0);
            }
            moveBack = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube"){
            redLight.enabled = false;
            moveBack = true;
            if(transform.childCount < 2){
                source.PlayOneShot(clip);
            }
            other.transform.parent = null;
        }
    }

    private void Update() {
        if(moveBack){
            if(transform.position.y < initPosition){
                transform.position = transform.position + new Vector3(0, +0.006f, 0);
            }else{
                moveBack = false;
            }
        }
    }
}
