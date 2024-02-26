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
    [SerializeField] private Light2D portalLight;
    [SerializeField] private GameObject trailPrefab;
    [SerializeField] private GameObject portalParticles;
    private AudioSource source;
    [SerializeField] private GameObject arrow;
    
    private void Awake() {

        if(isOn){
            TurnOn();
        }else{
            TurnOff();
        }

        arrow.transform.right = destinationPortal.transform.position - transform.position;

        source = GetComponent<AudioSource>();

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
                TeleportPlayer();
            }
        }
    }

    public void TurnOn(){
        isOn = true;
        arrow.SetActive(true);
        portalLight.enabled = true;
    }

    public void TurnOff(){
        isOn = false;
        arrow.SetActive(false);
        portalLight.enabled = false;
    }

    private void TeleportPlayer(){
        Destroy(player.GetComponentInChildren<TrailRenderer>().gameObject);
        player.transform.position = destinationPortal.transform.position;
        Instantiate(trailPrefab, player.transform);
        source.Play(0);
        GameObject currentParticles = Instantiate(portalParticles, transform);
        Destroy(currentParticles, 2f);
        GameObject currentParticles2 = Instantiate(portalParticles, destinationPortal.transform);
        Destroy(currentParticles2, 2f);
    }



}
