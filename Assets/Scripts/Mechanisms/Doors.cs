using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Doors : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerColor;
    private bool doorsOpen = false;
    private GameObject player;

    private void Awake() {
        SetDoorsBool();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OpenDoors(){
        doorsOpen = true;
        SetDoorsBool();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && playerColor.color == new Color(0,1f,0f,1f)){
            Debug.Log("Player is green");
        }
    }

    // void CloseDoors(){
    //     doorsOpen = false;
    //     SetDoorsBool();
    // }

    void SetDoorsBool(){
        animator.SetBool("DoorsOpen", doorsOpen);
    }

    private void Update() {
        
    }

}
    
