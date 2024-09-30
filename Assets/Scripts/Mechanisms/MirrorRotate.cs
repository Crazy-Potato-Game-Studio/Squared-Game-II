using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRotate : MonoBehaviour
{
    private bool canRotate;
    private GameObject player;
    [SerializeField] private GameObject mirror;
    public bool rotateLeft = false;
    public bool rotateRight = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" || other.tag == "ResistanceCollider"){
            player = other.gameObject;
            player.GetComponent<PlayerRotateMirror>().mirror = gameObject;
            canRotate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player" || other.tag == "ResistanceCollider"){
            player.GetComponent<PlayerRotateMirror>().mirror = null;
            canRotate = false;
        }
    }

    void FixedUpdate()
    {
        if(canRotate){
            if(rotateLeft){
                mirror.transform.Rotate(0, 0, 20f * Time.deltaTime);
            }
            if(rotateRight){
                mirror.transform.Rotate(0, 0, -20f * Time.deltaTime);
            }
        }
    }
}
