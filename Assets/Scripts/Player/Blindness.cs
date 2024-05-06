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

    private void Start() {
        globalVolume.TryGet<Vignette>(out vignette);
        vignette.active = false;
        blindnessIcon = GameObject.FindGameObjectWithTag("Blindness");
        blindnessIcon.SetActive(false);
    }

    public void StartBlindness(){
        vignette.active = true;
        blindnessIcon.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(StopBlindness());
    }

    IEnumerator StopBlindness(){
        yield return new WaitForSeconds(5f);
        vignette.active = false;
        blindnessIcon.SetActive(false);
    }
}
