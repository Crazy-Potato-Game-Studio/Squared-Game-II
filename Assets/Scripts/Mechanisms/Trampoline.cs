using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube"){
            source.PlayOneShot(clip);
        }
    }
}
