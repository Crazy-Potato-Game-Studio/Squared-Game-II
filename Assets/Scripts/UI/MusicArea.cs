using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicArea : MonoBehaviour
{
    [SerializeField] private GameObject musicManager;
    [SerializeField] private AudioClip music;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            musicManager.GetComponent<MusicManager>().StartPlayingMusic(music);
            musicManager.GetComponent<MusicManager>().isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            musicManager.GetComponent<MusicManager>().StopPlayingMusic();
            musicManager.GetComponent<MusicManager>().isInside = false;
        }
    }
}
