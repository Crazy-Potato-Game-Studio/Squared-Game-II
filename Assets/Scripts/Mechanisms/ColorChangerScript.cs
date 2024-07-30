using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerScript : MonoBehaviour
{
    private Color color;
    public string changerColor;
    [SerializeField] private Material material;
    private GameObject[] colorDoors;
    private AudioSource source;
    [SerializeField] private AudioClip clip;
    List<GameObject> sameColorDoors = new List<GameObject>();

    private void Awake() {
        color = material.GetColor("_BaseColor");
        colorDoors = GameObject.FindGameObjectsWithTag("colorDoors");
        source = GetComponent<AudioSource>();

        for(int i = 0; i < colorDoors.Length; i++){
            if(colorDoors[i].GetComponent<ColorDoors>().color == changerColor){
                addDoors(colorDoors[i]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.GetComponent<ChangeColor>().PlayerChangeColor(color);

            for(int i = 0; i < colorDoors.Length; i++){
                colorDoors[i].GetComponent<Collider2D>().isTrigger = false;
            }

            for(int i = 0; i < sameColorDoors.Count; i++){
                sameColorDoors[i].GetComponent<Collider2D>().isTrigger = true;
            }
            
            source.PlayOneShot(clip);
        }
    }

    void addDoors(GameObject doors)
    {
        sameColorDoors.Add(doors);
    }
}
