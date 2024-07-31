using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private bool playerInRange;
    private Rigidbody2D rb;
    [SerializeField] private GameObject hint;
    private bool moveUp;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        other.transform.parent = transform;
        playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other){
        other.transform.parent = null;
        playerInRange = false;
    }

    private void Update() {
        if(playerInRange && Input.GetKey(KeyCode.E)){
            Destroy(hint);
            moveUp = true;
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            cam.transform.parent = transform;
            Destroy(cam.GetComponent<CameraFollow>());
        }

        
    }

    private void FixedUpdate() {
        if(moveUp){
           transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime, transform.position.z);
        }
    }
}
