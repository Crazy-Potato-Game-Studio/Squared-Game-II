using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetAudioVolume(float volume){
        audioMixer.SetFloat("volume", volume);
    }
}
