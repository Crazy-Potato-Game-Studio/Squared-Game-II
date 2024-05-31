using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Blindness : MonoBehaviour
{
    [SerializeField] private VolumeProfile globalVolume;
    private Vignette vignette;
    private GameObject blindnessIcon;
    [SerializeField] private float vignetteInitValue;

    private void Start() {
        globalVolume.TryGet<Vignette>(out vignette);
        SetVignette(vignetteInitValue);
        blindnessIcon = GameObject.FindGameObjectWithTag("Blindness");
        blindnessIcon.SetActive(false);
    }

    public void StartBlindness(){
        SetVignette(1);
        blindnessIcon.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(StopBlindness());
    }

    IEnumerator StopBlindness(){
        yield return new WaitForSeconds(5f);
        SetVignette(vignetteInitValue);
        blindnessIcon.SetActive(false);
    }

    private void SetVignette(float vignetteValue){
        vignette.intensity.value = vignetteValue;
    }
}
