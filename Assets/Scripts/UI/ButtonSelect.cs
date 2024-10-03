using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelect : MonoBehaviour
{
    public GameObject arrow;
    public AudioSource source;
    public AudioClip clip;

    private void Start() {
        arrow.SetActive(false);
    }

    public void ShowArrow(){
        if(!UsedDevice.usingGamepad){
            arrow.SetActive(true);
        }
    }

    public void PlaySoundOnMouseHover(){
        if(!UsedDevice.usingGamepad){
            source.PlayOneShot(clip);
        }
    }
}
