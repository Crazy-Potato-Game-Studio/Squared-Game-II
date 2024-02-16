using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Portal : MonoBehaviour
{

    public GameObject destinationPortal;
    public bool isOn;
    private bool playerInRange = false;
    private GameObject player;

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private SpriteRenderer portalTop;
    [SerializeField] private Light2D portalLight;
    
    private void Start() {

        if(isOn){
            TurnOn();
        }else{
            TurnOff();
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(isOn && other.gameObject.tag == "Player"){
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            playerInRange = false;
        }
    }

    private void Update() {
        if(isOn && destinationPortal.GetComponent<Portal>().isOn){
            if(playerInRange && Input.GetKeyDown(KeyCode.E)){
                player.transform.position = destinationPortal.transform.position;
            }
        }
    }

    public void TurnOn(){
        isOn = true;
        particles.Play();
        portalTop.enabled = true;
        portalLight.enabled = true;
    }

    public void TurnOff(){
        isOn = false;
        particles.Stop();
        portalTop.enabled = false;
        portalLight.enabled = false;
    }

}
