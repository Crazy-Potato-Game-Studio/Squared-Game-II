using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public GameObject destinationPortal;
    private GameObject[] portals;
    public string portalColor;
    public bool isOn;
    private bool playerInRange = false;
    private GameObject player;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D portalLight;
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

        portals = GameObject.FindGameObjectsWithTag("Portal");
        for (int i = 0; i < portals.Length; i++)
        {
            if(portals[i].GetComponent<Portal>().portalColor == portalColor){
                if(portals[i].name != name){
                    destinationPortal = portals[i];
                }
            }
        }

        arrow.transform.right = destinationPortal.transform.position - transform.position;

        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(isOn && other.gameObject.tag == "Player"){
            player = other.gameObject;
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
            if(playerInRange && player.GetComponent<TurnPortalOn>().playerPressedE && !player.GetComponent<TurnPortalOn>().playerHasTeleported){
                TeleportPlayer();
                player.GetComponent<TurnPortalOn>().playerHasTeleported = true;
            }
            arrow.SetActive(true);
        }else{
            arrow.SetActive(false);
        }

    }

    public void TurnOn(){
        isOn = true;
        
        portalLight.enabled = true;
    }

    public void TurnOff(){
        isOn = false;
        
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
