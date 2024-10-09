using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private bool playerInRange;
    private Rigidbody2D rb;
    [SerializeField] private GameObject hint;
    private bool moveUp = false;
    private PlayerInputActions playerInputActions;
    private GameObject player;
    public GameObject theEnd;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.transform.parent = transform;
            playerInRange = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        other.transform.parent = null;
        playerInRange = false;
    }

    private void Update() {
        if(playerInRange && playerInputActions.Player.Interactions.ReadValue<float>() != 0 && !moveUp){
            Destroy(hint);
            moveUp = true;
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            cam.transform.parent = transform;
            Destroy(cam.GetComponent<CameraFollow>());
            Destroy(player.GetComponent<PlayerMovement>());
            theEnd.GetComponent<FinalScene>().StartFading();
        }  
    }

    private void FixedUpdate() {
        if(moveUp){
           transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime, transform.position.z);
        }
    }

}
