using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hint;
    [SerializeField] private float showDistance;
    private GameObject player;
    private float currentDistance;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        if(CalculateDistance() <= showDistance){
            ShowHint();
        }else{
            HideHint();
        }
    }

    float CalculateDistance(){
        currentDistance = Vector2.Distance(transform.position, player.transform.position);
        return currentDistance;
    }

    void ShowHint(){
        hint.enabled = true;
    }

    void HideHint(){
        hint.enabled = false;
    }
}
