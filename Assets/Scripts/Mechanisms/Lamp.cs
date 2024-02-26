using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private Material lampOnMaterial;
    [SerializeField] private Material lampOffMaterial;
    [SerializeField] private GameObject lampLight;
    [SerializeField] private SpriteRenderer lampSquare;

    private void Awake() {
        lampOnMaterial = lampSquare.material;
    }

    public void LampOn(){
        lampSquare.material = lampOnMaterial;
        lampLight.SetActive(true);
    }

    public void LampOff(){
        lampSquare.material = lampOffMaterial;
        lampLight.SetActive(false);
    }
}
