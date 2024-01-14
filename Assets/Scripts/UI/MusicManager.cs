using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip menu;
    [SerializeField] private AudioSource source;
    public bool isInside;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void StopPlayingMusic(){
       // source.enabled = false;
    }

    public void StartPlayingMusic(AudioClip clip){
        source.Stop();
        source.clip = clip;
        source.PlayOneShot(clip);

        
    }

    private void Update() {
        if(isInside){
            source.volume += Time.deltaTime / 5;
            if(source.volume >= 0.7f){
                source.volume = 0.7f;
            }
        }else{
            source.volume -= Time.deltaTime / 5;
            if(source.volume <= 0){
                source.volume = 0;
            }
        }
    }
}
