using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NieTak : MonoBehaviour
{
    public bool isOn;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;

    void Update()
    {
        if(isOn){
            videoPlayer.enabled = true;
            rawImage.enabled = true;
            videoPlayer.Play();
        }else{
            videoPlayer.enabled = false;
            rawImage.enabled = false;
            videoPlayer.Stop();
        }
    }
}
