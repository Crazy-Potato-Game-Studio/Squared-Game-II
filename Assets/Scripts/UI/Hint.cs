using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public Transform hintObject;
    [SerializeField] private GameObject hintGFX;

    void Update()
    {
        if(hintObject){
            transform.position = new Vector2(hintObject.position.x, hintObject.position.y);
        }else{
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            SetGFX(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            SetGFX(false);
        }
    }

    void SetGFX(bool isOn){
        hintGFX.SetActive(isOn);
    }

}
