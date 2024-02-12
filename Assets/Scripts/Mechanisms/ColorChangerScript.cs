using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerScript : MonoBehaviour
{
    private Color color;
    [SerializeField] private Material material;

    private void Awake() {
        color = material.GetColor("_BaseColor");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.GetComponent<ChangeColor>().PlayerChangeColor(color);
        }
    }
}
