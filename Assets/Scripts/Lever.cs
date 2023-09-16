using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject hint;
    private bool playerInRange = false;
    public GameObject whatToTurnOn;
    [SerializeField] private Sprite leverOn;
    [SerializeField] private Sprite leverOff;

    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source; 
    [SerializeField] private bool isOn = false;
    [SerializeField] private Light2D redLight;

    private void Start() {

        if(isOn){
            GetComponent<SpriteRenderer>().sprite = leverOn;
            redLight.enabled = true;
        }else{
            redLight.enabled = false;
        }
        hint.GetComponent<SpriteRenderer>().enabled = false;
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            hint.GetComponent<SpriteRenderer>().enabled = true;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            hint.GetComponent<SpriteRenderer>().enabled = false;
            playerInRange = false;
        }
    }

    private void Update() {

        if(playerInRange && Input.GetKeyDown(KeyCode.E)){
            if(!isOn){
                whatToTurnOn.GetComponent<Portal>().TurnOn(); // Na razie tylko portale
                isOn = true;
                GetComponent<SpriteRenderer>().sprite = leverOn;
                redLight.enabled = true;
            }else{
                whatToTurnOn.GetComponent<Portal>().TurnOff(); // Na razie tylko portale
                isOn = false;
                GetComponent<SpriteRenderer>().sprite = leverOff;
                redLight.enabled = false;
            }
            source.PlayOneShot(clip);
        }
    }
}
