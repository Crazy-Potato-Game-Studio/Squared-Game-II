using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private float initPosition;
    private bool moveBack;
    [SerializeField] private GameObject whatToTurnOn;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    private void Awake() {
        initPosition = transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube"){
            other.transform.parent = transform;

            TurnObjectOn();
            PlaySound();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube"){

            other.transform.parent = transform;

            if(initPosition - transform.position.y <= 0.1f){
                transform.position = transform.position + new Vector3(0, -0.006f, 0);
            }

            moveBack = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube"){  
            
            other.transform.parent = null;
            
            if(transform.childCount == 0){
                TurnObjectOff();
                moveBack = true;
                PlaySound();
            }
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

    private void PlaySound(){
        if(transform.childCount < 2){
            source.PlayOneShot(clip);
        }
    }

    private void TurnObjectOn(){
        if(whatToTurnOn.GetComponent<Portal>() != null){
            whatToTurnOn.GetComponent<Portal>().TurnOn();
        }else if(whatToTurnOn.GetComponent<Doors>() != null){
            whatToTurnOn.GetComponent<Doors>().OpenDoors();
        }
    }

    private void TurnObjectOff(){
        if(whatToTurnOn.GetComponent<Portal>() != null){
            whatToTurnOn.GetComponent<Portal>().TurnOff();
        }else if(whatToTurnOn.GetComponent<Doors>() != null){
            whatToTurnOn.GetComponent<Doors>().CloseDoors();
        }
    }
}
