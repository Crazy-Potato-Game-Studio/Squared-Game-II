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
            cam.GetComponent<CameraFollow>().player = transform;
        }

        if(moveUp){
            rb.velocity = new Vector2(0,1f * Time.deltaTime * 250);
        }
    }

    private void FixedUpdate() {
        
    }
}
