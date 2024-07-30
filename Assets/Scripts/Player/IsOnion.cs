using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnion : MonoBehaviour
{
    public static bool isTheOnion;
    [SerializeField] private GameObject playerGFX;
    [SerializeField] private Sprite onion;

    private void Awake() {
        if(isTheOnion){
            playerGFX.GetComponent<SpriteRenderer>().sprite = onion;
        }
    }
    
    public void beOnion(){
        playerGFX.GetComponent<SpriteRenderer>().sprite = onion;
    }
}
