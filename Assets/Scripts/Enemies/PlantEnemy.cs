using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemy : MonoBehaviour
{
    private bool playerInRange;
    private BoxCollider2D rangeCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject plantCanvas;

    void Awake()
    {
        if(parent.transform.localScale.x == -1){
            plantCanvas.transform.localScale = new Vector3(plantCanvas.transform.localScale.x * -1,plantCanvas.transform.localScale.y,plantCanvas.transform.localScale.z);
        }
        rangeCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            playerInRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            playerInRange = false;
        }
    }

    void Update()
    {
        animator.SetBool("plantAngry", playerInRange);
    }
}
