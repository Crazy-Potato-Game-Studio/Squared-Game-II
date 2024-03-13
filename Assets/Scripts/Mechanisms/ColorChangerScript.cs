using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerScript : MonoBehaviour
{
    private Color color;
    [SerializeField] private Material material;
    [SerializeField] private GameObject thisColorDoors;
    private GameObject[] colorDoors;
    private AudioSource source;
    [SerializeField] private AudioClip clip;

    private void Awake() {
        color = material.GetColor("_BaseColor");
        colorDoors = GameObject.FindGameObjectsWithTag("colorDoors");
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.GetComponent<ChangeColor>().PlayerChangeColor(color);
            for(int i = 0; i < colorDoors.Length; i++){
                colorDoors[i].GetComponent<Collider2D>().isTrigger = false;
            }
            if(thisColorDoors != null){
                thisColorDoors.GetComponent<Collider2D>().isTrigger = true;
            }
            source.PlayOneShot(clip);
        }
    }
}
