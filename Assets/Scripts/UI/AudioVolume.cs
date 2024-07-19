using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetAudioVolume(float volume){
        if(volume <= -20){
            audioMixer.SetFloat("volume", -80);
        }else{
            audioMixer.SetFloat("volume", volume);
        }
        
    }
}
