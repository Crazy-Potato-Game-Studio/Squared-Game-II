using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private Slider slider;
    private PlayerInputActions playerInputActions;
    float timeYouEnjoyWastingIsNotWastedTime;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

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

    private void Update() {
        float velocity;
        audioMixer.GetFloat("volume", out velocity);
        float changedVelocity;
        
        if(Time.deltaTime != 0){
            changedVelocity = velocity + playerInputActions.Player.ChangeVolume.ReadValue<float>() * Time.deltaTime * 15;
            timeYouEnjoyWastingIsNotWastedTime = Time.deltaTime;
        }else{
            changedVelocity = velocity + playerInputActions.Player.ChangeVolume.ReadValue<float>() * timeYouEnjoyWastingIsNotWastedTime * 15;
        }
        if(changedVelocity > 10){
            changedVelocity = 10;
        }else if(changedVelocity < -20 && playerInputActions.Player.ChangeVolume.ReadValue<float>() > 0){
            changedVelocity = -20;
        }else if(changedVelocity < -20){
            changedVelocity = -80;
        }
        audioMixer.SetFloat("volume", changedVelocity);

        slider = GetComponent<Slider>();
        slider.value = velocity;
    }
}