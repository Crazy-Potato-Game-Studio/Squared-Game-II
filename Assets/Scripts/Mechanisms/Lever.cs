using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lever : MonoBehaviour
{
    private bool playerInRange = false;
    public GameObject whatToTurnOn;
    [SerializeField] private Sprite leverOn;
    [SerializeField] private Sprite leverOff;

    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source; 
    [SerializeField] private bool isOn = false;
    [SerializeField] private Light2D redLight;
    private GameObject player;

    private void Start() {
        if(isOn){
            GetComponent<SpriteRenderer>().sprite = leverOn;
            redLight.enabled = true;
        }else{
            redLight.enabled = false;
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private bool PlayerInRange(){
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if(distance <= 4f){
            playerInRange = true;
        }else{
            playerInRange = false;
        }
        return playerInRange;
    }

    private void Update() {

        if(PlayerInRange() && Input.GetKeyDown(KeyCode.E)){
            if(!isOn){
                whatToTurnOn.GetComponent<Portal>().TurnOn(); 
                isOn = true;
                GetComponent<SpriteRenderer>().sprite = leverOn;
                redLight.enabled = true;
            }else{
                whatToTurnOn.GetComponent<Portal>().TurnOff(); 
                isOn = false;
                GetComponent<SpriteRenderer>().sprite = leverOff;
                redLight.enabled = false;
            }
            source.PlayOneShot(clip);
        }
    }
}
