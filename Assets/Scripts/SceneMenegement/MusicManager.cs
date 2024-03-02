using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioClip currentClip;
    private AudioSource source;
    private bool songBreak = false;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    private void Update() {
        if(!source.isPlaying && !songBreak){
            StartCoroutine("BreakBetweenSongs");
        }
    }

    IEnumerator BreakBetweenSongs(){
        songBreak = true;
        currentClip = source.clip;
        yield return new WaitForSeconds(25f);
        source.PlayOneShot(currentClip);
        songBreak = false;
    }
}
