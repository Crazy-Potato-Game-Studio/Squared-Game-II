using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Doors : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private SpriteRenderer playerColor;
    private bool doorsOpen = false;
    [SerializeField] private Color color;
    private AudioSource source;
    [SerializeField] private AudioClip clip;

    private void Awake() {
        SetDoorsBool();
        source = GetComponent<AudioSource>();
        playerColor = GameObject.Find("PlayerGFX").GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && playerColor.color == color){
            OpenDoors();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && playerColor.color == color){
            CloseDoors();
        }
    }

    void OpenDoors(){
        doorsOpen = true;
        SetDoorsBool();
        PlayDoorsSound();
    }

    void CloseDoors(){
        doorsOpen = false;
        SetDoorsBool();
        PlayDoorsSound();
    }

    void PlayDoorsSound(){
        source.PlayOneShot(clip);
    }

    void SetDoorsBool(){
        animator.SetBool("DoorsOpen", doorsOpen);
    }
}
    
