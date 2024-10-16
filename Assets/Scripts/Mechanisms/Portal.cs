using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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
    private PlayerInputActions playerInputActions;
    private bool keyPressed;
    
    private void Awake() {

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

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
            if(playerInRange && playerInputActions.Player.Interactions.ReadValue<float>() == 1 && !keyPressed){
                keyPressed = true;
                destinationPortal.GetComponent<Portal>().keyPressed = true;
                TeleportPlayer();
            }
            arrow.SetActive(true);
        }else{
            arrow.SetActive(false);
        }

        if(playerInputActions.Player.Interactions.ReadValue<float>() == 0){
            keyPressed = false;
            destinationPortal.GetComponent<Portal>().keyPressed = false;
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
        Debug.Log(currentParticles);
        Destroy(currentParticles, 2f);
        GameObject currentParticles2 = Instantiate(portalParticles, destinationPortal.transform);
        Destroy(currentParticles2, 2f);
    }

}
