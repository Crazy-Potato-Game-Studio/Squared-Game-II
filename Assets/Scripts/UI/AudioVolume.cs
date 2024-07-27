using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
        float velocity;
        audioMixer.GetFloat("volume", out velocity);    

        slider.value = velocity;
    }

    public void SetAudioVolume(float volume){
        if(volume <= -20){
            audioMixer.SetFloat("volume", -80);
        }else{
            audioMixer.SetFloat("volume", volume);
        }
        
    }
}