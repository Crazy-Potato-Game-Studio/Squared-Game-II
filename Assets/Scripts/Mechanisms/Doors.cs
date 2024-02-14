using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Doors : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public bool doorsOpen = false;
    private AudioSource source;
    [SerializeField] private AudioClip clip;

    private void Awake() {
        SetDoorsBool();
        source = GetComponent<AudioSource>();
    }

    public void OpenDoors(){
        doorsOpen = true;
        SetDoorsBool();
        PlayDoorsSound();
    }

    public void CloseDoors(){
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
    
