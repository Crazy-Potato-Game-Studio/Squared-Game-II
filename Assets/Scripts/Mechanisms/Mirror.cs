using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material reflectMaterial;

    private bool hitByLaser;

    public void ChangeMaterial(){
        GetComponent<SpriteRenderer>().material = reflectMaterial;
        hitByLaser = true;
    }

    private void Update() {
        if(!hitByLaser){
            GetComponent<SpriteRenderer>().material = defaultMaterial;
        }
        hitByLaser = false;
    }
}
