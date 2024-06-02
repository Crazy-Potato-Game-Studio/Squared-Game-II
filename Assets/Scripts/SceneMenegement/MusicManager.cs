using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource source;
    private GameObject soundtrackName;
    private string currentSong;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    public void SceneChange(){
        soundtrackName = GameObject.FindGameObjectWithTag("SoundtrackName");
        if(soundtrackName){
            AudioClip song = soundtrackName.GetComponent<SoundtrackNameScript>().soundtrackName;
            if(song.name != currentSong){
                PlayNewSong(song);
            }
        }
        
    }

    public void PlayNewSong(AudioClip song){
        source.clip = song;
        source.Play();
        currentSong = song.name;
    }
}
