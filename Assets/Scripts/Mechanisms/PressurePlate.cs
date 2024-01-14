using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PressurePlate : MonoBehaviour
{
    private float initPosition;
    private bool moveBack;
    [SerializeField] private GameObject whatToTurnOn;
    [SerializeField] private SpriteRenderer redLight;
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
            if(whatToTurnOn.GetComponent<Portal>() != null){
                whatToTurnOn.GetComponent<Portal>().TurnOn();
            }else{
                whatToTurnOn.GetComponent<NieTak>().isOn = true;
            }
            
            if(transform.childCount < 3){
                source.PlayOneShot(clip);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube"){

            other.transform.parent = transform;
            redLight.enabled = true;
            if(whatToTurnOn.GetComponent<Portal>() != null){
                whatToTurnOn.GetComponent<Portal>().TurnOn();
            }else{
                whatToTurnOn.GetComponent<NieTak>().isOn = true;
            }
            if(initPosition - transform.position.y <= 0.1f){
                transform.position = transform.position + new Vector3(0, -0.006f, 0);
            }
            moveBack = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube"){
            redLight.enabled = false;
            if(whatToTurnOn.GetComponent<Portal>() != null){
                whatToTurnOn.GetComponent<Portal>().TurnOff();
            }else{
                whatToTurnOn.GetComponent<NieTak>().isOn = false;
            }
            moveBack = true;
            if(transform.childCount < 3){
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
